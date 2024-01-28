using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy_Base : MonoBehaviour
{
    private Unit unit;
    EnemyStatData enemyStatData = new EnemyStatData();
    public enum AttackProperty
    {
        Physics, // 물리
        Magic, // 마법
    }
    [Flags]
    public enum AttackType
    {
        None = 0,
        Damage = 1 << 0, // 타격
        Penetrate= 1 << 1, // 관통
        Slash= 1 << 2, // 참격
        Wide= 1 << 3, // 광역
        Flame= 1 << 4, // 화염
        Freezing= 1 << 5, // 빙결
        Wind= 1 << 6, // 바람
        Lightning = 1 << 7, // 전격
        Light= 1 << 8, // 빛
        Dark= 1 << 9, // 어둠
        
        All = int.MaxValue
    }
    
    [Flags]
    public enum EnemyProperty
    {
        None = 0,
        Hostile = 1 << 0, // 선공 여부
        Move= 1 << 1, // 이동 or 고정
        Rush= 1 << 2, // 난입
        
        All = int.MaxValue
    }
    
    public virtual void EnemyAttack(AttackProperty property, AttackType type) // 오버라이딩
    {
        switch (type) // 공격타입에 따른 분리 //오버라이딩으로 없앨 가능성 높음
        {
            case AttackType.Damage:
                Slash(enemyStatData.atk);
                break;
            case AttackType.Penetrate:
                Penetrate();
                break;
            case AttackType.Slash:
                Smash();
                break;
            case AttackType.Wide:
                Wide();
                break;
            case AttackType.Flame:
                Flame();
                break;
            case AttackType.Freezing:
                Freezing();
                break;
            case AttackType.Wind:
                Wind();
                break;
            case AttackType.Lightning:
                Lightning();
                break;
            case AttackType.Light:
                Light();
                break;
            case AttackType.Dark:
                Dark();
                break;
        }
        //다른 부과효과 삽입
    }
    public virtual void EnemyDamaged()
    {
        // 적이 공격받았을때 처리하는 함수
        // 오버라이딩으로 각각 스크립트에서 약점,저항 처리
    }
    public void Slash(float atk)
    {
        int AttackRange = UnityEngine.Random.Range(0, BattleManager.Instance.playerPrefab.Length);
        unit = BattleManager.Instance.playerPrefab[AttackRange].GetComponent<Unit>();
        unit.TakeDamage(atk);
    }
    public void Penetrate()
    {

    }
    public void Smash()
    {

    }
    public void Wide()
    {

    }
    public void Flame()
    {

    }

    public void Freezing()
    {

    }
    public void Wind()
    {

    }
    public void Lightning()
    {

    }
    public void Light()
    {

    }
    public void Dark()
    {

    }
    public void Skill1()
    {

    }
}
