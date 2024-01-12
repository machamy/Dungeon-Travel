using System;

namespace Scripts.Player
{
    
    /// <summary>
    /// 1차 스탯
    /// HP, MP, ATK, DEF
    /// </summary>
    [Serializable]
    public class Stat : ICloneable
    {
        public float hp;
        public float mp;
        public float atk;
        public float def;
        public object Clone()
        {
            Stat clone = new Stat();
            clone.hp = hp;
            clone.mp = mp;
            clone.atk = atk;
            clone.def = def;
            return clone;
        }
    }

    /// <summary>
    /// 2차 스탯
    /// STR, VIT, MAG, AGI, LUK
    /// </summary>
    [Serializable]
    public class AdvancedStat : ICloneable
    {
        public float str;
        public float vit;
        public float mag;
        public float agi;
        public float luk;
        
        public object Clone()
        {
            AdvancedStat clone = new AdvancedStat();
            clone.str = str;
            clone.vit = vit;
            clone.mag = mag;
            clone.agi = agi;
            clone.luk = luk;
            
            return clone;
        }
    }
    
}