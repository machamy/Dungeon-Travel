using Scripts.Manager;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
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

    public enum BattleState { START, PLAYERTURN, ENEMYTURN, SECONDTURN, END }  //전투상태 열거형
    public enum SmallTurnState { START, PROCESSING, END } // 턴 상태 열거형
    public BattleState bState { get; set; }
    public SmallTurnState smallturn { get; set; }

    private Queue<int> turnQueue = new Queue<int>();
    private int[] agi_rank;

    private int alivePlayer;
    public int aliveEnemy;

    Character character;

    public UnitSpawn unitSpawn;
    public BackGround backGround;
    public EndBattle endBattle;

    private StationController[] stationController = new StationController[10];
    private GameObject[] playerGO = new GameObject[6];

    public ActMenu actMenu;
    public EventSystem eventSystem;

    private List<GameObject> enemyGOList = new List<GameObject>();

    public GameObject[] playerPrefab;

    public HUDmanager[] playerHUD;
    public HUDmanager[] enemyHUD;

    public Transform[] EnemySpawnerPoints = new Transform[4]; // 적 스폰지점 위치 받아오는 변수
    int spawnCount; // 스폰장소 지정 변수
    private Unit[] playerUnits = new Unit[6], enemyUnits = new Unit[6];

    public bool isEncounter;

    public int BigTurnCount;
    public TextMeshProUGUI BigTurn;
    public Material spriteOutline;


    private void Awake()
    {
        DB.Instance.UpdateDB(); // DB 불러오는 함수인데 실행 오래걸리니 안쓰면 주석처리
        SetupBattle();
        StartCoroutine(BattleCoroutine());
    }

    private void SetupBattle()
    {
        spawnCount = 0;

        int spawnPlayer = 0;
        for (; spawnPlayer < 3; spawnPlayer++) unitSpawn.SpawnPlayerUnit();
        playerUnits = unitSpawn.GetPlayerUnit();


        alivePlayer = spawnPlayer;

        unitSpawn.SpawnEnemyUnit(1, "토끼"); // 적 스폰은 나중에 데이터로 처리할수 있게 변경 예정
        unitSpawn.SpawnEnemyUnit(1, "슬라임");
        enemyUnits = unitSpawn.GetEnemyUnit();
        aliveEnemy = spawnCount;

        actMenu.SetUnits(playerUnits, enemyUnits);
        actMenu.SetUp(this, eventSystem, unitSpawn);
        PlayerTurnOrder();

        BigTurnCount = 1;
        smallturn = SmallTurnState.END;
        bState = BattleState.PLAYERTURN;
    }

    public GameObject[] GetPlayerGO(TargetType playerTargetType)
    {
        return playerGO;
    }

    private void PlayerTurnOrder() //플레이어끼리만 비교해놓음
    {
        Dictionary<int, float> agi_ranking = new Dictionary<int, float>(); //플레이어끼리 순서 정함

        for (int i = 0; i < alivePlayer; i++)
        {
            agi_ranking.Add(i, playerUnits[i].stat.agi);
        }

        var sortedDict = agi_ranking.OrderByDescending(x => x.Value);

        foreach (var kvp in agi_ranking)
        {
            turnQueue.Enqueue(kvp.Key);
        }
    }

    public void EndHalfTurn()
    {
        if (bState == BattleState.PLAYERTURN) bState = BattleState.ENEMYTURN;
        else if (bState == BattleState.ENEMYTURN)
        {
            bState = BattleState.PLAYERTURN;
            BigTurnCount++;
        }
        smallturn = SmallTurnState.START;
    }

    public IEnumerator BattleCoroutine()
    {
        while (true)
        {
            switch (bState)
            {
                case BattleState.START:
                    {
                        break;
                    }
                case BattleState.PLAYERTURN:
                    {
                        if (turnQueue.Count > 0)
                        {
                            Debug.Log(turnQueue.Peek().ToString() + " 차례");
                            actMenu.TurnStart(turnQueue.Dequeue());
                            smallturn = SmallTurnState.PROCESSING;
                            yield return new WaitUntil(() => smallturn != SmallTurnState.PROCESSING);
                        }
                        else if (turnQueue.Count <= 0)
                        {
                            EndHalfTurn();
                        }
                        break;
                    }

                case BattleState.ENEMYTURN:
                    {
                        for (int i = 0; i < spawnCount; i++)
                        {
                            if (enemyUnits[i] != null && !enemyUnits[i].IsDead())
                            {
                                enemyUnits[i].Attack();
                            }
                        }
                        PlayerTurnOrder();
                        EndHalfTurn();
                        break;
                    }

                case BattleState.SECONDTURN:
                    {
                        break;
                    }
                default:
                    {
                        break;
                    }
            }

            //BigTurn.text = "Turn   " + BigTurnCount.ToString();

            if (alivePlayer == 0) { Lose(); }
            if (aliveEnemy == 0) { WIN(); }

            yield return new WaitForSeconds(0.5f);
        }
    }

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

    public void Attack(Unit attackUnit, Unit targetUnit)
    {
        targetUnit.TakeDamage(5);
    }
    public void SkillAttack(Unit attackUnit, Unit targetUnit, SkillData useSkill)
    {
        float totalDamage = useSkill.physicsDamage; ;

        bool critical;
        bool isAround = false;

        if(isAround) { totalDamage = totalDamage * 0.8f; }
        if (useSkill.attackType == targetUnit.weakType)
        {
            critical = Random(attackUnit.stat.critical + 0.5);
            totalDamage *= 1.1f;
        }
        else
        {
            critical = Random(attackUnit.stat.critical + 0.5);
        }

        if (critical) { totalDamage *= 2.0f; }

        targetUnit.TakeDamage(totalDamage);
    }

    private bool Random(double probability)
    {
        System.Random random = new System.Random();
        double randomValue = random.NextDouble();
        return randomValue < probability;
    }

    public void EndSmallTurn() { smallturn = SmallTurnState.END; }

    public void EnemyDead() { aliveEnemy--; spawnCount--; }
    public void PlayerDead() { alivePlayer--; }
}