using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Entity;
using Scripts.Data;

public class UnitHolder
{
    public Character character {  get; private set; }
    public SkillData skillData;

    public GameObject gameObject;
    public int position;
    public bool isFriendly;
    public bool isDead = false;

    public string name => character.name;
    public float hp { get { return character.hp; } set { character.hp = value; } }
    public float mp { get { return character.mp; } set { character.mp = value; } }
    public float agi { get { return character.agi; } set { character.agi = value; } }

    public void SetCharacter(Character character)
    {
        this.character = character;
    } 
}
