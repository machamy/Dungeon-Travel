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


        /// <summary>
        /// 레벨업시 호출되는 함수
        /// </summary>
        /// <param name="lvDelta"></param>
        protected void OnLevelUpEvent(int lvDelta)
        {
            // 기존 스탯에 각 클래스의 성장 능력치를 더한다.
            rawStat += _class.growStat;
        }
    }
}