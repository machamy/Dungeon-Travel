using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts;

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
    private enum DeBuffType
    {
        Blood,
        Poison,
        Blind,
        Stun,
        Silence,
        Confuse,
    }
    Dictionary<DeBuffType, BuffData> debuffDic = new Dictionary<DeBuffType, BuffData>(); // 디버프 타입 저장 딕셔너리
    public void ApplyEffect()
    {
        if (debuffDic == null)
            return;
        if (debuffDic.ContainsKey(DeBuffType.Blood)) // 화상일때
        {
            debuffDic[DeBuffType.Blood].duration -= 1;
            Unit unit = gameObject.GetComponent<Unit>();
            unit.currentHP -= debuffDic[DeBuffType.Blood].damage;

            if (debuffDic[DeBuffType.Blood].duration <= 0)
            {
                debuffDic.Remove(DeBuffType.Blood);
            }
        }
        if (debuffDic.ContainsKey(DeBuffType.Poison)) // 독일때 
        {
            debuffDic[DeBuffType.Poison].duration -= 1;
            Unit unit = gameObject.GetComponent<Unit>();
            unit.currentHP -= debuffDic[DeBuffType.Poison].damage;

            if (debuffDic[DeBuffType.Poison].duration <= 0)
            {
                debuffDic.Remove(DeBuffType.Poison);
            }
        }
        if(debuffDic.ContainsKey(DeBuffType.Blind)) // 실명일때
        {
            
        }
        if(debuffDic.ContainsKey(DeBuffType.Silence)) // 침묵일때
        {
            debuffDic[DeBuffType.Silence].duration -= 1;
            if (debuffDic[DeBuffType.Silence].duration <= 0)
            {
                isSilence = false;
                debuffDic.Remove(DeBuffType.Silence);
            }
        }
        if(debuffDic.ContainsKey(DeBuffType.Stun)) // 기절일때
        {
            debuffDic[DeBuffType.Stun].duration -= 1;
            if (debuffDic[DeBuffType.Stun].duration <= 0)
            {
                isStun = false;
                debuffDic.Remove(DeBuffType.Stun);
            }
        }
        if (debuffDic.ContainsKey(DeBuffType.Confuse)) // 혼란일때
        {

        }
    }
    public void Blood(int probability, int duration, float damage) // 화상
    {
        if (debuffDic[DeBuffType.Blood] != null)
        {
            return;
        }
        else
        {
            int weight = Utility.WeightedRandom(probability, 100 - probability);
            if (weight <= probability)
            {
                BuffData buffData = new BuffData(duration, damage);
                debuffDic.Add(DeBuffType.Blood, buffData);
            }
            else
                return;
        }
    }
    public void Poison(int probability, int duration, float damage) // 독
    {
        if (debuffDic[DeBuffType.Poison] != null)
        {
            return;
        }
        else
        {
            int weight = Utility.WeightedRandom(probability, 100 - probability);
            if (weight <= probability)
            {
                BuffData buffData = new BuffData(duration, damage);
                debuffDic.Add(DeBuffType.Poison, buffData);
            }
            else
                return;
        }
    }
    public void Blind(int probability, int duration, float damage = 0) // 실명
    {

    }
    public void Stun(int probability, int duration, float damage = 0) // 기절
    {
        if (debuffDic[DeBuffType.Stun] != null)
        {
            return;
        }
        else
        {
            isStun = true;
            int weight = Utility.WeightedRandom(probability, 100 - probability);
            if (weight <= probability)
            {
                BuffData buffData = new BuffData(duration, damage);
                debuffDic.Add(DeBuffType.Stun, buffData);
            }
            else
                return;
        }
    }
    public void Silence(int probability, int duration, float damage = 0) // 침묵
    {
        if (debuffDic[DeBuffType.Silence] != null)
        {
            return;
        }
        else
        {
            isSilence = true;
            int weight = Utility.WeightedRandom(probability, 100 - probability);
            if (weight <= probability)
            {
                BuffData buffData = new BuffData(duration, damage);
                debuffDic.Add(DeBuffType.Silence, buffData);
            }
            else
                return;
        }
    }
    public void Confuse(int probability, int duration, float damage = 0)
    {
        if (debuffDic[DeBuffType.Confuse] != null)
        {
            return;
        }
        else
        {
            int weight = Utility.WeightedRandom(probability, 100 - probability);
            if (weight <= probability)
            {
                BuffData buffData = new BuffData(duration, damage);
                debuffDic.Add(DeBuffType.Confuse, buffData);
            }
            else
                return;
        }
    }
}

