using Scripts.Data;
using Scripts.Entity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEnemyUnit : BattleUnit
{
    public List<SkillData> skillDatas = new List<SkillData>();
    Enemy_Skill skill;
    public int index = -1;
    public override void Initialize(int floor, string name)
    {
        statData = DB.GetEnemyData(floor, name);
        skillDatas = DB.GetEnemySkillData(floor, name);
        //skill = new Enemy_Skill(this, battleManager);

        currentHP = statData.hp;
        currentMP = statData.mp;

        Debug.Log($"Unit Initialized: {statData.name} - HP: {currentHP}, MP: {currentMP}, ATK: {statData.atk}");
    }
}
