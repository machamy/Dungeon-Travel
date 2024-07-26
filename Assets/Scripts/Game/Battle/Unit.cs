using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Scripts.Entity;
using Unity.VisualScripting;
using Scripts.Data;
using JetBrains.Annotations;
using Unity.IO.LowLevel.Unsafe;
using System;

public class Unit : MonoBehaviour
{
    public Class _class;
    public StatData stat;
    public EnemyStatData enemyStat;

    public SkillData[] skills = new SkillData[4];
    public bool isGuard;
    public int atk;

    public string unitName;
    public string className;
    public AttackType weakType;

    public int unitLevel;

    public float maxHP;
    public float maxMP;
    public float currentHP;
    public float currentMP;

    public bool isEnemy, isBoss;
    public bool isDead;
    private HUDmanager HUD;
    private BattleManager battleManager;

    IEnemy enemy;
    private Character character;

    #region 초기세팅
    public void InitialSetting(BattleManager battleManager, HUDmanager hud, Character character = null) // 플레이어 초기 세팅
    {
        this.battleManager = battleManager;
        spriteRenderer = GetComponent<SpriteRenderer>();
        isDead = false;
        if(character == null)
        {
            isEnemy = true;
        }
        else
        {
            isEnemy = false;
            this.character = character;
            stat = character.FinalStat;
            maxHP = stat.hp;
            maxMP = stat.mp;
            currentHP = maxHP;
            currentMP = maxMP;
        }
        HUD = hud; HUD.SetupHUD(this);
    }

    public void EnemySetting(IEnemy enemy) // 적 초기 세팅
    {
        isEnemy = true;
        this.enemy = enemy;
        enemyStat = enemy.EnemyStatData;
        maxHP = enemyStat.hp;
        currentHP = enemyStat.hp;
        weakType = enemyStat.weakType;
        HUD.SetupHUD(this);
    }
    #endregion

    #region 배틀관련
    public void OnGuard()
    {
        Debug.Log("가드");
        isGuard = true;
    }

    public void Attack() // 적의 공격
    {
        enemy.Attack();
    }

    public void Attack(Unit targetUnit) // 플레이어의 공격
    {
        if (targetUnit.enemyStat == null)
        {
            Debug.LogError("잘못된 공격함수 사용");
            return;
        }
        else
        {
            targetUnit.TakeDamage(5); // 임시
        }
    }

    public void TakeDamage(float damage)  //유닛 체력 계산
    {
        if(isDead) return;
        if (isGuard)
        {
            Debug.Log("방어");
            isGuard = false;
            return;
;       }

        currentHP -= damage;
        if(currentHP <= 0)
        {
            currentHP = 0;
            HUD.Dead();
            Debug.Log(unitName + " 사망");
            if (isEnemy ) { battleManager.EnemyDead(); }
            else { battleManager.PlayerDead(); }
            isDead = true;
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

    public bool IsDead() { return isDead; }
    #endregion

    #region 아웃라인관련
    private Color color = Color.red;
    private int outlineSize = 2;

    private SpriteRenderer spriteRenderer;

    public void UpdateOutline(bool outline)
    {
        MaterialPropertyBlock mpb = new MaterialPropertyBlock();
        spriteRenderer.GetPropertyBlock(mpb);
        mpb.SetFloat("_Outline", outline ? 1f : 0);
        mpb.SetColor("_OutlineColor", color);
        mpb.SetFloat("_OutlineSize", outlineSize);
        spriteRenderer.SetPropertyBlock(mpb);
    }
    #endregion
}
