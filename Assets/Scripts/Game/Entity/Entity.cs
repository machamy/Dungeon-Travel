using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Scripts.Entity
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
        public Stat rawBaseStat = new Stat();

        /// <summary>
        /// 아이템/퍼브/2차스탯이 반영된 스탯
        /// </summary>
        public Stat FinalStat
        {
            get
            {
                Stat result = (Stat)rawBaseStat.Clone();
                // result = result + 모든 아이템의 stat 변화
                // result = result + 모든 버프의 stat 변화
                return result;
            }
        }

        /// <summary>
        /// 현재 체력
        /// </summary>
        public float hp;
        /// <summary>
        /// 현재 마나
        /// </summary>
        public float mp;


        private void Start()
        {
            hp = FinalStat.hp;
            mp = FinalStat.mp;
        }
    }
}