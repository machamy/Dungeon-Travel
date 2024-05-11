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
    public enum BattleState { PLAYERTURN, ENEMYTURN, SECONDTURN, WIN, LOSE, END}  //전투상태 열거형
    public enum PlayerTurn {None, Player0, Player1, Player2, Player3, Player4};
    public enum TurnState { START, PROCESSING, END } // 턴 상태 열거형
    public BattleState bState { get; set; }
    public TurnState tState { get; set; }

    private Queue<Unit> turnQueue = new Queue<Unit>();
    
    private int[] agi_rank;

    public ActMenu actmenu;

    public GameObject[] playerStation;
    private GameObject[] playerGO = new GameObject[5];

    public HUDmanager[] HUDs = new HUDmanager[6];
    public GameObject[] playerPrefab;
    public HUDmanager[] playerHUD = new HUDmanager[6];

    public Transform[] EnemySpawnerPoints = new Transform[4]; // 적 스폰지점 위치 받아오는 변수
    Enemy[] enemyPrefab = new Enemy[4]; // 적을 저장해두는 배열 초기화
    Boss bossPrefab;
    Enemy_Base enemy_Base = null;
    int SpawnCount; // 스폰장소 지정 변수
    private Unit[] playerunit = new Unit[6], enemyunit = new Unit[6];
    private SpriteOutline[] playeroutline = new SpriteOutline[6];

    public bool isEncounter;

    public int TurnCount;
    public TextMeshProUGUI Turn;
    public GameObject endcanvas;


    private void Awake()
    {
        SetupBattle();
    }

    private void Start()
    {
        tState = TurnState.END;
        SpawnCount = 0;
    }
    private void SetupBattle()
    {
        //플레이어 프리펩 불러오기
        for (int i = 0; i < playerPrefab.Length; i++)
        {
            playerGO[i] = Instantiate(playerPrefab[i], playerStation[i].transform);
            playeroutline[i] = playerGO[i].GetComponent<SpriteOutline>();
            playerunit[i] = playerGO[i].GetComponent<Unit>();
            playerunit[i].ConnectHUD(HUDs[i]);
        }
        actmenu.GetUnitComp(playerunit, playeroutline);

        DB.Instance.UpdateDB(); // DB 불러오는 함수인데 실행 오래걸리니 안쓰면 주석처리

        EnemySpawn(1, "토끼"); // 적 스폰은 나중에 데이터로 처리할수 있게 변경 예정
        EnemySpawn(1, "슬라임");

        /*if (isEncounter) //첫 턴 플로우차트
        {
            if (UnityEngine.Random.value < 0.7f) { bState = BattleState.ENEMYTURN; Debug.Log("적턴"); } //테스트하려고 SecondTurn으로 바꿔놨어요 원래는 Enemyturn
            else { bState = BattleState.SECONDTURN; Debug.Log("그냥 턴"); } 
        }
        else
        {
        }
        */

        Debug.Log("SetUpBattle 끝");

        FirstTurn();
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
            cloneEnemy.AddComponent<BuffManager>();

            if (boss)
            {
                bossPrefab = new Boss();
                enemy_Base = bossPrefab.NewBoss(floor, name,cloneEnemy);
                SpawnCount++;
            }
            else
            {
                enemyPrefab[SpawnCount] = new Enemy();
                enemy_Base = enemyPrefab[SpawnCount++].NewEnemy(floor, name,cloneEnemy); // 팩토리 패턴으로 에너미 베이스에 에너미 타입 생성
            }
            Debug.Log(enemy_Base.hp);
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
            Debug.Log("Key: " + kvp.Key + ", Value: " + kvp.Value);

            turnQueue.Enqueue(kvp.Key);
        }
    }

    private void FirstTurn()
    {
        PlayerTurnOrder();
    }

    private void SecondTurnOrder()
    {
        
    }
    
    public void EndTurn()
    {
        tState = TurnState.END;
    }

    private void Update()
    {
        switch (bState)
        {
            case BattleState.PLAYERTURN:
                {
                    if (turnQueue.Count > 0 && tState == TurnState.END)
                    {
                        Debug.Log(turnQueue.Peek().ToString() + " 차례");
                        actmenu.TurnStart(turnQueue.Dequeue());
                        tState = TurnState.PROCESSING;
                    }
                    else if(turnQueue.Count <= 0)
                    {
                        bState = BattleState.ENEMYTURN;
                    }
                    break;
                }

            case BattleState.ENEMYTURN:
                {
                    if (bossPrefab != null && bossPrefab.isDead == false)
                        bossPrefab.Attack();
                    for (int i = 0; i < enemyPrefab.Length; i++)
                    {
                        if (enemyPrefab[i] != null && enemyPrefab[i].isDead == false)
                            enemyPrefab[i].Attack();

                    }

                    PlayerTurnOrder();
                    bState = BattleState.PLAYERTURN;
                    break;
                }

            case BattleState.SECONDTURN:
                {
                    break;
                }

            case BattleState.END:
                {
                    endcanvas.SetActive(true);
                    return;
                }
            default:
                {
                    break;
                }
        }
        Turn.text = "Turn   " + TurnCount.ToString();
    }

    public void Attack(Unit attackplayer, Unit damagedplayer, BattleSkill usedSkill)
    {
        
    }
}
