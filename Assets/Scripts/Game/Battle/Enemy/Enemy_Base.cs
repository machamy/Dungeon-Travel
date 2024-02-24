using Scripts;
using Scripts.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy_Base : MonoBehaviour
{
    private Unit unit;
    public bool isDead = false;
    public enum AttackProperty
    {
        Physics, // 물리
        Magic, // 마법
    }

    public virtual void Init()
    {

    }

    public virtual float GetAgi()
    {
        return 0;
    }
    
    public virtual void EnemyAttack() // 오버라이딩
    {
        // 여기서 가중치를 부여해서 공격 타입 결정
        int weight = UnityEngine.Random.Range(0, 3);
        switch(weight)
        {
            case 0:
                SingleAttack(0,0,0);
                break;
            case 1:
                Skill1();
                break;
            case 2:
                Skill2();
                break;
        }
        //다른 부과효과 삽입
    }
    public virtual void EnemyDamaged(float atk, Scripts.Data.AttackType attackType, AttackProperty attackProperty)
    {
        // 적이 공격받았을때 처리하는 함수
        // 오버라이딩으로 각각 스크립트에서 약점,저항 처리
    }
    /// <summary>
    /// 단일공격
    /// </summary>
    /// <param name="atk"></param>
    /// <param name="attackPropertyparam>
    public void SingleAttack(float atk, Scripts.Data.AttackType attackType, AttackProperty attackProperty)
    {
        int AttackRange = UnityEngine.Random.Range(0, 5);
        GameObject go = GameObject.Find("Player (" + AttackRange + ")(Clone)");
        unit = go.GetComponent<Unit>();
        unit.TakeDamage(atk, attackType, attackProperty);
    }
    /// <summary>
    /// 광역공격
    /// </summary>
    /// <param name="atk"></param>
    /// <param name="attackPropertyparam>
    public void WideAttack(float atk, Scripts.Data.AttackType attackType, AttackProperty attackProperty)
    {
        for(int i = 0; i < 5; i++)
        {
            GameObject go = GameObject.Find("Player (" + i + ")(Clone)");
            unit = go.GetComponent<Unit>();
            unit.TakeDamage(atk, attackType, attackProperty);
        }
    }
    /// <summary>
    /// 전후공격
    /// </summary>
    /// <param name="atk"></param>
    /// <param name="attackPropertyparam>
    public void DoubleAttack(float atk, Scripts.Data.AttackType attackType, AttackProperty attackProperty)
    {


    }
    /// <summary>
    /// 전열공격
    /// </summary>
    /// <param name="atk"></param>
    /// <param name="attackPropertyparam>
    public void ForwardAttack(float atk, Scripts.Data.AttackType attackType, AttackProperty attackProperty)
    {

    }
    /// <summary>
    /// 후열공격
    /// </summary>
    /// <param name="atk"></param>
    /// <param name="attackPropertyparam>
    public void BackwardAttack(float atk, Scripts.Data.AttackType attackType, AttackProperty attackProperty)
    {

    }
    public void Skill1()
    {

    }
    public void Skill2()
    {

    }
}
