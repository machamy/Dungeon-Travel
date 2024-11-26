using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts;
using Scripts.Data;

public class BuffManager : MonoBehaviour
{
    public class BuffData
    {
        public int duration;
        public float damage;
        public BuffData(int duration, float figure)
        {
            this.duration = duration;
            this.damage = figure;
        }
    }
    public Dictionary<DebuffType, BuffData> debuffDic = new Dictionary<DebuffType, BuffData>(); // 디버프 타입 저장 딕셔너리
    public void ApplyEffect()
    {
        if (debuffDic == null)
            return;
        else
        {
            foreach(DebuffType debuffType in debuffDic.Keys)
            {
                if (debuffDic.ContainsKey(debuffType))
                {
                    debuffDic[debuffType].duration -= 1;
                    Unit unit = gameObject.GetComponent<Unit>();
                    unit.TakeDamage(debuffDic[debuffType].damage); // 공격 타입은 미정
                    if (debuffDic[debuffType].duration <= 0)
                    {
                        debuffDic.Remove(debuffType);
                    }
                }
            }
        }
    }
    public void DebuffAdd(DebuffType debuffType,float probability, int duration, float figure)
    {
        if (debuffDic[debuffType] != null)
        {
            return;
        }
        else
        {
            float weight = Utility.WeightedRandom(probability, 100 - probability);
            if (weight <= probability)
            {
                BuffData buffData = new BuffData(duration, figure);
                debuffDic.Add(debuffType, buffData);
            }
            else
                return;
        }
    }
}

