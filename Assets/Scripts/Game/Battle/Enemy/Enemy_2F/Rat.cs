using Scripts.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rat : Enemy_Base
{
    EnemyStatData enemyStatData = DB.GetEnemyData("쥐");
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
        //기본공격
        SingleAttack(enemyStatData.atk, AttackType.Penetrate, AttackProperty.Physics);
    }

    public override void EnemyDamaged(float atk, AttackType attackType, AttackProperty attackProperty)
    {
        currentHp -= atk;
        if (currentHp <= 0)
        {
            Destroy(gameObject);
        }
    }
}