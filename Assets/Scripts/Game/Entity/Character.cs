using Game.Entity.Character;
using Scripts.Data;
using Scripts.Game;
using System;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;

namespace Scripts.Entity
{
    /// <summary>
    /// 플레이어의 캐릭터  
    /// </summary>
    /// <remarks>
    /// Enemy와 분리할 필요가 있나 싶음
    /// </remarks>
    [Serializable]
    public class Character : Entity
    {
        /// <summary>
        /// 직업
        /// </summary>
        public Class _class;
        
        public int LV => lv;

        public EquipmentInventory EquipmentInventory { get; private set; }        // TODO : EquimentInventory 클래스 별도 생성
        
        public SkillTree SkillTree { get; private set; }

        /// <summary>
        /// 캐릭터 첫 생성시
        /// 기본 스탯, 현재 HP, MP 등의 값을 할당
        /// </summary>
        public Character(string name, int lv = 0)
        {
            this.name = name;
            this.lv = lv;
            if (lv == 0)
            {
                this.lv = 1;
            }
            //Inventory = Inventory.CreateInstance(27);            => Moved to PartyManager
            EquipmentInventory = EquipmentInventory.CreateInstance();
            SkillTree = new SkillTree();
        }

        public Character SetClass(Class @class)
        {
            this._class = @class;

            StatData statData = DB.GetStatData(_class.type,LV);
            rawBaseStat = statData;
            hp = rawBaseStat.hp;
            mp = rawBaseStat.mp;
            agi = rawBaseStat.agi; //임시방편으로 추가해놓음
            isFriendly = true;     //임시방편
            isDead = false;        //임시

            //SkillTree 변경
            SkillTree.SetClass(_class);
            
            return this;
        }
        

        /// <summary>
        /// 레벨업시 호출되는 함수
        /// </summary>
        /// <remarks>
        /// 레벨간 hp차이만큼 더한다.
        /// TODO: 장비에 %증가가 있다면 반영해야함
        /// </remarks>
        public void LevelUp()
        {
            if(lv >= 50)
            {
                Debug.LogError("최대레벨 도달");
                return;
            } 
            SetLevel(lv+1);
            OnLevelUpEvent(lv+1);
        }

        /// <summary>
        /// 엔티티의 레벨을 주어진 레벨로 설정한다.
        /// </summary>
        /// <param name="level"></param>
        public void SetLevel(int level)
        {
            if (level>50 )            {
                Debug.LogError("최대레벨 도달");
                return;
            }
            lv++;

            StatData statData = DB.GetStatData(_class.type, LV);
            hp += statData.hp - rawBaseStat.hp;
            rawBaseStat = statData;
        }

        private bool initialized = false;


        /// <summary>
        /// 레벨업시 호출되는 함수
        /// </summary>
        /// <param name="newLevel"></param>
        protected void OnLevelUpEvent(int newLevel)
        {
            // 기존 스탯에 각 클래스의 성장 능력치를 더한다.
            // rawBaseStat += _class.growStat;
        }


        public override string ToString()
        {
            return $"[Character] {name} \n" +
                   $"class:{_class.name}\n" +
                   $"lv : {LV}\n";
        }
    }
}