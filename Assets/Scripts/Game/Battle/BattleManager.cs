using Scripts.Manager;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine.UI;
using Scripts.Entity;
using Scripts.Data;
using System;
using TMPro;
using Scripts;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;
using UnityEditor;
using static BattleManager;
using UnityEngine.ProBuilder.MeshOperations;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Processors;

public class BattleManager : MonoBehaviour
{
    #region 싱글톤
    private static BattleManager instance = null;
    public static BattleManager Instance
    {
        get
        {
            // 없을경우 생성
            if (instance == null)
            {
                GameObject bm = GameObject.Find("BattleSystem");
                if (bm == null)
                {
                    bm = new GameObject("BattleSystem");
                    instance = bm.AddComponent<BattleManager>();
                }
            }
            return instance;
        }
    }
    #endregion

    public enum BattleState { Starting, Battle, EndBigTurn, EndGame }  //전투상태 열거형
    public enum SmallTurnState { Waiting, Processing } // 턴 상태 열거형
    public BattleState bState { get; set; }
    public SmallTurnState smallTurn { get; set; }

    private Queue<Unit> turnQueue = new Queue<Unit>();

    Character character;

    public UnitSpawn unitSpawn;
    public BackGround backGround;
    public EndBattle endBattle;

    private GameObject[] playerGO = new GameObject[6];

    public ActMenu actMenu;
    public EventSystem eventSystem;

    public HUDmanager[] playerHUD;
    public HUDmanager[] enemyHUD;

    private Unit[] playerUnits = new Unit[6];
    private Unit[] enemyUnits = new Unit[6];

    public int BigTurnCount;
    public TextMeshProUGUI BigTurn;

    public int alivePlayer;
    public int aliveEnemy;
    bool encounter;

    private void Awake()
    {
        encounter = false;
        DB.Instance.UpdateDB(); // DB 불러오는 함수인데 실행 오래걸리니 안쓰면 주석처리
        bState = BattleState.Starting;
        StartCoroutine(BattleCoroutine());
        
    }

    private void SetupBattle()
    {
        alivePlayer = 0;
        aliveEnemy = 0;
        BigTurnCount = 0;

        for (int i = 0; i < 3; i++)
        {
            unitSpawn.SpawnPlayerUnit();
            playerUnits = unitSpawn.GetPlayerUnit();
            alivePlayer = unitSpawn.PlayerCount;
        }

        unitSpawn.SpawnEnemyUnit(1, "토끼");
        unitSpawn.SpawnEnemyUnit(1, "슬라임");
        enemyUnits = unitSpawn.GetEnemyUnit();
        aliveEnemy = unitSpawn.EnemyCount;
        actMenu.SetUp(this, eventSystem, unitSpawn);

        Debug.Log("SetUp 완료");
    }

    public GameObject[] GetPlayerGO(TargetType playerTargetType)
    {
        return playerGO;
    }

    private void PlayerTurnOrder() //플레이어끼리만 비교해놓음
    {
        Dictionary<Unit, float> agi_ranking = new Dictionary<Unit, float>(); //플레이어끼리 순서 정함

        for (int i = 0; i < 6; i++)
        {
            if (playerUnits[i] != null)
            {
                if (playerUnits[i].isDead == false)
                {
                    agi_ranking.Add(playerUnits[i], playerUnits[i].stat.agi);
                }
            }        
        }

        var sortedDict = agi_ranking.OrderByDescending(x => x.Value);

        foreach (var kvp in agi_ranking)
        {
            turnQueue.Enqueue(kvp.Key);
        }
    }

