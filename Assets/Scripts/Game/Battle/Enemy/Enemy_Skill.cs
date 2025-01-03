using GeneralMonsterStates;
using System.Collections;
using Scripts;
using System;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Data;
using Scripts.Entity;
using UnityEditor.PackageManager;

public class Enemy_Skill
{
    BattleEnemyUnit enemy;
    BattleManager battleManager;
    int passiveDuration;
    public Enemy_Skill(BattleEnemyUnit enemy)
    {
        this.enemy = enemy;
        battleManager = GameObject.Find("BattleSystem").GetComponent<BattleManager>();
    }

    public List<Action<SkillData>> GetSkillList(int floor, string enemyName)
    {
        List<Action<SkillData>> skillLists = new List<Action<SkillData>>();
        switch (floor)
        {
            case 1:
                switch (enemyName)
                {
                    case "토끼":
                        skillLists.Add(EnemyAttack); // 기본공격
                        break;
                    case "슬라임":
                        skillLists.Add(EnemyAttack); // 기본공격
                        break;
                    case "야생꽃":
                        skillLists.Add(EnemyAttack); // 기본공격
                        skillLists.Add(EnemyAttack); // 가루뿌리기
                        break;
                    case "늑대":
                        skillLists.Add(EnemyAttack); // 기본공격
                        skillLists.Add(EnemyAttack); // 물어뜯기
                        break;
                    case "도적":
                        skillLists.Add(EnemyAttack); // 기본공격
                        skillLists.Add(EnemyAttack); // 암습
                        break;
                    case "까마귀":
                        skillLists.Add(EnemyAttack); // 일반공격
                        skillLists.Add(EnemyAttack); // 쪼아대기
                        break;
                    case "나무정령":
                        skillLists.Add(EnemyAttack); // 일반공격
                        skillLists.Add(EnemyAttack); // 가르기
                        skillLists.Add(EnemyAttack); // 뿌리
                        break;
                    case "바위골렘":
                        skillLists.Add(Core_Active); // 코어 활성화(패시브)
                        skillLists.Add(EnemyAttack); // 일반공격
                        skillLists.Add(EnemyAttack); // 발구르기
                        skillLists.Add(EnemyAttack); // 코어 레이저
                        break;
                    case "숲의수호자":
                        skillLists.Add(Berserk); // 광화(패시브)
                        skillLists.Add(EnemyAttack); // 일반공격
                        skillLists.Add(EnemyAttack); // 전방베기
                        skillLists.Add(EnemyAttack); // 종베기
                        break;
                }
                break;
            case 2:
                switch (enemyName)
                {
                    case "쥐":
                        skillLists.Add(EnemyAttack);
                        break;
                    case "슬라임":
                        skillLists.Add(EnemyAttack);
                        break;
                    case "도적":
                        skillLists.Add(EnemyAttack);
                        skillLists.Add(EnemyAttack);
                        break;
                    case "도적선봉대":
                        skillLists.Add(EnemyAttack);
                        skillLists.Add(Cut_Stab);
                        break;
                    case "도적궁수":
                        skillLists.Add(EnemyAttack);
                        skillLists.Add(EnemyAttack);
                        break;
                    case "도적대장":
                        skillLists.Add(Bandits);
                        skillLists.Add(EnemyAttack);
                        skillLists.Add(EnemyAttack);
                        break;
                    case "죽지못한갑옷":
                        skillLists.Add(EnemyAttack);
                        skillLists.Add(EnemyAttack);
                        skillLists.Add(EnemyAttack);
                        break;
                    case "몰락한여왕":
                        skillLists.Add(Reminiscence);
                        skillLists.Add(Authority);
                        skillLists.Add(EnemyAttack);
                        skillLists.Add(EnemyAttack);
                        break;
                }
                break;
            //case 3:
            //    switch (enemyName)
            //    {
                    
            //    }
            //    break;
            //case 4:
            //    switch (enemyName)
            //    {

            //    }
            //    break;
            //case 5:
            //    switch (enemyName)
            //    {

            //    }
            //    break;
        }

        return skillLists;
    }

