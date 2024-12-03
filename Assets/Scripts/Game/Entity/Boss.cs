using Scripts.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Entity
{
    public class Boss : IEnemy
    {
        public StatData StatData
        {
            get { return StatData; }
            set { StatData = value; }
        }
        private List<SkillData> skillDatas = new List<SkillData>();
        public List<SkillData> SkillDatas
        {
            get { return skillDatas; }
            set { skillDatas = value; }
        }
        private List<Action<SkillData>> skillLists = new List<Action<SkillData>>();
        public List<Action<SkillData>> SkillLists
        {
            get { return skillLists; }
            set { skillLists = value; }
        }
        public int Index {  get; set; } // 스킬의 인덱스
        BattleManager battleManager;
        public GameObject gameObject = null;
        public float CurrentHP { get; set; }
        bool passiveTrigger = false;
        public BuffManager BuffManager { get; set; }
        Enemy_Skill skill;
        public Action<SkillData> Passive { get; set; }
        public void NewEnemy(int floor, string name,GameObject gameObject, BattleManager battleManager)
        {
            this.gameObject = gameObject;
            this.battleManager = battleManager;
            //skill = new Enemy_Skill(this, battleManager);
            StatData = DB.GetEnemyData(floor, name);
            skillDatas = DB.GetEnemySkillData(floor, name);
            //SkillLists = skill.GetSkillList(floor, name);
            CurrentHP = StatData.hp;
            BuffManager = gameObject.GetComponent<BuffManager>();
            Index = -1;
        }

        public void Attack(Action<SkillData> passive)
        {
            if (BuffManager.debuffDic.ContainsKey(DebuffType.Stun)) // 기절이라면 공격 함수 실행 x
                return;
            int[] weightArr = new int[5] { 0, 0, 0, 0, 0 };
            int i = 0;
            foreach (SkillData skilldata in skillDatas)
            {
                if (skilldata.skillWeight == 0) // 가중치가 0이면 보스의 체력에 따라 발동되는 패시브
                    passiveTrigger = true;
                else if (skilldata.skillWeight == -1) // 가중치가 -1이면 전체적으로 적용되는 패시브
                    battleManager.bossPassive = skillLists[i];
                else
                    weightArr[i++] = skilldata.skillWeight; // 가중치가 있다면 스킬
            }
            if (passiveTrigger == true && ((CurrentHP) / (StatData.hp) < 0.5)) // 체력 변수를 따로 두어서 패시브 트리거를 다르게 두어야함
            {
                SkillLists[0].Invoke(skillDatas[0]); // 패시브 발동
                passiveTrigger = false;
            }
            else
            {
                if (Index == -1) // 미리 지정되어있는 스킬이 없을때
                {
                    Index = Utility.WeightedRandom(weightArr);
                    if (BuffManager.debuffDic.ContainsKey(DebuffType.Silence)) // 침묵이라면  skillLists[0]에 저장되어 있는 기본공격만 하도록
                        Index = 1;
                }
                passive?.Invoke(skillDatas[Index]);
                SkillLists[Index].Invoke(skillDatas[Index]);
                Index = -1;
            }
        }
    }
}

