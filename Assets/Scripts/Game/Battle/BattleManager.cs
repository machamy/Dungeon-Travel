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
    private int alivePlayer;
    public int aliveEnemy;

    Character character;

    private int[] agi_rank;

    public ActMenu actMenu;

    public GameObject[] playerStation;
    private GameObject[] playerGO = new GameObject[5];
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
    public GameObject endcanvas;
    public Material spriteOutline;


    private void Awake()
    {
        DB.Instance.UpdateDB(); // DB 불러오는 함수인데 실행 오래걸리니 안쓰면 주석처리
        spawnCount = 0;
        bState = BattleState.START;
        endcanvas.SetActive(false);
        SetupBattle();
    }

    private void SetupBattle()
    {
        //플레이어 프리펩 불러오기
        for (int i = 0; i < playerPrefab.Length; i++)
        {
            playerGO[i] = Instantiate(playerPrefab[i], playerStation[i].transform);
            playerUnits[i] = playerGO[i].GetComponent<Unit>();
            character = DataManager.Instance.party.GetCharacter(i);
            playerUnits[i].InitialSetting(this, playerHUD[i],character);
        }
        alivePlayer = playerPrefab.Length;

        EnemySpawn(1, "토끼"); // 적 스폰은 나중에 데이터로 처리할수 있게 변경 예정
        EnemySpawn(1, "슬라임");
        aliveEnemy = spawnCount;

        actMenu.SetUnits(playerUnits, enemyUnits);
        actMenu.SetBM(this);
        PlayerTurnOrder();
        StartCoroutine("BattleCoroutine");


        BigTurnCount = 1;
        smallturn = SmallTurnState.END;
        bState = BattleState.PLAYERTURN;
    }

    /// <summary>
    /// 적을 스폰하는 함수
    /// </summary>
    /// <param name="floor"></param>
    /// <param name="name"></param>
    /// <param name="boss"> 만약 보스 객체라면 3번째 인자로 true </param>
    public void EnemySpawn(int floor, string name, bool boss = false) // 적 스폰하는 함수 프리펩으로 받아와서 생성
    {
        //게임오브젝트 생성 및 컴포넌트 추가
        GameObject cloneEnemy = new GameObject($"{name}({spawnCount})");
        SpriteRenderer image = cloneEnemy.AddComponent<SpriteRenderer>();
        cloneEnemy.AddComponent<BuffManager>();
        enemyUnits[spawnCount] = cloneEnemy.AddComponent<Unit>();

        //게임 오브젝트 위치설정
        cloneEnemy.transform.position = EnemySpawnerPoints[spawnCount].position;
        cloneEnemy.transform.SetParent(EnemySpawnerPoints[spawnCount]);
        cloneEnemy.transform.localScale = new Vector3(5, 5, 1);

        //SpriteRenderer 설정
        string sprite_name = Convert.ToString(floor) + "F_" + name;
        image.sprite = Resources.Load<Sprite>($"BattlePrefabs/EnemySprites/{sprite_name}");
        image.material = spriteOutline;

        //Unit컴포넌트 초기설정
        enemyUnits[spawnCount].InitialSetting(this, enemyHUD[spawnCount]);

        if (boss)
        {
            Boss bossPrefab = new Boss();
            bossPrefab.NewEnemy(floor, name, cloneEnemy, this);
            enemyUnits[spawnCount].EnemySetting(bossPrefab);
        }
        else
        {
            Enemy enemyPrefab = new Enemy();
            enemyPrefab.NewEnemy(floor, name, cloneEnemy, this);    // 팩토리 패턴으로 에너미 베이스에 에너미 타입 생성
            enemyUnits[spawnCount].EnemySetting(enemyPrefab);
        }
        enemyGOList.Add(cloneEnemy);
        spawnCount++;
    }

    private void PlayerTurnOrder() //플레이어끼리만 비교해놓음
    {
        Dictionary<int, float> agi_ranking = new Dictionary<int, float>(); //플레이어끼리 순서 정함

        for (int i = 0; i < 5; i++)
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

    IEnumerator BattleCoroutine()
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

            BigTurn.text = "Turn   " + BigTurnCount.ToString();

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
        endcanvas.SetActive(true);
        Debug.Log("승리");
        StopCoroutine("BattleCoroutine");
    }

    /// <summary>
    /// 배틀 종료시 실행
    /// </summary>
    public void Lose()
    {
        endcanvas.SetActive(true);
        Debug.Log("패배");
        StopCoroutine("BattleCoroutine");
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
    /// <summary>
    /// 공격 타입에 따라서 플레이어의 게임오브젝트를 받아오는 함수
    /// </summary>
    /// <param name="enemyTargetType"></param>
    /// <returns></returns>
    public GameObject[] GetPlayerGO(TargetType enemyTargetType)
    {
        GameObject[] go = new GameObject[5];
        switch (enemyTargetType)
        {
            case TargetType.Single:
                int AttackRange = Utility.WeightedRandom(20, 20, 20, 20, 20);
                GameObject clone = playerGO[AttackRange];
                go[0] = clone;
                break;
            case TargetType.Front:
                break;
            case TargetType.Back:
                break;
            case TargetType.Area:
                for (int i = 0; i < 5; i++)
                {
                    GameObject _clone = playerGO[i];
                    go[i] = _clone;
                }
                break;
        }
        return go;
    }
    /// <summary>
    /// 공격 타입에 따라서 적의 게임오브젝트를 받아오는 함수
    /// </summary>
    /// <param name="enemyTargetType"></param>
    /// <returns></returns>
    public GameObject[] GetEnemyGO(TargetType enemyTargetType)
    {
        GameObject[] go = new GameObject[4];
        switch (enemyTargetType)
        {
            case TargetType.Single:
                int AttackRange = Utility.WeightedRandom(25, 25, 25, 25);
                go[0] = enemyGOList[AttackRange];
                break;
            case TargetType.Front:
                break;
            case TargetType.Back:
                break;
            case TargetType.Area:
                for (int i = 0; i < enemyGOList.Count; i++)
                {
                    GameObject _clone = enemyGOList[i];
                    go[i] = _clone;
                }
                break;
        }
        return go;
    }
    public void EndSmallTurn() { smallturn = SmallTurnState.END; }

    public void EnemyDead() { aliveEnemy--; spawnCount--; }
    public void PlayerDead() { alivePlayer--; }
}