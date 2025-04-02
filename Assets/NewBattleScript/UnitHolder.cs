using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Entity;
using Scripts.Data;

public class UnitHolder
{
    Character character;

    public List<SkillData> skillData = new List<SkillData>();

    public GameObject gameObject;
    public int position;
    public bool isFriendly;
    public bool isDead = false;
    public bool guard = false;

    public enum StatusEffect { stun, blind, silence, confusion}
    public Dictionary<StatusEffect, int> statusEffect;

    public string name { get { return character.name; } set { character.name = value; } }
    public string className { get { return character._class.name; } }
    public int lv { get { return character.LV; } }
    public float hp { get { return character.hp; } set { character.hp = value; } }
    public float mp { get { return character.mp; } set { character.mp = value; } }
    public float agi { get { return character.agi; } set { character.agi = value; } }

    public float rawHp { get { return character.rawBaseStat.hp; } set { character.rawBaseStat.hp = value;} }
    public float rawMp { get { return character.rawBaseStat.mp;} set { character.rawBaseStat.mp = value;} }

    public void SetCharacter(Character character)
    {
        this.character = character;
        statusEffect = new Dictionary<StatusEffect, int>();
    }

    public void TakeDamage()
    {
        
    }

    public void Attack()
    {
        if (statusEffect.ContainsKey(StatusEffect.blind))
        {

        }
    }

    public void Skill(SkillData skill)
    {

    }
    
    public void TurnEnd()
    {
        foreach(StatusEffect effect in statusEffect.Keys)
        {
            statusEffect[effect]--;
            if (statusEffect[effect] == 0) statusEffect.Remove(effect);
        }
    }
}
