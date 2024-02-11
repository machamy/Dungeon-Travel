using Scripts.Manager;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using System;

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

    public ActMenu actmenu;

    public GameObject[] playerStation;
    private GameObject[] playerGO = new GameObject[5];

    public GameObject[] playerPrefab;
    private HUDmanager[] playerHUD = new HUDmanager[6];

    private Transform[] EnemySpawnerPoints = new Transform[4]; // 적 스폰지점 위치 받아오는 변수
    public Enemy_Base[] enemyPrefab = new Enemy_Base[4];
    int SpawnCount; // 스폰장소 지정 변수
    private Unit[] playerunit = new Unit[6], enemyunit = new Unit[6];

    public bool isEncounter;
    public int TurnCount;
    public GameObject endcanvas;
    private void Awake()
    {
        endcanvas.SetActive(false);
        bState = BattleState.START;

        actmenu.aState = ActMenu.ActState.ChooseAct;

        SpawnCount = 0;
        SetupBattle();
    }


    private void SetupBattle()
    {
        PlayerSpawn();
        actmenu.GetUnit(playerunit[0]);


        // 적 프리펩 불러오기
        for(int i = 0; i < EnemySpawnerPoints.Length; i++) // 적 스폰 위치 받아오기
        {
            EnemySpawnerPoints[i] = GameObject.Find("EnemySpawner" + i).GetComponent<Transform>();
        }
        EnemySpawn(Define_Battle.Enemy_Type.Rabbit); // 적 스폰은 나중에 데이터로 처리할수 있게 변경 예정


        if (isEncounter) //첫 턴 플로우차트
        {
            float random = UnityEngine.Random.value;
            if (random < 0.7f) { bState = BattleState.ENEMYTURN; }
            else { bState = BattleState.SECONDTURN; }
        }
    }

    private void PlayerSpawn() //플레이어 위치에 맞게 소환
    {
        //플레이어 프리펩 불러오기
        for (int i = 0; i < playerPrefab.Length; i++)
        {
            GameObject tempplayerGO = Instantiate(playerPrefab[i], playerStation[i].transform);  //플레이어 프리펩 생성
            Unit tempplayerunit = tempplayerGO.GetComponent<Unit>();
            int position = tempplayerunit.position;
            Destroy(tempplayerGO);  //위치 가져오려고 만들었는데 나중에 고쳐야될듯

            playerGO[i] = Instantiate(playerPrefab[i], playerStation[position].transform);
            playerunit[i] = playerGO[i].GetComponent<Unit>();
            playerunit[i].ConnectHUD(playerStation[position].GetComponent<HUDmanager>());  //유닛스크립트에 HUD 매니저 연결
            Debug.Log("Player" + i + "세팅 완료");
        }
    }
    public void EnemySpawn(Define_Battle.Enemy_Type enemy_Type) // 적 스폰하는 함수 프리펩으로 받아와서 생성
    {
        try
        {
            enemyPrefab[SpawnCount] = Instantiate(Resources.Load<Enemy_Base>($"BattlePrefabs/EnemyPrefabs/{enemy_Type}"));
            enemyPrefab[SpawnCount].transform.position = EnemySpawnerPoints[SpawnCount].position;
            enemyPrefab[SpawnCount].transform.parent = EnemySpawnerPoints[SpawnCount++].transform;
        }
        catch
        {
            Debug.LogError($"Load 문제 발생 BattlePrefabs/EnemyPrefabs/{enemy_Type}확인 바람");
        }
    }

    private void PlayerTurnOrder() //플레이어끼리만 비교해놓음
    {
        Dictionary<Unit, float> agi_ranking = new Dictionary<Unit, float>();

        for(int i = 0; i < 5; i++)
        {
            agi_ranking[playerunit[i]] = playerunit[i].stat.agi;
        }
        agi_ranking = agi_ranking.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

        Debug.Log("Sorted Dictionary:");

        foreach (var kvp in agi_ranking)
        {
            Debug.Log("Key: " + kvp.Key + ", Value: " + kvp.Value);
        }
    }

    private void SecondTurnOrder()
    {
        float enemyAgi = enemyPrefab[0].GetAgi();
    }

    private void Update()
    {
        if(bState == BattleState.START)
        {

        }

        if(bState == BattleState.PLAYERTURN)
        {

        }

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

        if(bState == BattleState.SECONDTURN)
        {

        }

        if(bState == BattleState.WIN)
        {

        }

        if(bState == BattleState.LOSE)
        {

        }

        if (bState == BattleState.END)
        {
            endcanvas.SetActive(true);
            return;
        }
    }

}
