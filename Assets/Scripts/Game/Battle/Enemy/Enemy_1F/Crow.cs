using Scripts.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crow : Enemy_Base
{
    EnemyStatData enemyStatData = DB.GetEnemyData("까마귀");
    [HideInInspector]
    public float currentHp;

    public override void Init()
    {
        currentHp = enemyStatData.hp;
    }
    public override void EnemyAttack()
    {
        int weight = UnityEngine.Random.Range(0, 99); // 가중치 아직 안건드림
        if (weight < 50)
            weight = 0;
        else
            weight = 1;

        switch (weight)
        {
            case 0: // 기본공격
                AttackType myAttackType = AttackType.Slash;
                SingleAttack(enemyStatData.atk, myAttackType);
                break;
            case 1: // 쪼아대기
                Pecking();
                break;
        }
    }
    public override void EnemyDamaged()
    {

    }

    public void Pecking()
    {
        AttackType myAttackType = AttackType.Penetrate;
        SingleAttack(enemyStatData.atk, myAttackType);
    }
}
