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