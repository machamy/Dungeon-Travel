using Scripts.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fallen_Queen : Enemy_Base
{
    EnemyStatData enemyStatData = DB.GetEnemyData("몰락한여왕");
    [HideInInspector]
    public float currentHp;
    private bool isReady;
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
        if (((currentHp) / (enemyStatData.hp) < 0.3f) && isReady == false) // 아직 2페이즈 조건 모름
        {
            Recall(); // 회상(패시브)
            isReady = true;
        }
        int weight = UnityEngine.Random.Range(0, 99); // 가중치 아직 안건드림
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
                WideAttack(enemyStatData.atk, AttackType.Slash, AttackProperty.Physics);
                break;
            case 1: // 쌍수베기
                Scream();
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

    public void Recall() // 임시함수
    {
        currentHp += enemyStatData.hp * 0.5f;
    }
    public void Scream()
    {
        WideAttack(enemyStatData.atk, AttackType.Dark, AttackProperty.Magic);
    }
}
