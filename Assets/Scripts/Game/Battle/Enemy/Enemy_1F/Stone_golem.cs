using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone_golem : Enemy_Base
{
    EnemyStatData enemyStatData = DB.GetEnemyData("바위골렘");
    [HideInInspector]
    public float currentHp;
    private bool isReady; // 코어 활성화

    public override void Init()
    {
        currentHp = enemyStatData.hp;
        isReady = false;
    }
    public override void EnemyAttack()
    {
        if((currentHp)/(enemyStatData.hp) > 0.5f) // hp가 50퍼 초과일때
        {
            int weight = UnityEngine.Random.Range(0, 99); // 가중치 아직 안건드림
            if (weight < 50)
                weight = 0;
            else
                weight = 1;

            switch (weight)
            {
                case 0: // 기본공격
                    AttackType myAttackType = AttackType.Penetrate;
                    SingleAttack(enemyStatData.atk, myAttackType);
                    break;
                case 1: // 발구르기
                    Stomp();
                    break;
            }
        }
        else // hp가 50퍼 이하일때
        {
            if(isReady == false)
            {
                Core_Active(); // 코어 활성화
            }
            else
            {
                Core_Laser();
            }
        }
        
    }
    public override void EnemyDamaged()
    {

    }
    public void Stomp()
    {
        AttackType myAttackType = AttackType.Slash;
        WideAttack(enemyStatData.atk, myAttackType);
        // 낮은 확률로 혼란
    }
    public void Core_Active()
    {
        isReady = true;
        // 화염 속성 공격력 상승
    }
    public void Core_Laser()
    {
        AttackType myattackType = AttackType.Flame;
        DoubleAttack(enemyStatData.atk, myattackType);
    }
}
