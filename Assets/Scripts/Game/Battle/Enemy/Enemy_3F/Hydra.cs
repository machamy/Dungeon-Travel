using Scripts.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts;

public class Hydra : Enemy_Base
{
    EnemyStatData enemyStatData = DB.GetEnemyData(3, "쌍둥이 심해 인어");
    [HideInInspector]
    public float currentHp;
    private bool isReady;
    float HpPer;
    public override void Init()
    {
        currentHp = enemyStatData.hp;
        isReady = false;
        HpPer = 0.6f;
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
        if (isReady == false && (currentHp / enemyStatData.hp) <= HpPer)
        {
            Regeneration(); // 재생
            HpPer -= 0.1f;
            if(HpPer <= 0.4f)
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
                    Baptism_Flame(); // 화염 세례
                    break;
                case 2:
                    Electric_Whip(); // 전기 채찍
                    break;
            }
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
    private void NormalAttack()
    {
        int[] AttackRangeArray = new int[] { 0, 1, 2, 3, 4 };
        int AttackRange1 = UnityEngine.Random.Range(0, 5);
        for (int i = AttackRange1; i < AttackRangeArray.Length - 1; i++)
        {
            AttackRangeArray[i] = AttackRangeArray[i + 1];
        }
        int AttackRange2 = AttackRangeArray[UnityEngine.Random.Range(0, 4)];
        for(int i = AttackRange2; i<AttackRangeArray.Length - 2; i++)
        {
            AttackRangeArray[i] = AttackRangeArray[i + 1];
        }
        int AttackRange3 = AttackRangeArray[UnityEngine.Random.Range(0, 3)];


        GameObject go1 = GameObject.Find("Player (" + AttackRange1 + ")(Clone)");
        Unit unit1 = go1.GetComponent<Unit>();
        unit1.TakeDamage(enemyStatData.atk, AttackType.Smash, AttackProperty.Physics);

        GameObject go2 = GameObject.Find("Player (" + AttackRange2 + ")(Clone)");
        Unit unit2 = go2.GetComponent<Unit>();
        unit2.TakeDamage(enemyStatData.atk, AttackType.Smash, AttackProperty.Physics);

        GameObject go3 = GameObject.Find("Player (" + AttackRange3 + ")(Clone)");
        Unit unit3 = go3.GetComponent<Unit>();
        unit3.TakeDamage(enemyStatData.atk, AttackType.Smash, AttackProperty.Physics);
    }
    private void Baptism_Flame()
    {
        WideAttack(enemyStatData.atk, AttackType.Flame, AttackProperty.Magic);
    }
    private void Electric_Whip()
    {
        int AttackRange1 = UnityEngine.Random.Range(0, 5);
        GameObject go1 = GameObject.Find("Player (" + AttackRange1 + ")(Clone)");
        Unit unit1 = go1.GetComponent<Unit>();
        unit1.TakeDamage(enemyStatData.atk, AttackType.Smash, AttackProperty.Physics);
        BuffManager pBuffManager = go1.GetComponent<BuffManager>();
        pBuffManager.Confuse(3, 0, 100);
    }
    private void Regeneration()
    {
        //상태이상 스킬이 적중하면 return;
    }
}
