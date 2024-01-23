using Scripts.Manager;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEngine;

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
                }

                instance = bm.AddComponent<BattleManager>();
            }
            return instance;
        }
    }
    #endregion
    public enum BattleState { PLAYERTURN, ENEMYTURN, WIN, LOSE, INBATTLE = PLAYERTURN & ENEMYTURN }  //전투상태 열거형
    public enum TurnState { START, PROCESSING, END } // 턴 상태 열거형
    public bool IsAttacked { get; set; } // 나중에 던전쪽으로 빼는게 나을듯

    private bool isProcessing = false; // processing 상태를 알려주는 변수

    public BattleState bState { get; set; }
    public TurnState tState { get; set; }

    public GameObject[] playerStation;
    private GameObject[] playerGO = new GameObject[5];
    public GameObject[] playerPrefab;
    private HUDmanager[] playerHUD = new HUDmanager[6];

    private Transform[] EnemySpawnerPoints = new Transform[4]; // 적 스폰지점 위치 받아오는 변수
    int SpawnCount; // 스폰장소 지정 변수
    private Unit[] playerunit = new Unit[6], enemyunit = new Unit[6];
    private void Awake()
    {
        bState = BattleState.INBATTLE; // 
        tState = TurnState.START;
        isProcessing = false;
        SpawnCount = 0;
        SetupBattle();
    }


    private void SetupBattle()
    {
        //플레이어 프리펩 불러오기
        for (int i = 0; i < playerPrefab.Length; i++) 
        {
            GameObject tempplayerGO = Instantiate(playerPrefab[i], playerStation[i].transform);  //플레이어 프리펩 생성
            Unit tempplayerunit = tempplayerGO.GetComponent<Unit>();
            int position = tempplayerunit.position;
            Destroy(tempplayerGO);
            Debug.Log("위치: " + position);


            playerGO[i] = Instantiate(playerPrefab[i], playerStation[position].transform);
            playerunit[i] = playerGO[i].GetComponent<Unit>();
            playerunit[i].ConnectHUD(playerStation[position].GetComponent<HUDmanager>());  //유닛스크립트에 HUD 매니저 연결
            Debug.Log("Player" + i + "세팅 완료");
        }
        // 적 프리펩 불러오기
        for(int i = 0; i < EnemySpawnerPoints.Length; i++) // 적 스폰 위치 받아오기
        {
            EnemySpawnerPoints[i] = GameObject.Find("EnemySpawner" + i).GetComponent<Transform>();
        }
        EnemySpawn(Define_Battle.Enemy_Type.Rabbit); // 적 스폰은 나중에 데이터로 처리할수 있게 변경 예정
        StartCoroutine(BattleSequenceCor());
    }
    private void EnemySpawn(Define_Battle.Enemy_Type enemy_Type) // 적 스폰하는 함수 프리펩으로 받아와서 생성
    {
        GameObject enemy;
        try
        {
            enemy = Instantiate(Resources.Load<GameObject>($"BattlePrefabs/EnemyPrefabs/{enemy_Type}"));
            enemy.transform.position = EnemySpawnerPoints[SpawnCount].position;
            enemy.transform.parent = EnemySpawnerPoints[SpawnCount++].transform;
        }
        catch
        {
            Debug.LogError($"Load 문제 발생 BattlePrefabs/EnemyPrefabs/{enemy_Type}확인 바람");
        }
    }

    private void Update()
    {
        if(bState == BattleState.INBATTLE) // 전투상황
        {
            if (tState == TurnState.START) //전투 시작 처리
            {
                tState = TurnState.PROCESSING;
            }
            else if (tState == TurnState.PROCESSING) // 전투 중 처리
            {
                if(!isProcessing)
                {
                    isProcessing = true;
                    
                }
            }
            else if (tState == TurnState.END) // 전투 마침 처리
            {

            }
        }
        else if(bState == BattleState.WIN) // 전투 승리 처리
        {

        }
        else if(bState == BattleState.LOSE) // 전투 패배 처리
        {

        }
    }

    void TurnPriority() // 선공권 처리 함수
    {
        if(true) // 플레이어의 민첩이 높을때 // 임시코드
        {
            bState = BattleState.PLAYERTURN;

        }
        else // 적의 민첩이 높을때
        {
            bState = BattleState.ENEMYTURN;

        }
    }
    IEnumerator BattleSequenceCor()
    {
        if(IsAttacked == false) // 적 심볼과 충돌여부 처리
        {
                            //70퍼 확률로 적이 공격(미구현)
            TurnPriority();
        }
        else
        {
                            //우리팀 공격권 한번 처리(미구현)
            TurnPriority();
        }

        if (bState == BattleState.PLAYERTURN) // 플레이어 턴
        {
            yield return selectAction(); // 플레이어 행동 코루틴 실행
            if(true) //적 체력이 0일때 // 임시코드
            {
                bState = BattleState.WIN;
            }
            bState = BattleState.ENEMYTURN;
        }
        else if (bState == BattleState.ENEMYTURN)
        {
            yield return enemySelectAction(); // 적 행동 코루틴 실행
            if(true) // 플레이어 체력이 0일때 // 임시코드
            {
                bState = BattleState.LOSE;
            }
            bState = BattleState.PLAYERTURN;
        }
        
    }


    IEnumerator selectAction() // 임시함수
    {
        while (bState == BattleState.PLAYERTURN)
        {
            if (Input.GetKey(KeyCode.A))
            {

            }
            if (Input.GetKey(KeyCode.B))
            {

            }
            if (Input.GetKey(KeyCode.X))
            {

            }
            if (Input.GetKey(KeyCode.Y))
            {

            }
            yield return null;
        }
    }

    IEnumerator enemySelectAction() // 임시함수
    {
        yield return null;
    }

}
