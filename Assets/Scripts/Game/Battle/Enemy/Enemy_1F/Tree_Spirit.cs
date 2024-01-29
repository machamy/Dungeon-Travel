using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;

public class Tree_Spirit : Enemy_Base
{
    EnemyStatData enemyStatData = DB.GetEnemyData("나무정령");
    [HideInInspector]
    public float currentHp;

    public override void Init()
    {
        currentHp = enemyStatData.hp;
    }
    public override void EnemyAttack()
    {
        int weight = UnityEngine.Random.Range(0, 99); // 가중치 아직 안건드림
        if (weight < 33)
            weight = 0;
        else if(weight < 66)
            weight = 1;
        else 
            weight = 2;

        switch (weight)
        {
            case 0: // 기본공격
                AttackType myAttackType = AttackType.Penetrate;
                SingleAttack(enemyStatData.atk, myAttackType);
                break;
            case 1: // 가르기
                Split();
                break;
            case 2: // 뿌리
                Root();
                break;
        }
    }
    public override void EnemyDamaged()
    {

    }

    public void Split()
    {
        AttackType myAttackType = AttackType.Slash;
        WideAttack(enemyStatData.atk, myAttackType);
    }
    public void Root()
    {
        AttackType myAttackType = AttackType.Penetrate;
        DoubleAttack(enemyStatData.atk, myAttackType);
        // 낮은 확률로 기절
    }
}
