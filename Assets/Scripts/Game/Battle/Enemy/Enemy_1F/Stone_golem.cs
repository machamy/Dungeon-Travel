using Scripts.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts;

public class Stone_golem : Enemy_Base
{
    EnemyStatData enemyStatData = DB.GetEnemyData(1,"바위골렘");
    [HideInInspector]
    public float currentHp;
    private bool isReady; // 코어 활성화

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
        if((currentHp)/(enemyStatData.hp) > 0.5f) // hp가 50퍼 초과일때
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
    public void Stomp()
    {
        WideAttack(enemyStatData.atk, AttackType.Slash, AttackProperty.Physics);
        // 낮은 확률로 혼란
    }
    public void Core_Active()
    {
        isReady = true;
        // 화염 속성 공격력 상승
    }
    public void Core_Laser()
    {
        DoubleAttack(enemyStatData.atk, AttackType.Flame, AttackProperty.Magic);
    }
}
