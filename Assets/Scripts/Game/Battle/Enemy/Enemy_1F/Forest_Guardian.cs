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
    public override void EnemyAttack()
    {
        if (((currentHp) / (enemyStatData.hp) > 0.5f)&& isReady == false) // 아직 2페이즈 조건 모름
        {
            Berserk(); // 광화
        }
        else // 광화를 제외한 상황
        {
            int weight = UnityEngine.Random.Range(0, 99); // 가중치 아직 안건드림
            if (weight < 33)
                weight = 0;
            else if (weight < 66)
                weight = 1;
            else
                weight = 2;

            switch (weight)
            {
                case 0: // 기본공격
                    AttackType myAttackType = AttackType.Penetrate;
                    SingleAttack(enemyStatData.atk, myAttackType);
                    break;
                case 1: // 발구르기
                    Forward_Slash();
                    break;
                case 2: // 종베기
                    Cutter();
                    break;
            }
            
        }

    }
    public override void EnemyDamaged()
    {

    }
    public void Forward_Slash()
    {
        AttackType myAttackType = AttackType.Slash;
        ForwardAttack(enemyStatData.atk, myAttackType); // 전열공격
        // 낮은 확률로 혼란
    }
    public void Cutter()
    {
        AttackType myattackType = AttackType.Slash;
        DoubleAttack(enemyStatData.atk, myattackType);
    }
    public void Berserk()
    {
        isReady = true;
        //공격력,민첩상승  방어력 하락
    }
    
}
