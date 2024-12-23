using Scripts.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newBattleSkill", menuName = "CreateBattleSkill")]
public class BattleSkill : ScriptableObject
{
    public string skillName;
    public string infomation;
    public string property;
    public string type;
    public float atk;
    public int cost;
    public bool isAttack;
    public bool isBuff;
    public bool isHeal;
    public TargetType targetType;
    public AttackType attackType;
}
