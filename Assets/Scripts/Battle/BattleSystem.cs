using Scripts.Entity;
using System.Collections;
using UnityEngine;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WIN, LOSE}  //전투상태 열거형

public class BattleSystem : MonoBehaviour
{   
    public BattleState State;

    public GameObject[] playerStation;

    public GameObject[] playerPrefab;

    private HUDmanager[] playerHUD = new HUDmanager[6];
 
    private Unit[] playerunit = new Unit[6], enemyunit = new Unit[6];

    void Start()
    {

        for (int i = 0; i < playerStation.Length; i++)
        {
            Debug.Log("시작");
            playerHUD[i] = playerStation[i].GetComponent<HUDmanager>();
            Debug.Log("HUD" + i + " 완료");
        }

        State = BattleState.START;
        SetupBattle();
    }

    void SetupBattle()
    {
        for(int i = 0; i< playerPrefab.Length; i++)
        {
            GameObject playerGO = Instantiate(playerPrefab[i], playerStation[i].transform); //플레이어 프리펩 생성
            Unit playerunit = playerGO.GetComponent<Unit>();
            playerHUD[i].Setup(playerunit);
            Debug.Log("Player" + i + "세팅 완료" );
        }
        

        State = BattleState.PLAYERTURN; //플레이어 선공
        StartCoroutine(playerturn());
    }

    IEnumerator playerturn()
    {
        while (State == BattleState.PLAYERTURN)
        {
            if (Input.GetKey(KeyCode.A))
            {
                PlayerAttack();
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
        StopCoroutine(playerturn());
    }

    void PlayerAttack()
    {
        Debug.Log("공격");
        State = BattleState.ENEMYTURN;
        EnemyAttack();
    }
    void EnemyAttack()
    {
        Debug.Log("적 공격");
        State = BattleState.PLAYERTURN;
        StartCoroutine(playerturn());
    }

}
