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
    public enum BattleState { START, PLAYERTURN, ENEMYTURN, SECONDTURN, WIN, LOSE, END}  //전투상태 열거형
    public enum PlayerTurn {None, Player0, Player1, Player2, Player3, Player4};
    public enum TurnState { START, PROCESSING, END } // 턴 상태 열거형
    public BattleState bState { get; set; }

    public List<Unit> playerTurnOrder;

    public ActMenu actmenu;

    public GameObject[] playerStation;
    private GameObject[] playerGO = new GameObject[5];

    public HUDmanager[] HUDs = new HUDmanager[6];
    public GameObject[] playerPrefab;
    public HUDmanager[] playerHUD = new HUDmanager[6];

    public Transform[] EnemySpawnerPoints = new Transform[4]; // 적 스폰지점 위치 받아오는 변수
    public Enemy[] enemyPrefab = new Enemy[4]; // 적을 저장해두는 배열 초기화
    private Enemy_Base enemy_Base = null;
    int SpawnCount; // 스폰장소 지정 변수
    private Unit[] playerunit = new Unit[6], enemyunit = new Unit[6];
    private SpriteOutline[] playeroutline = new SpriteOutline[6];

    public bool isEncounter;

    public int TurnCount;
    public TextMeshProUGUI Turn;
    public GameObject endcanvas;
    private void Awake()
    {
        bState = BattleState.START;
        SpawnCount = 0;
        for (int i = 0; i < 4;i++)
        {
            enemyPrefab[i] = new Enemy(); // 배열에 객체 생성
        }
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

        if (isEncounter) //첫 턴 플로우차트
        {
            float random = UnityEngine.Random.value;
            if (random < 0.7f) { bState = BattleState.ENEMYTURN; } //테스트하려고 SecondTurn으로 바꿔놨어요 원래는 Enemyturn
            else { bState = BattleState.SECONDTURN; } 
        }

        PlayerTurnOrder();
        Debug.Log("스폰 완료");

        actmenu.TurnStart(playerTurnOrder[0]);
    }

    public void EnemySpawn(int floor, string name) // 적 스폰하는 함수 프리펩으로 받아와서 생성
    {
        try
        {
            GameObject cloneEnemy = new GameObject($"Clone({SpawnCount})");
            var newEnemy = Instantiate(cloneEnemy, EnemySpawnerPoints[SpawnCount].position, Quaternion.identity);
            newEnemy.transform.parent = EnemySpawnerPoints[SpawnCount].transform;
            newEnemy.transform.localScale = new Vector3(5, 5, 1); // 게임 오브젝트 생성후 스케일 고정까지

            enemy_Base = enemyPrefab[SpawnCount++].NewEnemy(floor, name); // 팩토리 패턴으로 에너미 베이스에 에너미 타입 생성
            Debug.Log(enemy_Base.hp);

            string sprite_name = Convert.ToString(floor) + "F_" + name;
            SpriteRenderer image = newEnemy.AddComponent<SpriteRenderer>(); // 스프라이트 불러오기
            image.sprite = Resources.Load<Sprite>($"BattlePrefabs/EnemySprites/{sprite_name}");
            newEnemy.AddComponent<BuffManager>();
        }
        catch
        {
            Debug.LogError($"BattlePrefabs/EnemySprites/Load문제 발생");
        }
    }

    private List<Unit> PlayerTurnOrder() //플레이어끼리만 비교해놓음
    {
        playerTurnOrder = new List<Unit>();
        Dictionary<Unit, float> agi_ranking = new Dictionary<Unit, float>();

        for(int i = 0; i < 5; i++)
        {
            agi_ranking.Add(playerunit[i], playerunit[i].stat.agi);
        }

        agi_ranking = agi_ranking.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

        foreach (var kvp in agi_ranking)
        {
            Debug.Log("Key: " + kvp.Key + ", Value: " + kvp.Value);

            playerTurnOrder.Add(kvp.Key);
        }

        return playerTurnOrder;
    }

    private void FirstTurn()
    {
        PlayerTurnOrder();
        actmenu.TurnStart(playerTurnOrder[0]);
    }
    private void SecondTurnOrder()
    {
        
    }
    
    private void Update()
    {
        switch(bState)
        {
            case BattleState.START:
                {
                    TurnCount = 0;
                    endcanvas.SetActive(false);
                    SetupBattle();
                    break;
                }
            case BattleState.PLAYERTURN:
                {
                    break;
                }
            case BattleState.ENEMYTURN:
                {
                    enemyPrefab[0].Attack();
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

        /*
        if(bState == BattleState.ENEMYTURN)
        {
            Enemy_Base[] live_enemy = new Enemy_Base[enemyPrefab.Length];
            int count = 0;
            for(int i =0; i<SpawnCount;i++)
            {
              if (enemyPrefab[i].isDead == false)
                    live_enemy[count++] = enemyPrefab[i];
            }
            for (int i =0; i < count; i++)
            {
                live_enemy[i].EnemyAttack();
            }
            bState = BattleState.PLAYERTURN;
        }
        */
    }

    public void Attack(Unit attackplayer, Unit damagedplayer, BattleSkill useSkill)
    {
        if (attackplayer.ConsumeMP(useSkill.Cost))
        {
            //damagedplayer.ConsumeMP(useSkill.damage);
        }
    }
}
