using UnityEngine;

namespace Scripts.Player
{
    public class Entity : MonoBehaviour
    {
        [SerializeField]
        protected int lv;
        [SerializeField]
        protected string name;
        
        /// <summary>
        /// 아이템/버프/2차스탯이 반영되기 전의 스탯
        /// </summary>
        public Stat rawStat;

        /// <summary>
        /// 2차 스탯.
        /// </summary>
        public AdvancedStat advancedStat;

        /// <summary>
        /// MaxHP : hp + vit + 장비 + 버프
        /// </summary>
        public float MaxHp => rawStat.hp + advancedStat.vit; // + 장비 + 버프
        /// <summary>
        /// MaxMP : mp + mag + 장비 + 버프
        /// </summary>
        public float MaxMP => rawStat.mp + advancedStat.mag; // + 장비 + 버프
        /// <summary>
        /// ATK : atk + str + 장비 + 버프
        /// </summary>
        public float ATK => rawStat.atk + advancedStat.str; // + 장비 + 버프
         /// <summary>
         /// DEF : def  + vit  + 장비 + 버프
         /// </summary>
        public float DEF => rawStat.def + advancedStat.vit; // + 장비 + 버프

        /// <summary>
        /// 현재 HP
        /// </summary>
        public float currentHP;
        /// <summary>
        /// 현재 MP
        /// </summary>
        public float currentMP;
        
        
    }
}