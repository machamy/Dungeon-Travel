using Scripts.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts;

public class Thief_Boss : Enemy_Base
{
    EnemyStatData enemyStatData = DB.GetEnemyData(2,"도적대장");
    [HideInInspector]
    public float currentHp;
    private bool isReady;
    public override void Init()
    {
        Bandits(); // 도적단(패시브)
        currentHp = enemyStatData.hp;
        isReady = false;
    }
    public override float GetAgi()
    {
        return enemyStatData.agi;
    }
    public override void EnemyAttack()
    {
        if (((currentHp) / (enemyStatData.hp) < 0.5f) && isReady == false) // 아직 2페이즈 조건 모름
        {
            Bandits(); // 도적단(패시브)
            isReady = true;
        }
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
                SingleAttack(enemyStatData.atk, AttackType.Slash, AttackProperty.Physics);
                break;
            case 1: // 쌍수베기
                Double_Slash();
                break;
        }
    }
    public override void EnemyDamaged(float atk, AttackType attackType, AttackProperty attackProperty)
    {
        if (enemyStatData.weakType == attackType)
        {
            atk *= 2f; // 임시
            //크리확률 증가
        }
        if (enemyStatData.resistType == attackType)
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

    public void Bandits() // 임시함수
    {
        if(BattleManager.Instance.enemyPrefab[1] == null)
        {
            BattleManager.Instance.EnemySpawn(Define_Battle.Enemy_Type.Thief);
        }
        if (BattleManager.Instance.enemyPrefab[2] == null)
        {
            BattleManager.Instance.EnemySpawn(Define_Battle.Enemy_Type.Thief);
        }
    }
    public void Double_Slash()
    {
        Unit unit;
        int AttackRange = UnityEngine.Random.Range(0, 5);
        GameObject go = GameObject.Find("Player (" + AttackRange + ")(Clone)");
        unit = go.GetComponent<Unit>();

        unit.TakeDamage(enemyStatData.atk * 0.75f, AttackType.Slash, AttackProperty.Physics);
        unit.TakeDamage(enemyStatData.atk * 0.75f, AttackType.Slash, AttackProperty.Physics);
    }
}
