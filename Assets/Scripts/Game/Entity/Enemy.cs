using Scripts.Data;
using Scripts;
using System;
using System.Collections.Generic;

namespace Scripts.Entity
{
    public class Enemy
    {
        public EnemyStatData enemyStatData;
        private List<SkillData> skillDatas = new List<SkillData>();
        private List<Action<SkillData, EnemyStatData>> skillLists = new List<Action<SkillData, EnemyStatData>>();
        float currentHp;
        bool passiveTrigger = false;
        Enemy_Skill skill = new Enemy_Skill();
        public Enemy_Base NewEnemy(int floor, string name)
        {
            enemyStatData = DB.GetEnemyData(floor, name);
            skillDatas = DB.GetEnemySkillData(floor, name);
            skillLists = skill.GetSkillList(floor, name);
            currentHp = enemyStatData.hp;
            return new Enemy_Base(this);
        }

        

        public void Attack()
        {
            int[] weightArr = new int[] { };
            foreach(SkillData skilldata in  skillDatas)
            {
                int i = 0;
                weightArr[i++] = skilldata.skillWeight;
            }
            int weight = Utility.WeightedRandom(weightArr);
            skillLists[weight].Invoke(skillDatas[weight],enemyStatData);
        }
    }
}