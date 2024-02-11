using Scripts.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poison_Mushroom : Enemy_Base
{
    EnemyStatData enemyStatData = DB.GetEnemyData(3, "독버섯");
    [HideInInspector]
    public float currentHp;

    public override void Init()
    {
        currentHp = enemyStatData.hp;
    }
    public override float GetAgi()
    {
        return enemyStatData.agi;
    }
    public override void EnemyAttack()
    {
        BuffManager buffManager = gameObject.GetComponent<BuffManager>();
        if (buffManager.isStun == true)
            return;
        //맹독
        int AttackRange = UnityEngine.Random.Range(0, 5);
        GameObject go = GameObject.Find("Player (" + AttackRange + ")(Clone)");
        BuffManager pBuffManager = go.GetComponent<BuffManager>();
        pBuffManager.Poison(3, enemyStatData.atk, 50); // 확률은 아직
    }

    public override void EnemyDamaged(float atk, AttackType attackType, AttackProperty attackProperty)
    {
        if (enemyStatData.WeakType == attackType)
        {
            atk *= 2f; // 임시
            //크리확률 증가
        }
        if (enemyStatData.ResistType == attackType)
        {
            atk /= 2f; // 임시
        }
        currentHp -= atk;
        if (currentHp <= 0)
        {
            Destroy(gameObject);
            return;
        }
    }
}
