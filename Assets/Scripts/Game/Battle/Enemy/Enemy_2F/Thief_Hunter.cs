using Scripts.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thief_Hunter : Enemy_Base
{
    EnemyStatData enemyStatData = DB.GetEnemyData("도적궁수");
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
                SingleAttack(enemyStatData.atk, AttackType.Penetrate, AttackProperty.Physics);
                break;
            case 1: // 실명 화살
                Blind_Arrow();
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

    public void Blind_Arrow()
    {
        SingleAttack(enemyStatData.atk, AttackType.Penetrate, AttackProperty.Physics);
        // 낮은 확률로 2턴간 실명
    }
}