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
        public BuffData(int duration, float damage)
        {
            this.duration = duration;
            this.damage = damage;
        }
    }
    public bool isSilence = false;
    public bool isStun = false;
    Dictionary<DebuffType, BuffData> debuffDic = new Dictionary<DebuffType, BuffData>(); // 디버프 타입 저장 딕셔너리
    public Dictionary<DebuffType, Action<float, int, float>> debuffAction = new Dictionary<DebuffType, Action<float, int, float>>();

    public void Awake()
    {
        debuffAction.Add(DebuffType.Blood, Blood);
        debuffAction.Add(DebuffType.Poison, Poison);
        debuffAction.Add(DebuffType.Silence, Silence);
        debuffAction.Add(DebuffType.Blind, Blind);
        debuffAction.Add(DebuffType.Confuse, Confuse);
        debuffAction.Add(DebuffType.Stun, Stun);
    }

    public void ApplyEffect()
    {
        if (debuffDic == null)
            return;
        if (debuffDic.ContainsKey(DebuffType.Blood)) // 화상일때
        {
            debuffDic[DebuffType.Blood].duration -= 1;
            Unit unit = gameObject.GetComponent<Unit>();
            unit.currentHP -= debuffDic[DebuffType.Blood].damage;

            if (debuffDic[DebuffType.Blood].duration <= 0)
            {
                debuffDic.Remove(DebuffType.Blood);
            }
        }
        if (debuffDic.ContainsKey(DebuffType.Poison)) // 독일때 
        {
            debuffDic[DebuffType.Poison].duration -= 1;
            Unit unit = gameObject.GetComponent<Unit>();
            unit.currentHP -= debuffDic[DebuffType.Poison].damage;

            if (debuffDic[DebuffType.Poison].duration <= 0)
            {
                debuffDic.Remove(DebuffType.Poison);
            }
        }
        if(debuffDic.ContainsKey(DebuffType.Blind)) // 실명일때
        {
            
        }
        if(debuffDic.ContainsKey(DebuffType.Silence)) // 침묵일때
        {
            debuffDic[DebuffType.Silence].duration -= 1;
            if (debuffDic[DebuffType.Silence].duration <= 0)
            {
                isSilence = false;
                debuffDic.Remove(DebuffType.Silence);
            }
        }
        if(debuffDic.ContainsKey(DebuffType.Stun)) // 기절일때
        {
            debuffDic[DebuffType.Stun].duration -= 1;
            if (debuffDic[DebuffType.Stun].duration <= 0)
            {
                isStun = false;
                debuffDic.Remove(DebuffType.Stun);
            }
        }
        if (debuffDic.ContainsKey(DebuffType.Confuse)) // 혼란일때
        {

        }
    }
        public void Blood(float probability, int duration, float damage) // 화상
        {
        if (debuffDic[DebuffType.Blood] != null)
        {
            return;
        }
        else
        {
            float weight = Utility.WeightedRandom(probability, 100 - probability);
            if (weight <= probability)
            {
                BuffData buffData = new BuffData(duration, damage);
                debuffDic.Add(DebuffType.Blood, buffData);
            }
            else
                return;
        }
    }
    public void Poison(float probability, int duration, float damage) // 독
    {
        if (debuffDic[DebuffType.Poison] != null)
        {
            return;
        }
        else
        {
            float weight = Utility.WeightedRandom(probability, 100 - probability);
            if (weight <= probability)
            {
                BuffData buffData = new BuffData(duration, damage);
                debuffDic.Add(DebuffType.Poison, buffData);
            }
            else
                return;
        }
    }
    public void Blind(float probability, int duration, float damage = 0) // 실명
    {

    }
    public void Stun(float probability, int duration, float damage = 0) // 기절
    {
        if (debuffDic[DebuffType.Stun] != null)
        {
            return;
        }
        else
        {
            isStun = true;
            float weight = Utility.WeightedRandom(probability, 100 - probability);
            if (weight <= probability)
            {
                BuffData buffData = new BuffData(duration, damage);
                debuffDic.Add(DebuffType.Stun, buffData);
            }
            else
                return;
        }
    }
    public void Silence(float probability, int duration, float damage = 0) // 침묵
    {
        if (debuffDic[DebuffType.Silence] != null)
        {
            return;
        }
        else
        {
            isSilence = true;
            float weight = Utility.WeightedRandom(probability, 100 - probability);
            if (weight <= probability)
            {
                BuffData buffData = new BuffData(duration, damage);
                debuffDic.Add(DebuffType.Silence, buffData);
            }
            else
                return;
        }
    }
    public void Confuse(float probability, int duration, float damage = 0)
    {
        if (debuffDic[DebuffType.Confuse] != null)
        {
            return;
        }
        else
        {
            float weight = Utility.WeightedRandom(probability, 100 - probability);
            if (weight <= probability)
            {
                BuffData buffData = new BuffData(duration, damage);
                debuffDic.Add(DebuffType.Confuse, buffData);
            }
            else
                return;
        }
    }
}

