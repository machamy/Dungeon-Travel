using Scripts.Data;
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
    public override float GetAgi()
    {
        return enemyStatData.agi;
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
                SingleAttack(enemyStatData.atk, AttackType.Penetrate, AttackProperty.Physics);
                break;
            case 1: // 가르기
                Split();
                break;
            case 2: // 뿌리
                Root();
                break;
        }
    }
    public override void EnemyDamaged(float atk, AttackType attackType, AttackProperty attackProperty)
    {
        currentHp -= atk;
        if (currentHp <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void Split()
    {
        WideAttack(enemyStatData.atk, AttackType.Slash, AttackProperty.Physics);
    }
    public void Root()
    {
        DoubleAttack(enemyStatData.atk, AttackType.Penetrate, AttackProperty.Physics);
        // 낮은 확률로 기절
    }
}
