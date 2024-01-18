using System.Collections;
using UnityEngine;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WIN, LOSE}  //전투상태 열거형

public class BattleSystem : MonoBehaviour
{   
    public BattleState State;
    
    public GameObject playerPrefab, enemyPrefab;
    public GameObject playerHUD, enemyHUD;
   
    public Transform playerBattleStation, enemyBattleStation;

    private HPSlider playerHP, enemyHP;

    public Unit playerUnit, enemyUnit;

    void Start()
    {
        State = BattleState.START;
        SetupBattle();
    }

    void SetupBattle()
    {
        GameObject playerGO = Instantiate(playerPrefab, playerBattleStation); //플레이어 프리펩 생성
        playerUnit = playerGO.GetComponent<Unit>();
        playerHP = playerHUD.GetComponentInChildren<HPSlider>();
        playerHP.maxHP = playerUnit.maxHP;
        playerHP.UpdateHPbar(playerHP.maxHP, playerHP.maxHP);
  
        GameObject enemyGO = Instantiate(enemyPrefab, enemyBattleStation); //적 프리펩 생성
        enemyUnit = enemyGO.GetComponent<Unit>();
        enemyHP = enemyHUD.GetComponentInChildren<HPSlider>();
        enemyHP.maxHP = enemyUnit.maxHP;
        enemyHP.UpdateHPbar(enemyHP.maxHP, enemyHP.maxHP);

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

        enemyHP.currentHP -= 10;
        enemyHP.UpdateHPbar(enemyHP.currentHP ,playerHP.maxHP);

        State = BattleState.ENEMYTURN;
        EnemyAttack();
    }
    void EnemyAttack()
    {
        Debug.Log("적 공격");

        playerHP.currentHP -= 10;
        playerHP.UpdateHPbar(playerHP.currentHP, playerHP.maxHP);

        State = BattleState.PLAYERTURN;
        StartCoroutine(playerturn());
    }

}
