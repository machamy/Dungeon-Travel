using Scripts.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts;
using static Enemy_Base;

public class Currupted_Water_Sprit : Enemy_Base
{
    EnemyStatData enemyStatData = DB.GetEnemyData(3, "오염된 물의 정령");
    [HideInInspector]
    public float currentHp;
    private bool isReady;
    int probability;
    public override void Init()
    {
        currentHp = enemyStatData.hp;
        isReady = false;
        probability = 10;
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
        if (isReady == false && (currentHp/enemyStatData.hp)<= 0.3f)
        {
            Water_Rage(); // 물의 분노
            isReady = true;
        }
        else
        {
            switch (weight)
            {
                case 0:
                    SingleAttack(enemyStatData.atk, AttackType.Smash, AttackProperty.Physics); //기본공격
                    break;
                case 1:
                    Huge_Wave(); // 거대한 파도
                    break;
                case 2:
                    Pollution(); // 오염
                    break;
            }
        }
    }

    public override void EnemyDamaged(float atk, AttackType attackType, AttackProperty attackProperty)
    {
        if (enemyStatData.weakType == attackType)
        {
            atk *= 2f; // 임시
            //크리확률 증가
        }
        if (enemyStatData.resistType == attackType)
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
    private void Huge_Wave()
    {
        for (int i = 0; i < 5; i++)
        {
            GameObject go = GameObject.Find("Player (" + i + ")(Clone)");
            Unit unit = go.GetComponent<Unit>();
            unit.TakeDamage(enemyStatData.atk, AttackType.Smash, AttackProperty.Physics);
            BuffManager pBuffManager = go.GetComponent<BuffManager>();
            pBuffManager.Stun(3, 0, probability);
        }
    }
    private void Pollution()
    {
        DoubleAttack(enemyStatData.atk, AttackType.Dark, AttackProperty.Magic);
        // 낮은 확률로 실명
    }
    private void Water_Rage()
    {
        probability = 50;
    }
}
