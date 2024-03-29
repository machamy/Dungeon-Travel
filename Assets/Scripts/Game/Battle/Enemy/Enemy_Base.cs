using Scripts;
using Scripts.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Scripts.Entity;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy_Base
{
    private Unit unit;
    public bool isDead = false;
    public float hp;
    private Enemy enemy;
    private Boss boss;
    public enum AttackProperty
    {
        Physics, // 물리
        Magic, // 마법
    }
    public Enemy_Base(Enemy enemy)
    {
        this.enemy = enemy;
        hp = enemy.enemyStatData.hp;
    }
    public Enemy_Base(Boss boss)
    {
        this.boss = boss;
    }
    public virtual void Init()
    {

    }
    public float Agi
    {
        get { return enemy.enemyStatData.agi; }
    }


    public float Hp
    {
        get { return enemy.enemyStatData.hp; }
    }
}
