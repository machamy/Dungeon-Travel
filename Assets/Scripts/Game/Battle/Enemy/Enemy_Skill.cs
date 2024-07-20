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
    IEnemy enemy;
    public Enemy_Skill(IEnemy enemy)
    {
        this.enemy = enemy;
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
        GameObject[] go = new GameObject[5];
        switch (enemyTargetType)
        {
            case TargetType.Single:
                int AttackRange = Utility.WeightedRandom(20, 20, 20, 20, 20);
                GameObject clone = GameObject.Find($"Player ({AttackRange})(Clone)");
                go[0] = clone;
                break;
            case TargetType.Front:
                break;
            case TargetType.Back:
                break;
            case TargetType.Area:
                for (int i = 0; i < 5; i++)
                {
                    GameObject _clone = GameObject.Find($"Player ({i})(Clone)");
                    go[i] = _clone;
                }
                break;
        }
        return go;
    }

    public void EnemyAttack(SkillData skillData) // 특별한 로직이 아닌 일반적인 공격
    {
        GameObject[] Opponent = GetOpponent(skillData.enemyTargetType);
        for (int i = 0; i < Opponent.Length; i++)
        {
            if (Opponent[i] == null)
                continue;
            BuffManager buffManager = Opponent[i].GetComponent<BuffManager>();
            Unit unit = Opponent[i].GetComponent<Unit>(); // 데미지 계산식 나오면 수정
            unit.TakeDamage(skillData.physicsDamage);
            Debug.Log("공격완료");
            if (skillData.debuffType != DebuffType.None)
                buffManager.DebuffAdd(skillData.debuffType, skillData.buffRatio, 2, 0); // 데미지는 아직 정해지지 않음
        }
    }

    public void Core_Active(SkillData skillData)
    {
        enemy.Index = 3; // 다음 공격을 코어 레이저로 고정
        enemy.EnemyStatData.atk++; // 수치는 모름
    }

    public void Berserk(SkillData skillData)
    {
        enemy.EnemyStatData.atk++;
        enemy.EnemyStatData.agi++;
        enemy.EnemyStatData.def--;
        enemy.EnemyStatData.mdef--; // 수치는 모름
    }

    public void Cut_Stab(SkillData skillData)
    {
        AttackType[] attackType = Enum.GetValues(typeof(AttackType)) as AttackType[];
        GameObject[] Opponent = GetOpponent(skillData.enemyTargetType);
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
        if(BattleManager.Instance.aliveEnemy < 3)
        {
            for(int i = BattleManager.Instance.aliveEnemy; i < 3; i++)
            {
                BattleManager.Instance.EnemySpawn(2, "도적선봉대");
            }
        }
    }
}
