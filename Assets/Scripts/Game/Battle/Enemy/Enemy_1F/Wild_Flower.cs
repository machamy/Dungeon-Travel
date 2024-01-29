using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wild_Flower : Enemy_Base
{
    EnemyStatData enemyStatData = DB.GetEnemyData("야생꽃");
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

        switch(weight)
        {
            case 0: // 기본공격
                AttackType myAttackType = AttackType.Penetrate;
                SingleAttack(enemyStatData.atk, myAttackType);
                break;
            case 1: // 가루뿌리기
                Sprinkle_powder();
                break;
        }
        
    }
    public override void EnemyDamaged()
    {

    }

    public void Sprinkle_powder()
    {
        //데미지x 낮은 확률로 대상 실명 // 단일?
    }
}
