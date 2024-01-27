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
    public enum AttackType
    {
        Damage, // 타격
        Penetrate, // 관통
        Slash, // 참격
        Wide, // 광역
        Flame, // 화염
        Freezing, // 빙결
        Wind, // 바람
        Lightning, // 전격
        Light, // 빛
        Dark, // 어둠
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
