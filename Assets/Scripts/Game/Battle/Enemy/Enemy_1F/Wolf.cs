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
    public override float GetAgi()
    {
        return enemyStatData.agi;
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
                SingleAttack(enemyStatData.atk, AttackType.Slash, AttackProperty.Physics);
                break;
            case 1: // 물어뜯기
                Bite();
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

    public void Bite()
    {
        SingleAttack(enemyStatData.atk, AttackType.Penetrate, AttackProperty.Physics);
        // 낮은 확률로 출혈
    }
}
