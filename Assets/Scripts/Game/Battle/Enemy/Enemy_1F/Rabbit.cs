using Scripts.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rabbit : Enemy_Base
{
    EnemyStatData enemyStatData = DB.GetEnemyData(1,"토끼");
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
        //기본공격
        SingleAttack(enemyStatData.atk, AttackType.Smash, AttackProperty.Physics);
    }
    public override void EnemyDamaged(float atk, AttackType attackType, AttackProperty attackProperty)
    {
        currentHp -= atk;
        if (currentHp <= 0)
        {
            Destroy(gameObject);
            return;
        }
    }
}
