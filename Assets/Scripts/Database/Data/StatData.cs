using System;
using Script.Data;
using Script.Global;

namespace Scripts.Data
{
    
    /// <summary>
    /// 스탯
    /// </summary>
    [Serializable]
    public class StatData :IDBdata, ICloneable
    {
        //1차 스탯
        /// <summary>
        /// 기본 체력
        /// </summary>
        public float hp;
        /// <summary>
        /// 기본 mp
        /// </summary>
        public float mp;
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
        /// 근력 보정치
        /// </summary>
        public float strWeight;
        /// <summary>
        /// 마법 보정치
        /// </summary>
        public float magWeight;
        /// <summary>
        /// 생명 보정치
        /// </summary>
        public float vitWeight;
        
        //2차스탯
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
        
        
        
        public object Clone()
        {
            StatData clone = new StatData();
            clone.hp = hp;
            clone.mp = mp;
            clone.atk = atk;
            clone.def = def;
            clone.mdef = mdef;

            clone.strWeight = strWeight;
            clone.magWeight = magWeight;
            clone.vitWeight = vitWeight;
            
            clone.str = str;
            clone.vit = vit;
            clone.mag = mag;
            clone.agi = agi;
            clone.luk = luk;
            return clone;
        }

        public static StatData operator+(StatData origin, StatData other)
        {
            StatData result = (StatData)origin.Clone();
            result.hp += other.hp;
            result.mp += other.mp;
            result.atk += other.atk;
            result.def += other.def;
            result.mdef += other.mdef;
            
            result.strWeight += other.strWeight;
            result.magWeight += other.magWeight;
            result.vitWeight += other.vitWeight;
            
            result.str += other.str;
            result.vit += other.vit;
            result.mag += other.mag;
            result.agi += other.agi;
            result.luk += other.luk;
            return result;
        }
    }

}