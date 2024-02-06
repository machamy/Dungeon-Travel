using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Entity;
using Unity.VisualScripting;
using Scripts.Data;
using static Enemy_Base;

public class Unit : MonoBehaviour
{
    public Class _class;
    public Stat stat;

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
        HUD.SetupHUD(maxHP, maxMP);
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

    public void Update()
    {
        if(isdead)
        {
            
        }
    }
}
