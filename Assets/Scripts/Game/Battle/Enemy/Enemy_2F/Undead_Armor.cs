using Scripts.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Undead_Armor : Enemy_Base
{
    EnemyStatData enemyStatData = DB.GetEnemyData("죽지못한갑주");
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
            if (weight < 33)
                weight = 0;
            else if (weight < 66)
                weight = 1;
            else
                weight = 2;
        }
        switch (weight)
        {
            case 0: // 기본공격
                SingleAttack(enemyStatData.atk, AttackType.Slash, AttackProperty.Physics);
                break;
            case 1: // 전방베기
                Forward_Slash();
                break;
            case 2: // 종베기
                Cutter();
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

    public void Forward_Slash()
    {
        ForwardAttack(enemyStatData.atk, AttackType.Slash, AttackProperty.Physics);
    }
    public void Cutter()
    {
        DoubleAttack(enemyStatData.atk, AttackType.Slash, AttackProperty.Physics);
    }
}
