using Scripts.Manager;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Scripts.Entity;
using System;
using TMPro;
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
    public enum BattleState { START, PLAYERTURN, ENEMYTURN, SECONDTURN, END}  //전투상태 열거형
    public enum PlayerTurn {None, Player0, Player1, Player2, Player3, Player4};
    public enum SmallTurnState { START, PROCESSING, END } // 턴 상태 열거형
    public BattleState bState { get; set; }
    public SmallTurnState smallturn { get; set; }

    private Queue<Unit> turnQueue = new Queue<Unit>();
    public int alivePlayer, aliveEnemy;
    
    private int[] agi_rank;

    public ActMenu actmenu;

    public GameObject[] playerStation;
    private GameObject[] playerGO = new GameObject[5];

    public HUDmanager[] HUDs;
    public GameObject[] playerPrefab;
    public HUDmanager[] playerHUD;
    public HUDmanager[] enemyHUD;

    public Transform[] EnemySpawnerPoints = new Transform[4]; // 적 스폰지점 위치 받아오는 변수
    Enemy[] enemyPrefab = new Enemy[4]; // 적을 저장해두는 배열 초기화
    Boss bossPrefab;
    Enemy_Base[] enemy_Base = new Enemy_Base[4];
    int SpawnCount; // 스폰장소 지정 변수
    private Unit[] playerunit = new Unit[6], enemyunit = new Unit[6];
    private SpriteOutline[] playerOutlines = new SpriteOutline[6], enemyOutlines = new SpriteOutline[4];

    public bool isEncounter;

    public int BigTurnCount;
    public TextMeshProUGUI BigTurn;
    public GameObject endcanvas;
    public Material spriteOutline;


    private void Awake()
    {
        SpawnCount = 0;
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
            playerOutlines[i] = playerGO[i].GetComponent<SpriteOutline>();
            playerunit[i] = playerGO[i].GetComponent<Unit>();
            playerunit[i].Connect(this, playerHUD[i]);
        }
        alivePlayer = playerPrefab.Length;

        DB.Instance.UpdateDB(); // DB 불러오는 함수인데 실행 오래걸리니 안쓰면 주석처리

        EnemySpawn(1, "토끼"); // 적 스폰은 나중에 데이터로 처리할수 있게 변경 예정
        EnemySpawn(1, "슬라임");

        actmenu.GetUnitComponent(playerunit, playerOutlines, enemyOutlines);
        /*if (isEncounter) //첫 턴 플로우차트
        {
            if (UnityEngine.Random.value < 0.7f) { bState = BattleState.ENEMYTURN; Debug.Log("적턴"); }
            else { bState = BattleState.SECONDTURN; Debug.Log("그냥 턴"); } 
        }
        else
        {
        }
        */

        smallturn = SmallTurnState.END;
        StartCoroutine("BattleCoroutine");
        FirstTurn();
    }

    private void FirstTurn()
    {
        BigTurnCount = 1;
        PlayerTurnOrder();
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
        try
        {
            GameObject cloneEnemy = new GameObject($"{name}({SpawnCount})");
            cloneEnemy.transform.position = EnemySpawnerPoints[SpawnCount].position;
            cloneEnemy.transform.SetParent(EnemySpawnerPoints[SpawnCount]);
            cloneEnemy.transform.localScale = new Vector3(5, 5, 1); // 게임 오브젝트 생성후 스케일 고정까지

            string sprite_name = Convert.ToString(floor) + "F_" + name;
            SpriteRenderer image = cloneEnemy.AddComponent<SpriteRenderer>(); // 스프라이트 불러오기
            image.sprite = Resources.Load<Sprite>($"BattlePrefabs/EnemySprites/{sprite_name}");
            image.material = spriteOutline;
            enemyOutlines[SpawnCount] = cloneEnemy.AddComponent<SpriteOutline>();
            cloneEnemy.AddComponent<BuffManager>();

            if (boss)
            {
                bossPrefab = new Boss();
                enemy_Base[SpawnCount] = bossPrefab.NewBoss(floor, name,cloneEnemy);
            }
            else
            {
                enemyPrefab[SpawnCount] = new Enemy();
                enemy_Base[SpawnCount] = enemyPrefab[SpawnCount].NewEnemy(floor, name,cloneEnemy);    // 팩토리 패턴으로 에너미 베이스에 에너미 타입 생성 
            }
            enemy_Base[SpawnCount].Connect(this, enemyHUD[SpawnCount]);
            SpawnCount++;
        }
        catch
        {
            Debug.LogError($"BattlePrefabs/EnemySprites/Load문제 발생");
        }
    }

    private void PlayerTurnOrder() //플레이어끼리만 비교해놓음
    {
        Dictionary<Unit,float> agi_ranking = new Dictionary<Unit, float>(); //플레이어끼리 순서 정함

        for (int i = 0; i < 5; i++)
        {
            agi_ranking.Add(playerunit[i],playerunit[i].stat.agi);
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
                            actmenu.TurnStart(turnQueue.Dequeue());
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
                        if (bossPrefab != null && bossPrefab.isDead == false)
                            bossPrefab.Attack();

                        for (int i = 0; i < SpawnCount; i++)
                        {
                            if (enemyPrefab[i] != null && enemyPrefab[i].isDead == false)
                            {
                                enemyPrefab[i].Attack();
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
            if (alivePlayer == 0) Lose();
            yield return new WaitForSeconds(0.5f);
            BigTurn.text = "Turn   " + BigTurnCount.ToString();

            if (alivePlayer == 0) { Lose(); }
            if (SpawnCount == 0) { WIN(); }
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
    public void Attack(int attackedUnit)
    {
        enemy_Base[attackedUnit].TakeDamage(5, 0);
    }

    public void SmallTurnEnd()
    {
        smallturn = SmallTurnState.END;
    }

    public void EnemyDead()
    {
        SpawnCount--;
    }
    public void PlayerDead()
    {
        alivePlayer--;
    }
}
