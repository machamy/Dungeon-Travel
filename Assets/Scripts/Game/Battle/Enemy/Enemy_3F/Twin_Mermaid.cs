using Scripts.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts;
using System;

public class Twin_Mermaid : Enemy_Base
{
    EnemyStatData enemyStatData = DB.GetEnemyData(3, "쌍둥이 심해 인어");
    [HideInInspector]
    public float currentHp;
    private bool isReady;
    public override void Init()
    {
        currentHp = enemyStatData.hp;
        isReady = false;
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
        if (isReady == false && (currentHp / enemyStatData.hp) <= 0.5f)
        {
            Ghost(); // 유체화
            isReady = true;
        }
        else
        {
            switch (weight)
            {
                case 0:
                    NormalAttack();
                    break;
                case 1:
                    Mermaid_Song(); // 인어의 노래
                    break;
                case 2:
                    Electric_Emission(); // 전류방출
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
    private void NormalAttack()
    {
        int[] AttackRangeArray = new int[] { 0, 1, 2, 3, 4 };
        int AttackRange1 = UnityEngine.Random.Range(0, 5);
        for (int i = AttackRange1; i < AttackRangeArray.Length - 1; i++)
        {
            AttackRangeArray[i] = AttackRangeArray[i + 1];
        }
        int AttackRange2 = AttackRangeArray[UnityEngine.Random.Range(0, 4)];


        GameObject go1 = GameObject.Find("Player (" + AttackRange1 + ")(Clone)");
        Unit unit1 = go1.GetComponent<Unit>();
        unit1.TakeDamage(enemyStatData.atk, AttackType.Penetrate, AttackProperty.Physics);

        GameObject go2 = GameObject.Find("Player (" + AttackRange2 + ")(Clone)");
        Unit unit2 = go2.GetComponent<Unit>();
        unit2.TakeDamage(enemyStatData.atk, AttackType.Penetrate, AttackProperty.Physics);
    }
    private void Mermaid_Song()
    {
        for (int i = 0; i < 5; i++)
        {
            GameObject go = GameObject.Find("Player (" + i + ")(Clone)");
            Unit unit = go.GetComponent<Unit>();
            unit.TakeDamage(enemyStatData.atk, AttackType.Smash, AttackProperty.Physics);
            BuffManager pBuffManager = go.GetComponent<BuffManager>();
            pBuffManager.Silence(3, 0, 50); // 중간확률
        }
    }
    private void Electric_Emission()
    {
        DoubleAttack(enemyStatData.atk, AttackType.Lightning, AttackProperty.Magic);
    }
    private void Ghost()
    {
        // 회피율이 증가한다.
    }
}
