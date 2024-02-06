using Scripts.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thief_Vanguard : Enemy_Base
{
    EnemyStatData enemyStatData = DB.GetEnemyData("도적선봉대");
    [HideInInspector]
    public float currentHp;

    public override void Init()
    {
        currentHp = enemyStatData.hp;
    }
    public override float GetAgi()
    {
        return enemyStatData.agi;
    }
    public override void EnemyAttack()
    {
        int weight = UnityEngine.Random.Range(0, 99); // 가중치 아직 안건드림
        if (weight < 50)
            weight = 0;
        else
            weight = 1;

        switch (weight)
        {
            case 0: // 기본공격
                SingleAttack(enemyStatData.atk, AttackType.Slash, AttackProperty.Physics);
                break;
            case 1: // 베고 찌르기
                Cut_Stab();
                break;
        }
    }
    public override void EnemyDamaged(float atk, AttackType attackType, AttackProperty attackProperty)
    {
        currentHp -= atk;
        if (currentHp <= 0)
        {
            Destroy(gameObject);
            return;
        }
    }

    public void Cut_Stab()
    {
        Unit unit;
        int AttackRange = UnityEngine.Random.Range(0, BattleManager.Instance.playerPrefab.Length);
        unit = BattleManager.Instance.playerPrefab[AttackRange].GetComponent<Unit>();

        unit.TakeDamage(enemyStatData.atk, AttackType.Slash, AttackProperty.Physics);
        unit.TakeDamage(enemyStatData.atk, AttackType.Slash, AttackProperty.Physics);
    }
}
