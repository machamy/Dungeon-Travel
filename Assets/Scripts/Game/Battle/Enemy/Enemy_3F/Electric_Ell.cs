using Scripts.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts;

public class Electric_Ell : Enemy_Base
{
    EnemyStatData enemyStatData = DB.GetEnemyData(3, "전기장어 정령");
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
        int weight = Utility.WeightedRandom(50, 50); // 가중치는 아직
        if (buffManager.isStun == true)
            return;
        if (buffManager.isSilence == true)
            weight = 0;
        switch (weight)
        {
            case 0:
                SingleAttack(enemyStatData.atk, AttackType.Smash, AttackProperty.Physics); //기본공격
                break;
            case 1:
                Electric_Whip(); // 전기 채찍
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
    private void Electric_Whip()
    {
        SingleAttack(enemyStatData.atk, AttackType.Lightning, AttackProperty.Magic);
        //중간확률 혼란
    }
}
