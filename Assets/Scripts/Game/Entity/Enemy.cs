using Scripts.Data;
using Scripts;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Entity
{
    public class Enemy : IEnemy
    {
        private EnemyStatData enemyStatData;
        public EnemyStatData EnemyStatData 
        {   
            get { return enemyStatData; } 
            set { enemyStatData = value; } 
        }
        private List<SkillData> skillDatas = new List<SkillData>();
        private List<Action<SkillData>> skillLists = new List<Action<SkillData>>();
        public List<Action<SkillData>> SkillLists
        {
            get { return skillLists; }
            set { skillLists = value; }
        }
        public int Index { get; set; } // 스킬의 인덱스
        public Action<SkillData> Passive { get; set; }

        public GameObject gameObject = null;
        public BuffManager BuffManager { get; set; }
        public int CurrentHP { get; set; }
        bool passiveTrigger = false;
        Enemy_Skill skill;
        public void NewEnemy(int floor, string name, GameObject gameObject, BattleManager battleManager)
        {
            this.gameObject = gameObject;
            skill = new Enemy_Skill(this, battleManager);
            EnemyStatData = DB.GetEnemyData(floor, name);
            skillDatas = DB.GetEnemySkillData(floor, name);
            SkillLists = skill.GetSkillList(floor, name);
            BuffManager = gameObject.GetComponent<BuffManager>();
            CurrentHP = enemyStatData.hp;
            Index = -1;
        }

        public void Attack(Action<SkillData> passive)
        {
            if (BuffManager.debuffDic.ContainsKey(DebuffType.Stun)) // 기절이라면 공격 함수 실행 x
                return;
            int[] weightArr = new int[5];
            int i = 0;
            foreach (SkillData skilldata in  skillDatas)
            {
                weightArr[i++] = skilldata.skillWeight;
            }
            if (Index == -1) // 미리 지정되있는 스킬이 없을때
            {
                Index = Utility.WeightedRandom(weightArr);
                if (BuffManager.debuffDic.ContainsKey(DebuffType.Silence)) // 침묵이라면  skillLists[0]에 저장되어 있는 기본공격만 하도록
                    Index = 0;
            }
            passive.Invoke(skillDatas[Index]);
            SkillLists[Index].Invoke(skillDatas[Index]);
            Index = -1;
        }
    }
}