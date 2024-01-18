using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Base : MonoBehaviour
{
    public int EnemyLevel; // 적 레벨
    public int EnemyDamage; // 적 공격력
    public int EnemyMaxHealth; // 적 최대체력
    public int EnemyCurrentHealth; // 적 현재체력
    public int EnemyAgility; // 적 민첩            
    public int EnemyMana; // 적 마나                    // 이 여섯가지 수치 유니티 인스펙터창에서 조정

    public enum AttackType
    {
        Damage, // 타격
        Penetrate, // 관통
        Slash, // 참격
    }
    public virtual void EnemyAttack(AttackType type) // 오버라이딩
    {
        switch (type) // 공격타입에 따른 분리 //오버라이딩으로 없앨 가능성 높음
        {
            case AttackType.Damage:
                Damaged();
                break;
            case AttackType.Penetrate:
                Penetrate();
                break;
            case AttackType.Slash:
                Slash();
                break;
        }
        //다른 부과효과 삽입
    }
    public void Damaged()
    {

    }
    public void Penetrate()
    {

    }
    public void Slash()
    {

    }
    public void Skill1()
    {

    }
}
