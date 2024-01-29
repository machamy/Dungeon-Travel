using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thief : Enemy_Base
{
    EnemyStatData enemyStatData = DB.GetEnemyData("도적");
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
            case 0: // 기본공격 //아직 안정해진듯
               
                break;
            case 1: // 암습
                Sneak_Attack();
                break;
        }
    }
    public override void EnemyDamaged()
    {

    }

    public void Sneak_Attack()
    {
        AttackType myAttackType = AttackType.Slash;
        SingleAttack(enemyStatData.atk, myAttackType);
        // 낮은 확률로 독
    }
}
