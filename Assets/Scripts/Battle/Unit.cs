using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Entity;
using Unity.VisualScripting;

public class Unit : MonoBehaviour
{
    public Class _class;
    public Stat stat;
    public AdvancedStat advancedStat;

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

    public void TakeDamage(float damage)
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

    public void ConsumeMP(float consume_amount)
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
