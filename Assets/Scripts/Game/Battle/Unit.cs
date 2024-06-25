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
using System;

public class Unit : MonoBehaviour
{
    public Class _class;
    public DeprecatedStat stat;
    public BattleSkill[] skills = new BattleSkill[4];

    public SkillData[] skillDatas = new SkillData[4];
    public bool isguard;

    public Unit() { }
    public Unit(HUDmanager hud)
    {
        isDead = false;
        isPlayer = true;
        HUD = hud;
        HUD.SetupHUD(this);
        atk = 5;

        maxHP = 50; maxMP = 50; currentHP = 50; currentMP = 50;

        skills[0] = new BattleSkill() { Name = "fireball", Infomation = "Infomation panel", Property = "fire", Cost = 30 };
        skills[1] = new BattleSkill() { Name = "fireball222", Infomation = "22222", Property = "fire", Cost = 40 };
        skills[2] = new BattleSkill() { Name = "fireball3333", Infomation = "3333333", Property = "fire", Cost = 50 };
        skills[3] = new BattleSkill() { Name = "fireball44444", Infomation = "444444444", Property = "fire", Cost = 60 };
    }
    public int atk;

    public string unitName;
    public string className;

    public int unitLevel;

    public float maxHP;
    public float maxMP;
    public float currentHP;
    public float currentMP;

    public bool isPlayer;
    public bool isDead;
    private HUDmanager HUD;

    public void Connect(HUDmanager hud)
    {
        HUD = hud;
        HUD.SetupHUD(this);
    }


    #region 배틀관련
    public void TakeDamage(float damage, AttackType attackType)  //유닛 체력 계산
    {
        if(isDead) return;

        currentHP -= damage;
        if(currentHP <= 0)
        {
            Debug.Log(unitName + " 사망");
            currentHP = 0;
            isDead = true;
            HUD.DeadColor();
            HUD.UpdateHUD(0, maxHP);
            if (isPlayer) { BattleManager.Instance.PlayerDead(); }
            else { BattleManager.Instance.EnemyDead(); }
            return;
        }

        HUD.UpdateHUD(currentHP, currentMP);
    }

    /// <summary>
    /// 마나가 충분한지 아닌지 판단, 충분하면 true 아니면 false
    /// </summary>
    /// <param name="necessaryMP"></param>
    /// <returns></returns>
    public bool enoughMP(float necessaryMP)
    {
        if (currentMP < necessaryMP) return false;
        else return true;
    }

    /// <summary>
    /// 마나가 충분하지 않으면 false, 마나가 충분하면 마나소모를 한 후 true반환
    /// </summary>
    /// <param name="consume_amount"></param>
    /// <returns></returns>
    public void ConsumeMP(float consume_amount)  //유닛 마나 계산
    {
        currentMP -= consume_amount;
        HUD.UpdateHUD(currentHP, currentMP);
    }
    #endregion

    #region 아웃라인관련
    private Color color = Color.red;
    private bool OnOff;
    public int outlineSize = 2;

    private SpriteRenderer spriteRenderer;

    void OnEnable()
    {
        UpdateOutline(false);
    }
    public void UpdateOutline(bool outline)
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        MaterialPropertyBlock mpb = new MaterialPropertyBlock();
        spriteRenderer.GetPropertyBlock(mpb);
        mpb.SetFloat("_Outline", outline ? 1f : 0);
        mpb.SetColor("_OutlineColor", color);
        mpb.SetFloat("_OutlineSize", outlineSize);
        spriteRenderer.SetPropertyBlock(mpb);
    }
    #endregion
}