    public void EnemyAttack(SkillData skillData) // 특별한 로직이 아닌 일반적인 공격
    {
        List<BattleUnit> Opponent = battleManager.GetPlayerUnits(skillData.enemyTargetType);
        //Debug.Log($"Opponent : {Opponent.Count}");
        for (int i = 0; i < Opponent.Count; i++)
        {
            if (Opponent[i] == null)
            {
                Debug.Log(Opponent[i].name);
                continue;
            }
            BuffManager buffManager = Opponent[i].GetComponent<BuffManager>();
            Unit unit = Opponent[i].GetComponent<Unit>(); // 데미지 계산식 나오면 수정
            unit.TakeDamage(skillData.physicsDamage);
            if (skillData.debuffType != DebuffType.None)
                buffManager.DebuffAdd(skillData.debuffType, skillData.buffRatio, 2, 0); // 데미지는 아직 정해지지 않음
        }
    }

    public void Core_Active(SkillData skillData)
    {
        enemy.index = 3; // 다음 공격을 코어 레이저로 고정
        enemy.statData.atk++; // 수치는 모름
    }

    public void Berserk(SkillData skillData)
    {
        enemy.statData.atk++;
        enemy.statData.agi++;
        enemy.statData.def--;
        enemy.statData.mdef--; // 수치는 모름
    }

    public void Cut_Stab(SkillData skillData)
    {
        AttackType[] attackType = Enum.GetValues(typeof(AttackType)) as AttackType[];
        List<BattleUnit> Opponent = battleManager.GetPlayerUnits(skillData.enemyTargetType);
        BuffManager buffManager = Opponent[0].GetComponent<BuffManager>();
        Unit unit = Opponent[0].GetComponent<Unit>();
        foreach(AttackType type in attackType) // 여기서 공격 2번하게 데미지 계산
        {
            unit.TakeDamage(skillData.physicsDamage); // 데미지 계산식 나오면 수정
            Debug.Log("공격완료");
            if (skillData.debuffType != DebuffType.None)
                buffManager.DebuffAdd(skillData.debuffType, skillData.buffRatio, 2, 0); // 데미지는 아직 정해지지 않음
        }
    }

    public void Bandits(SkillData skillData)
    {
        if(BattleManager.aliveEnemy < 3)
        {
            for(int i = BattleManager.aliveEnemy; i < 3; i++)
            {
                battleManager.createUnit.EnemyUnitSpawn(2, "도적선봉대");
            }
        }
    }

    public void Authority(SkillData skillData)
    {
        if(skillData.rank != 0)
        {
            List<BattleUnit> enemyGO = battleManager.GetPlayerUnits(skillData.enemyTargetType);
            for (int i = 0; i < enemyGO.Count; i++)
            {
                BuffManager buffManager = enemyGO[i].GetComponent<BuffManager>();
                if (skillData.attackType == AttackType.Slash)
                    buffManager.DebuffAdd(DebuffType.Silence, 30f, 3, 0);
            }
        }
        else
        {
            List<BattleUnit> playerGO = battleManager.GetPlayerUnits(skillData.enemyTargetType);
            for (int i = 0; i < playerGO.Count; i++)
            {
                BuffManager buffManager = playerGO[i].GetComponent<BuffManager>();
                if (skillData.attackType == AttackType.Slash)
                    buffManager.DebuffAdd(DebuffType.Silence, 30f, 3, 0);
            }
        }
    }
    

    public void Reminiscence(SkillData skillData) // HUD를 어떻게 업데이트 할것인지 생각해야함
    {
        enemy.currentHP = enemy.statData.hp / 2;
    }

    public void RageOfWater(SkillData skillData)
    {
        enemy.skillDatas[2].debuffRatio += 20f;
        enemy.skillDatas[3].debuffRatio += 20f;
    }

    public void Ghost(SkillData skillData)
    {
        List<BattleUnit> playerGO = battleManager.GetPlayerUnits(skillData.enemyTargetType);
        for (int i = 0; i < playerGO.Count; i++)
        {
            playerGO[i].GetComponent<Unit>().stat.accuracy -= 10f; // 수치는 미정
        }
    }

    public void Reincarnation(SkillData skillData)
    {
        battleManager.bossPassive = ReincarnationActive;
        int passiveDuration = battleManager.BigTurnCount;
    }

    public void ReincarnationActive(SkillData skillData)
    {

    }
}
