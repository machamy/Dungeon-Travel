using Scripts.Data;
using Scripts.Entity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts;
using System;

public class BattleEnemyUnit : BattleUnit
{
    public List<SkillData> skillDatas = new List<SkillData>();
    Enemy_Skill skill;
    private List<Action<SkillData>> skillLists;
    public int index = -1;
    public override void Initialize(int floor, string name)
    {
        statData = DB.GetEnemyData(floor, name);
        skillDatas = DB.GetEnemySkillData(floor, name);
        skill = new Enemy_Skill(this);
        skillLists = skill.GetSkillList(floor, name);

        currentHP = statData.hp;
        currentMP = statData.mp;

        Debug.Log($"Unit Initialized: {statData.name} - HP: {currentHP}, MP: {currentMP}, ATK: {statData.atk}");
        spriteRenderer = GetComponent<SpriteRenderer>();
        buffManager = GetComponent<BuffManager>();
    }

    public override void Attack(BattleUnit target = null, BattleSkill skillData = null)
    {
        Debug.Log($"{statData.name} attack");
        if (buffManager.debuffDic.ContainsKey(DebuffType.Stun)) // 기절이라면 공격 함수 실행 x
            return;
        int[] weightArr = new int[5];
        int i = 0;
        foreach (SkillData skilldata in skillDatas)
        {
            weightArr[i++] = skilldata.skillWeight;
        }
        if (index == -1) // 미리 지정되있는 스킬이 없을때
        {
            index = Utility.WeightedRandom(weightArr);
            if (buffManager.debuffDic.ContainsKey(DebuffType.Silence)) // 침묵이라면  skillLists[0]에 저장되어 있는 기본공격만 하도록
                index = 0;
        }
        //Debug.Log($"index = {index}");
        skillLists[index].Invoke(skillDatas[index]);
        index = -1;
    }
}
