using Scripts.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Entity
{
    public class Boss : IEnemy
    {
        public EnemyStatData EnemyStatData
        {
            get { return EnemyStatData; }
            set { EnemyStatData = value; }
        }
        private List<SkillData> skillDatas = new List<SkillData>();
        private List<Action<SkillData>> skillLists = new List<Action<SkillData>>();
        public List<Action<SkillData>> SkillLists
        {
            get { return skillLists; }
            set { skillLists = value; }
        }
        public GameObject gameObject = null;
        float currentHp;
        bool passiveTrigger = false;
        BuffManager buffManager;
        Enemy_Skill skill;
        public void NewEnemy(int floor, string name,GameObject gameObject)
        {
            this.gameObject = gameObject;
            skill = new Enemy_Skill(this);
            EnemyStatData = DB.GetEnemyData(floor, name);
            skillDatas = DB.GetEnemySkillData(floor, name);
            SkillLists = skill.GetSkillList(floor, name);
            currentHp = EnemyStatData.hp;
            buffManager = gameObject.GetComponent<BuffManager>();
        }

        public void Attack()
        {
            if (buffManager.debuffDic.ContainsKey(DebuffType.Stun)) // 기절이라면 공격 함수 실행 x
                return;
            int[] weightArr = new int[5];
            foreach (SkillData skilldata in skillDatas)
            {
                int i = 0;
                weightArr[i++] = skilldata.skillWeight;
                if (skilldata.skillWeight == 0)
                    passiveTrigger = true;
            }
            if (passiveTrigger == true && ((currentHp) / (EnemyStatData.hp) < 0.5f)) // 체력 변수를 따로 두어서 패시브 트리거를 다르게 두어야함
            {
                SkillLists[0].Invoke(skillDatas[0]); // 패시브 발동
                passiveTrigger = false;
            }
            else
            {
                int weight = Utility.WeightedRandom(weightArr);
                if (buffManager.debuffDic.ContainsKey(DebuffType.Silence)) // 침묵이라면  skillLists[0]에 저장되어 있는 기본공격만 하도록
                    weight = 0;
                SkillLists[weight].Invoke(skillDatas[weight]);
            }
        }
    }
}

