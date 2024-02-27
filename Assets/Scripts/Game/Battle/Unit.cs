using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Scripts.Entity;
using Unity.VisualScripting;
using Scripts.Data;
using static Enemy_Base;
using JetBrains.Annotations;
using Unity.IO.LowLevel.Unsafe;

public class Unit : MonoBehaviour
{
    public Class _class;
    public DeprecatedStat stat;
    public SpriteOutline outline;
    public BattleSkill[] skills = new BattleSkill[4];
    public SkillData[] skillDatas = new SkillData[4];
    public BattleSkill exampleskill = new BattleSkill();
    public bool isguard;
    public string Name;

    public void Awake()
    {
        skills[0] = new BattleSkill() { Name = "fireball", Infomation = "It's an Infomation", Property = "fire",Cost = 30};
        skills[1] = new BattleSkill() { Name = "fireball222", Infomation = "22222", Property = "fire", Cost = 40 };
        skills[2] = new BattleSkill() { Name = "fireball3333", Infomation = "3333333", Property = "fire", Cost = 50 };
        skills[3] = new BattleSkill() { Name = "fireball44444", Infomation = "444444444", Property = "fire", Cost = 60 };
    }

    public void OnOutline()
    {
        outline.OnOff = true;
    }

    public void OffOutline()
    {
        outline.OnOff = false;
    }

    public int position = -1;

    public string unitName;
    public string className;

    public int unitLevel;

    public float maxHP;
    public float maxMP;
    public float currentHP;
    public float currentMP;

    public bool isdead = false;
    private HUDmanager HUD;

    public void ConnectHUD(HUDmanager getHUD)
    {
        HUD = getHUD;
        //HUD.SetupHUD(this);
    }

    public void TakeDamage(float damage, AttackType attackType, Enemy_Base.AttackProperty attackProperty)  //유닛 체력 계산
    {
        if(isdead) return;

        currentHP -= damage;
        if(currentHP <= 0)
        {
            currentHP = 0;
            isdead = true;
            HUD.UpdateHUD(0, maxHP);
            return;
        }

        HUD.UpdateHUD(currentHP, currentMP);
    }

    public void ConsumeMP(float consume_amount)  //유닛 마나 계산
    {
        currentMP -= consume_amount;
        if (currentMP <= 0)
        {
            currentMP += consume_amount;
        }
        HUD.UpdateHUD(currentHP, currentMP);
    }
    
    public void MPShort()
    {
        Debug.Log("마나 부족");
    }
}
