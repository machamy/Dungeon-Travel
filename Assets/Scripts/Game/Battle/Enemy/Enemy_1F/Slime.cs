using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Enemy_Base
{
    EnemyStatData enemyStatData = DB.GetEnemyData("슬라임");
    [HideInInspector]
    public float currentHp;

    public override void Init()
    {
        currentHp = enemyStatData.hp;
    }
    public override void EnemyAttack()
    {
        //기본공격
        AttackType myAttackType = AttackType.Smash;
        SingleAttack(enemyStatData.atk,myAttackType);
    }

    public override void EnemyDamaged()
    {
        
    }
}
