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
    public Stat stat;
    public SpriteOutline outline;
    public BattleSkill[] skills = new BattleSkill[4];
    public BattleSkill exampleskill = new BattleSkill();
    public bool isguard;

    public void Awake()
    {
        outline = GetComponent<SpriteOutline>();
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
        HUD.SetupHUD(maxHP, maxMP,unitName,className);
    }

    public void TakeDamage(float damage, AttackType attackType, AttackProperty attackProperty)  //유닛 체력 계산
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
