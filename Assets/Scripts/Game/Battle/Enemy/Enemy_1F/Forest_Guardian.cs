using Scripts.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Forest_Guardian : Enemy_Base
{
    EnemyStatData enemyStatData = DB.GetEnemyData("숲의 수호자");
    [HideInInspector]
    public float currentHp;
    private bool isReady; // 2페이즈

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
        if (((currentHp) / (enemyStatData.hp) < 0.5f)&& isReady == false) // 아직 2페이즈 조건 모름
        {
            Berserk(); // 광화
            isReady = true;
        }
        else // 광화를 제외한 상황
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
                    SingleAttack(enemyStatData.atk, AttackType.Penetrate, AttackProperty.Physics);
                    break;
                case 1: // 전방베기
                    Forward_Slash();
                    break;
                case 2: // 종베기
                    Cutter();
                    break;
            }
            
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
        ForwardAttack(enemyStatData.atk, AttackType.Slash, AttackProperty.Physics); // 전열공격
        // 낮은 확률로 혼란
    }
    public void Cutter()
    {
        DoubleAttack(enemyStatData.atk, AttackType.Slash, AttackProperty.Physics);
    }
    public void Berserk()
    {
        isReady = true;
        //공격력,민첩상승  방어력 하락
    }
    
}
