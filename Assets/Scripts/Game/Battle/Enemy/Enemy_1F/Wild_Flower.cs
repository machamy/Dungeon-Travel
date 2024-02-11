using Scripts.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts;

public class Wild_Flower : Enemy_Base
{
    EnemyStatData enemyStatData = DB.GetEnemyData(1,"야생꽃");
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
        int weight = Utility.WeightedRandom(50, 50); // 가중치 아직 안건드림
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
                SingleAttack(enemyStatData.atk, AttackType.Penetrate, AttackProperty.Physics);
                break;
            case 1: // 가루뿌리기
                Sprinkle_powder();
                break;
        }
        
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

    public void Sprinkle_powder()
    {
        //데미지x 낮은 확률로 대상 실명 // 단일?
    }
}
