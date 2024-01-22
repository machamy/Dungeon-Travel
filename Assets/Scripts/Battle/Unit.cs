using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Entity;

public class Unit : MonoBehaviour
{
    public Class _class;
    public Stat stat;

    public int position;
    public string unitName;
    public int unitLevel;
    public float maxHP;
    public float maxMP;
    public float currentHP;
    public float currentMP;

    public float ATK, DEF;
}
