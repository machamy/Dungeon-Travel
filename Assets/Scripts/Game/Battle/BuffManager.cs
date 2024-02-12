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
        Burn,
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
        if (debuffDic.ContainsKey(DeBuffType.Burn)) // 화상일때
        {
            debuffDic[DeBuffType.Burn].duration -= 1;
            Unit unit = gameObject.GetComponent<Unit>();
            unit.currentHP -= debuffDic[DeBuffType.Burn].damage;

            if (debuffDic[DeBuffType.Burn].duration <= 0)
            {
                debuffDic.Remove(DeBuffType.Burn);
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
    public void Burn(int duration, float damage, float probability) // 화상
    {
        if (debuffDic[DeBuffType.Burn] != null)
        {
            debuffDic[DeBuffType.Burn].duration += duration;
        }
        else
        {
            int weight = UnityEngine.Random.Range(0, 100); // 확률은 아직
            if (weight <= probability)
            {
                BuffData buffData = new BuffData(duration, damage);
                debuffDic.Add(DeBuffType.Burn, buffData);
            }
            else
                return;
        }
    }
    public void Poison(int duration, float damage, float probability) // 독
    {
        if (debuffDic[DeBuffType.Poison] != null)
        {
            debuffDic[DeBuffType.Poison].duration += duration;
        }
        else
        {
            int weight = UnityEngine.Random.Range(0, 100); // 확률은 아직
            if (weight <= probability)
            {
                BuffData buffData = new BuffData(duration, damage);
                debuffDic.Add(DeBuffType.Poison, buffData);
            }
            else
                return;
        }
    }
    public void Blind(int duration, float damage, float probability) // 실명
    {

    }
    public void Stun(int duration, float damage, float probability) // 기절
    {
        if (debuffDic[DeBuffType.Stun] != null)
        {
            debuffDic[DeBuffType.Stun].duration += duration;
        }
        else
        {
            isStun = true;
            int weight = UnityEngine.Random.Range(0, 100); // 확률은 아직
            if (weight <= probability)
            {
                BuffData buffData = new BuffData(duration, damage);
                debuffDic.Add(DeBuffType.Stun, buffData);
            }
            else
                return;
        }
    }
    public void Silence(int duration, float damage, float probability) // 침묵
    {
        if (debuffDic[DeBuffType.Silence] != null)
        {
            debuffDic[DeBuffType.Silence].duration += duration;
        }
        else
        {
            isSilence = true;
            int weight = UnityEngine.Random.Range(0,100); // 확률은 아직
            if (weight <= probability)
            {
                BuffData buffData = new BuffData(duration, damage);
                debuffDic.Add(DeBuffType.Silence, buffData);
            }
            else
                return;
        }
    }
    public void Confuse(int duration, float damage, float probability)
    {
        if (debuffDic[DeBuffType.Confuse] != null)
        {
            debuffDic[DeBuffType.Confuse].duration += duration;
        }
        else
        {
            int weight = UnityEngine.Random.Range(0, 100); // 확률은 아직
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

