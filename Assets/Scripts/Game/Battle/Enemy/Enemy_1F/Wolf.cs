using Scripts.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wolf : Enemy_Base
{
    EnemyStatData enemyStatData = DB.GetEnemyData("늑대");
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
            case 1: // 물어뜯기
                Bite();
                break;
        }
    }
    public override void EnemyDamaged()
    {

    }

    public void Bite()
    {
        AttackType myAttackType = AttackType.Penetrate;
        SingleAttack(enemyStatData.atk, myAttackType);
        // 낮은 확률로 출혈
    }
}
