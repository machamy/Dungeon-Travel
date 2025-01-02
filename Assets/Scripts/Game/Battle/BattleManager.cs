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
using System.Security.Cryptography;

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
    public static int alivePlayer { get; set; }
    public static int aliveEnemy {  get; set; }

    public enum BattleState { Starting, Battle, EndBigTurn, EndGame }  //전투상태 열거형
    public enum SmallTurnState { Waiting, Processing } // 턴 상태 열거형
    public BattleState bState { get; set; }
    public SmallTurnState smallTurn { get; set; }

    private Queue<BattleUnit> turnQueue = new Queue<BattleUnit>();

    public CreateUnit createUnit;
    public BackGround backGround;
    public EndCanvas endCanvas;

    private GameObject[] playerGO = new GameObject[6];

    public ActMenu actMenu;

    private BattlePlayerUnit[] battlePlayerUnit;
    private BattleEnemyUnit[] battleEnemyUnit;

    public int BigTurnCount;
    public TextMeshProUGUI BigTurn;

    bool encounter;

    public Action<SkillData> bossPassive;
    public Dictionary<Unit, int> alivePlayers = new Dictionary<Unit, int>();


    private void Awake()
    {
        DB.Instance.UpdateDB();
        Application.targetFrameRate = 30; //30 프레임 고정, 작업 수월하게 하려고
    }

    private void OnEnable()
    {
        encounter = false;
        actMenu.gameObject.SetActive(true);
        backGround.gameObject.SetActive(true); 
        bState = BattleState.Starting;
        StartCoroutine(BattleCoroutine());
        
    }

    private void SetupBattle()
    {
        alivePlayer = 0;
        aliveEnemy = 0;
        BigTurnCount = 0;

        createUnit.InitialUnitSpawn();
        battlePlayerUnit = createUnit.GetPlayerUnit();
        battleEnemyUnit = createUnit.GetEnemyUnit();
        actMenu.Initailize(this, createUnit);

        Debug.Log("SetUp 완료");
    }

    public List<Unit> GetPlayerUnits(TargetType targetType)
    {
        List<Unit> goList = new List<Unit>();
        switch (targetType)
        {
            case TargetType.Single:
                int randomInt = Utility.WeightedRandom(new int[] { 33,33,34 });
                if (battlePlayerUnit[randomInt] != null)
                {
                    //goList.Add(playerUnits[randomInt]);
                }
                else
                {
                    Debug.LogError($"playerUnits[{randomInt}] is null");
                }
                break;
            case TargetType.All:
                break;
            case TargetType.Front:
                break;
            case TargetType.Back:
                break;
        }
        return goList;
    }
    private void PlayerTurnOrder() //플레이어끼리만 비교해놓음
    {
        Dictionary<BattleUnit, float> agi_ranking = new Dictionary<BattleUnit, float>(); //플레이어끼리 순서 정함

        for (int i = 0; i < battlePlayerUnit.Length; i++)
        {
            if (battlePlayerUnit[i] != null)
            {
                if (battlePlayerUnit[i].isDie == false)
                {
                    agi_ranking.Add(battlePlayerUnit[i], battlePlayerUnit[i].data.agi);
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
        Dictionary<BattleUnit, float> agi_ranking = new Dictionary<BattleUnit, float>();

        for(int i = 0;i < battleEnemyUnit.Length; i++)
        {
            if (battleEnemyUnit[i] != null)
            {
                if (battleEnemyUnit[i].isDie == false)
                {
                    agi_ranking.Add(battleEnemyUnit[i], battleEnemyUnit[i].statData.agi);
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
        Dictionary<BattleUnit, float> agi_ranking = new Dictionary<BattleUnit, float>();

        for (int i = 0; i < battlePlayerUnit.Length; i++)
        {
            if (battlePlayerUnit[i] != null)
            {
                if (battlePlayerUnit[i].isDie == false)
                {
                    agi_ranking.Add(battlePlayerUnit[i], battlePlayerUnit[i].data.agi);
                }
            }
        }
        for(int i = 0; i < battleEnemyUnit.Length; i++)
        {
            if (battleEnemyUnit[i] != null)
            {
                if (battleEnemyUnit[i].isDie == false)
                {
                    agi_ranking.Add(battleEnemyUnit[i], battleEnemyUnit[i].data.agi);
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
                        SmallTurnMethod();
                        break;
                    }
                case BattleState.EndBigTurn:
                    {
                        Debug.Log("턴 종료");
                        GeneralTurnOrder();
                        bState = BattleState.Battle;
                        smallTurn = SmallTurnState.Waiting;
                        BigTurnCount++;
                        break;
                    }
                case BattleState.EndGame:
                    {
                        break;
                    }
            }

            if (alivePlayer == 0) Lose();
            if (aliveEnemy == 0) Win();

            yield return new WaitForSeconds(0.1f);
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
                //Debug.Log(turnQueue.Peek().ToString() + " 차례: " + turnQueue.Peek().isEnemy);
                actMenu.TurnStart(turnQueue.Dequeue());
            }

            else if (turnQueue.Peek().isEnemy == true & smallTurn == SmallTurnState.Waiting)
            {
                smallTurn = SmallTurnState.Processing;
                //Debug.Log(turnQueue.Peek().ToString() + " 차례" + turnQueue.Peek().isEnemy);
                turnQueue.Dequeue().Attack();
                EndSmallTurn();
            }
        }
        
    }

    public void EndSmallTurn() { smallTurn = SmallTurnState.Waiting; }

    /// <summary>
    /// 배틀 승리시 실행
    /// </summary>
    public void Win()
    {
        endCanvas.gameObject.SetActive(true);
        Debug.Log("승리");
        StopCoroutine("BattleCoroutine");
    }

    /// <summary>
    /// 배틀 종료시 실행
    /// </summary>
    public void Lose()
    {
        endCanvas.gameObject.SetActive(true);
        Debug.Log("패배");
        StopCoroutine("BattleCoroutine");
    }

    public void Destroy()
    {
        for(int i = 0; battlePlayerUnit[i] != null && i< battlePlayerUnit.Length; i++)
        {
            Destroy(battlePlayerUnit[i].gameObject);
        }
        for (int i = 0; battleEnemyUnit[i] != null && i < battleEnemyUnit.Length; i++)
        {
            Destroy(battleEnemyUnit[i].gameObject);
        }
        alivePlayer = 0;
        aliveEnemy = 0;
        backGround.gameObject.SetActive(false);
        endCanvas.gameObject.SetActive(false);
        actMenu.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    //public Dictionary<Unit, int> GetAlivePlayer()
    //{
    //    for(int i = 0; i < playerUnits.Length;i++)
    //    {
    //        if (playerUnits[i] == null)
    //            continue;
    //        else
    //        {
    //            if (!playerUnits[i].isDead)
    //                alivePlayers.Add(playerUnits[i], i);
    //        }
    //    }
    //    return alivePlayers;
    //}
}