using Script.Data;
using Scripts.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnemyStatData : ScriptableObject, IDBdata
{
    /// <summary>
    /// 적 이름
    /// </summary>
    public string name;
    /// <summary>
    /// 기본 체력
    /// </summary>
    public float hp;
    /// <summary>
    /// 기본 공격력
    /// </summary>
    public float atk;
    /// <summary>
    /// 기본 방어력
    /// </summary>
    public float def;
    /// <summary>
    /// 기본 속성방어율
    /// </summary>
    public float mdef;

    /// <summary>
    /// 적중률
    /// </summary>
    public float accuracy;
    /// <summary>
    /// 회피률
    /// </summary>
    public float dodge;
    /// <summary>
    /// 크리확률
    /// </summary>
    public float critical;
    /// <summary>
    /// 공격저항력
    /// </summary>
    public float strcret;
    ///<summary>
    ///마법저항력
    ///</summary>
    public float magcret;

    /// <summary>
    /// 기본 근력
    /// </summary>
    public float str;
    /// <summary>
    /// 기본 생명력
    /// </summary>
    public float vit;
    /// <summary>
    /// 기본 마법력
    /// </summary>
    public float mag;
    /// <summary>
    /// 기본 민첩
    /// </summary>
    public float agi;
    /// <summary>
    /// 기본 운
    /// </summary>
    public float luk;

    /// <summary>
    /// 적 행동 속성
    /// </summary>
    public EnemyProperty Property;
    /// <summary>
    /// 취약 방어 타입
    /// </summary>
    public AttackType WeakType;
    /// <summary>
    /// 방어 타입
    /// </summary>
    public AttackType ResistType;
    
}
