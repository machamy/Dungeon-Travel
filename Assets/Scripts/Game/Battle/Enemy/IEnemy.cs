using Scripts.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemy
{
    public void Attack(Action<SkillData> passive);
    public void NewEnemy(int floor, string name, GameObject gameObject, BattleManager battleManager);
    public List<Action<SkillData>> SkillLists { get; set; }
    public EnemyStatData EnemyStatData { get; set; }
    public int Index { get; set; }
    public Action<SkillData> Passive { get; set; }
    public BuffManager BuffManager { get; set; }
    public int CurrentHP { get; set; }
}
