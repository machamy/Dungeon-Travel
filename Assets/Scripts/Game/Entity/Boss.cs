using Scripts.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Entity
{
    public class Boss
    {
        private EnemyStatData enemyStatData = new EnemyStatData();
        private List<SkillData> skillDatas = new List<SkillData>();
        private List<Action<SkillData, EnemyStatData>> skillLists = new List<Action<SkillData, EnemyStatData>>();
        float currentHp;
        bool passiveTrigger = false;
        Enemy_Skill skill;
        public Enemy_Base NewBoss(int floor, string name)
        {
            enemyStatData = DB.GetEnemyData(floor, name);
            skillDatas = DB.GetEnemySkillData(floor, name);
            skillLists = skill.GetSkillList(floor, name);
            currentHp = enemyStatData.hp;
            return new Enemy_Base(this);
        }
        public float Agi
        {
            get { return enemyStatData.agi; }
        }


        public float Hp
        {
            get { return enemyStatData.hp; }
        }

        public void Attack()
        {
            int[] weightArr = new int[] { };
            foreach (SkillData skilldata in skillDatas)
            {
                int i = 0;
                weightArr[i++] = skilldata.skillWeight;
                if (skilldata.skillWeight == 0)
                    passiveTrigger = true;
            }
            if (passiveTrigger == true && ((currentHp) / (enemyStatData.hp) < 0.5f))
            {
                skillLists[0].Invoke(skillDatas[0],enemyStatData); // 패시브 발동
                passiveTrigger = false;
            }
            else
            {
                int weight = Utility.WeightedRandom(weightArr);
                skillLists[weight].Invoke(skillDatas[weight], enemyStatData);
            }
        }
    }
}

