using UnityEngine;
using UnityEngine.Serialization;

namespace Scripts.Player
{


    public class Entity : MonoBehaviour
    {
        [SerializeField]
        protected int lv;
        [SerializeField]
        protected string name;

        /// <summary>
        /// 아이템/버프/2차스탯이 반영되기 전 기본 스탯
        /// </summary>
        public Stat rawBaseStat;
        /// <summary>
        /// 아이템/버프 반영전 기본 2차 스탯.
        /// </summary>
        public AdvancedStat rawAdvancedStat;
        
        
        
        
        
        /// <summary>
        /// 최대 HP를 갱신하는 함수.
        /// 변동량에 따라 현재 hp도 변경한다
        /// </summary>
        /// <returns>갱신된 maxHP값</returns>
        public float UpdateMaxHP()
        {
            float newHP = rawBaseStat.hp + rawAdvancedStat.vit;  // + 장비 + 버프
            float delta = newHP - _maxHP;
            currentHP += delta;
            _maxHP = newHP;
            return _maxHP;
        }
        /// <summary>
        /// 최대 MP를 갱신하는 함수.
        /// 변동량에 따라 현재 mp도 변경한다
        /// </summary>
        /// <returns>갱신된 maxMP값</returns>
        public float UpdateMaxMP()
        {
            float newMP = rawBaseStat.mp + rawAdvancedStat.mag;  // + 장비 + 버프
            float delta = newMP - _maxMP;
            currentMP += delta;
            _maxMP = newMP;
            return _maxMP;
        }
        /// <summary>
        /// MaxHP : hp + vit + 장비 + 버프
        /// </summary>
        public float MaxHP => _maxHP;
        protected float _maxHP;

        /// <summary>
        /// MaxMP : mp + mag + 장비 + 버프
        /// </summary>
        public float MaxMP => _maxMP; // + 장비 + 버프
        protected float _maxMP;
        
        
        
        /// <summary>
        /// ATK : atk + str + 장비 + 버프
        /// </summary>
        public float ATK => rawBaseStat.atk + rawAdvancedStat.str; // + 장비 + 버프
         /// <summary>
         /// DEF : def  + vit  + 장비 + 버프
         /// </summary>
        public float DEF => rawBaseStat.def + rawAdvancedStat.vit; // + 장비 + 버프

         
         
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