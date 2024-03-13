using GeneralMonsterStates;
using System.Collections;
using Scripts;
using System;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Data;
using static Enemy_Base;
using UnityEditor.PackageManager;

public class Enemy_Skill
{
    private static Enemy_Skill instance;
    public static Enemy_Skill Instance { get { return instance; } }

    public List<Action<SkillData, EnemyStatData>> GetSkillList(int floor, string enemyName)
    {
        List<Action<SkillData, EnemyStatData>> skillLists = new List<Action<SkillData, EnemyStatData>>();
        switch(floor)
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
                        skillLists.Add(EnemyAttack); // 코어 활성화(패시브)
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
                switch(enemyName)
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
                        skillLists.Add(EnemyAttack);
                        break;
                    case "도적궁수":
                        skillLists.Add(EnemyAttack);
                        skillLists.Add(EnemyAttack);
                        break;
                    case "도적대장":
                        skillLists.Add(EnemyAttack);
                        skillLists.Add(EnemyAttack);
                        skillLists.Add(EnemyAttack);
                        break;
                    case "죽지못한갑옷":
                        skillLists.Add(EnemyAttack);
                        skillLists.Add(EnemyAttack);
                        skillLists.Add(EnemyAttack);
                        break;
                    case "몰락한여왕":
                        skillLists.Add(EnemyAttack);
                        skillLists.Add(EnemyAttack);
                        skillLists.Add(EnemyAttack);
                        skillLists.Add(EnemyAttack);
                        break;
                }
                break;
        }
        
        return skillLists;
    }
    public GameObject[] GetOpponent(TargetType enemyTargetType) // 공격대상을 받아오는 함수
    {
        GameObject[] go = null;
        switch (enemyTargetType)
        {
            case TargetType.Single:
                int AttackRange = Utility.WeightedRandom(20, 20, 20, 20, 20);
                go[0] = GameObject.Find("Player (" + AttackRange + ")(Clone)");
                break;
            case TargetType.Front:
                break;
            case TargetType.Back:
                break;
            case TargetType.Area:
                for (int i = 0; i < 5; i++)
                {
                    go[i] = GameObject.Find("Player (" + i + ")(Clone)");
                }
                break;
        }
        return go;
    }

    public void EnemyAttack(SkillData skillData , EnemyStatData enemyStatData = null) // 특별한 로직이 아닌 일반적인 공격
    {
        GameObject[] Opponent = GetOpponent(skillData.enemyTargetType);
        for(int i =0; i < Opponent.Length; i++)
        {
            BuffManager buffManager = Opponent[i].GetComponent<BuffManager>();
            Unit unit = Opponent[i].GetComponent<Unit>();
            unit.TakeDamage(skillData.physicsDamage, skillData.attackType);
            if(skillData.isDebuff == true)
            {
                //디버프 처리
            }
        }
    }

    public List<Action<SkillData, EnemyStatData>> Core_Active() // 이것만 건드리면 됨
    {
        List<Action<SkillData, EnemyStatData>> skillLists = new List<Action<SkillData, EnemyStatData>>
        {
            EnemyAttack
        };
        return skillLists;
    }

    public void Berserk(SkillData skillData, EnemyStatData enemyStatData)
    {
        enemyStatData.atk++;
        enemyStatData.agi++;
        enemyStatData.def--;
        enemyStatData.mdef--; // 수치는 모름
    }
}
