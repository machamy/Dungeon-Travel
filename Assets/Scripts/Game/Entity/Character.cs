using System;
using Unity.VisualScripting;

namespace Scripts.Player
{
    public class Character : Entity
    {
        /// <summary>
        /// 직업
        /// </summary>
        public Class _class;
        /// <summary>
        /// 아이템/버프가 반영되기 전의 스탯
        /// </summary>
        public Stat rawStat;

        public int LV => lv;

        private void Start()
        {
            if (LV == 0)
            {
                lv = 1;
                rawStat = (Stat)_class.defaultStat.Clone();
            }
        }
    }
}