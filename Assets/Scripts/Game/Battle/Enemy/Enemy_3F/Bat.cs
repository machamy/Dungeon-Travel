using Scripts.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : Enemy_Base
{
    EnemyStatData enemyStatData = DB.GetEnemyData(3, "박쥐");
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
        BuffManager buffManager = gameObject.GetComponent<BuffManager>();
        if (buffManager.isStun == true)
            return;
        //기본공격
        SingleAttack(enemyStatData.atk, AttackType.Penetrate, AttackProperty.Physics);
    }

    public override void EnemyDamaged(float atk, AttackType attackType, AttackProperty attackProperty)
    {
        if (enemyStatData.WeakType == attackType)
        {
            atk *= 2f; // 임시
            //크리확률 증가
        }
        if (enemyStatData.ResistType == attackType)
        {
            atk /= 2f; // 임시
        }
        currentHp -= atk;
        if (currentHp <= 0)
        {
            Destroy(gameObject);
            return;
        }
    }
}