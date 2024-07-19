using Scripts.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemy
{
    public void Attack();
    public void NewEnemy(int floor, string name, GameObject gameObject);
    public List<Action<SkillData>> SkillLists { get; set; }
    public EnemyStatData EnemyStatData { get; set; }
}
