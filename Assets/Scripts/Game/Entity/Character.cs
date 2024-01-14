using System;
using System.Text;
using Unity.VisualScripting;

namespace Scripts.Player
{
    [Serializable]
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
                if(!initialed)
                    init();
            }
        }

        private bool initialed = false;

        /// <summary>
        /// 캐릭터 첫 생성시
        /// 기본 스탯, 현재 HP, MP 등의 값을 할당
        /// </summary>
        public void init()
        {
            rawBaseStat = (Stat)_class.defaultStat.Clone();
            currentHP = UpdateMaxHP();
            currentMP = UpdateMaxMP();
        }


        /// <summary>
        /// 레벨업시 호출되는 함수
        /// </summary>
        /// <param name="lvDelta"></param>
        protected void OnLevelUpEvent(int lvDelta)
        {
            // 기존 스탯에 각 클래스의 성장 능력치를 더한다.
            rawBaseStat += _class.growStat;
        }


        public override string ToString()
        {
            return $"[Character] {name} \n" +
                   $"class:{_class.name}\n" +
                   $"lv : {LV}\n";
        }
    }
}