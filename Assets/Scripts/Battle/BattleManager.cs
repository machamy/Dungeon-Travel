using Scripts.Manager;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
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
                GameObject bm = GameObject.Find("@Battle");
                if (bm == null)
                {
                    bm = new GameObject("@Battle");
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

    public bool isProcessing = false; // processing 상태를 알려주는 변수

    public BattleState bState { get; set; }
    public TurnState tState { get; set; }
    private void Awake()
    {
        bState = BattleState.INBATTLE; // 
        tState = TurnState.START;
        isProcessing = false;
    }
    private void Start()
    {

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
                    SetupBattle();
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
    void SetupBattle()
    {
                            // 플레이어, 적 프리펩 불러오기 등등(미구현)
        StartCoroutine(BattleSequenceCor());
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