    void EnemyTurnOrder()
    {
        Dictionary<Unit, float> agi_ranking = new Dictionary<Unit, float>();

        for(int i = 0;i < 6; i++)
        {
            if (enemyUnits[i] != null)
            {
                if (enemyUnits[i].isDead == false)
                {
                    agi_ranking.Add(enemyUnits[i], enemyUnits[i].stat.agi);
                } 
            }
                
        }

        var soredDict = agi_ranking.OrderByDescending(x => x.Value);

        foreach(var kvp in agi_ranking)
        {
            turnQueue.Enqueue(kvp.Key);
        }
    }
    void GeneralTurnOrder()
    {
        Dictionary<Unit, float> agi_ranking = new Dictionary<Unit, float>();

        for (int i = 0; i < 6; i++)
        {
            if (playerUnits[i] != null)
            {
                if (playerUnits[i].isDead == false)
                {
                    agi_ranking.Add(playerUnits[i], playerUnits[i].stat.agi);
                }  
            }
                
            if (enemyUnits[i] != null)
            {
                if (enemyUnits[i].isDead == false)
                {
                    agi_ranking.Add(enemyUnits[i], enemyUnits[i].stat.agi);
                }
            }
               
        }

        var sortedDict = agi_ranking.OrderByDescending(x => x.Value);

        foreach (var kvp in agi_ranking)
        {
            turnQueue.Enqueue(kvp.Key);
        }
    }
    void FirstTurnMethod()
    {
        if (encounter)
        {
            // 0부터 100 사이의 무작위 값 생성
            int randomValue = UnityEngine.Random.Range(0, 100); // Random.Range(int, int)

            if (randomValue < 70) //70% 확률로 실행
            {
                EnemyTurnOrder();
                PlayerTurnOrder();
            }
            else //30% 확률로 실행  
            {
                GeneralTurnOrder();
            }
        }
        else
        {
            PlayerTurnOrder();
            EnemyTurnOrder();
        }
    }

    IEnumerator BattleCoroutine()
    {
        while (true)
        {
            switch (bState)
            {
                case BattleState.Starting:
                    {
                        SetupBattle();
                        FirstTurnMethod();

                        smallTurn = SmallTurnState.Waiting;
                        bState = BattleState.Battle; 
                        break;
                    }

                case BattleState.Battle:
                    {
                        Debug.Log("배틀 시작 <"  + turnQueue.Count + ">");
                        SmallTurnMethod();
                        break;
                    }
                case BattleState.EndBigTurn:
                    {
                        Debug.Log("턴 종료");
                        GeneralTurnOrder();
                        BigTurnCount++;
                        break;
                    }
                case BattleState.EndGame:
                    {
                        break;
                    }
            }

            yield return new WaitForSeconds(0.5f);
        }
    }

    void SmallTurnMethod()
    {
        if ( turnQueue.Count == 0 & smallTurn == SmallTurnState.Waiting) // 큐에 아무것도 들어있지 않을때 실행
        {
            bState = BattleState.EndBigTurn;
        }

        if (turnQueue.Count > 0)
        {
            if (turnQueue.Peek().isEnemy == false & smallTurn == SmallTurnState.Waiting)
            {
                smallTurn = SmallTurnState.Processing;
                Debug.Log(turnQueue.Peek().ToString() + " 차례: " + turnQueue.Peek().isEnemy);
                actMenu.TurnStart(turnQueue.Dequeue());
            }

            else if (turnQueue.Peek().isEnemy == true & smallTurn == SmallTurnState.Waiting)
            {
                smallTurn = SmallTurnState.Processing;
                Debug.Log(turnQueue.Peek().ToString() + " 차례");
                turnQueue.Dequeue().Attack();
                EndSmallTurn();
            }
        }
    }

    public void EndSmallTurn() { smallTurn = SmallTurnState.Waiting; }

    /// <summary>
    /// 배틀 승리시 실행
    /// </summary>
    public void WIN()
    {
        endBattle.gameObject.SetActive(true);
        Debug.Log("승리");
        StopCoroutine("BattleCoroutine");
        EndBattle();
    }

    /// <summary>
    /// 배틀 종료시 실행
    /// </summary>
    public void Lose()
    {
        endBattle.gameObject.SetActive(true);
        Debug.Log("패배");
        StopCoroutine("BattleCoroutine");
        EndBattle();
    }

    public void EndBattle()
    {
        backGround.gameObject.SetActive(false);
        actMenu.gameObject.SetActive(false);
        unitSpawn.gameObject.SetActive(false);
        endBattle.gameObject.SetActive(true);
    }

    
}