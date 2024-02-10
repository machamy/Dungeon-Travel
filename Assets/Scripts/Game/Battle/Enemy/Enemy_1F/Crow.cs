using Scripts.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crow : Enemy_Base
{
    EnemyStatData enemyStatData = DB.GetEnemyData(1,"까마귀");
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
        int weight = UnityEngine.Random.Range(0, 99); // 가중치 아직 안건드림
        BuffManager buffManager = gameObject.GetComponent<BuffManager>();
        if (buffManager.isStun == true)
            return;
        if (buffManager.isSilence == true)
            weight = 0;
        else
        {
            if (weight < 50)
                weight = 0;
            else
                weight = 1;
        }

        switch (weight)
        {
            case 0: // 기본공격
                SingleAttack(enemyStatData.atk, AttackType.Slash, AttackProperty.Physics);
                break;
            case 1: // 쪼아대기
                Pecking();
                break;
        }
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

    public void Pecking()
    {
        SingleAttack(enemyStatData.atk, AttackType.Penetrate, AttackProperty.Physics);
    }
}
