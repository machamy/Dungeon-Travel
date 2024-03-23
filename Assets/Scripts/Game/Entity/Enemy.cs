using Scripts.Data;
using Scripts;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Entity
{
    public class Enemy
    {
        public EnemyStatData enemyStatData;
        private List<SkillData> skillDatas = new List<SkillData>();
        private List<Action<SkillData, EnemyStatData>> skillLists = new List<Action<SkillData, EnemyStatData>>();
        GameObject gameObject = null;
        float currentHp;
        bool passiveTrigger = false;
        public bool isDead = false;
        Enemy_Skill skill = new Enemy_Skill();
        public Enemy_Base NewEnemy(int floor, string name, GameObject gameObject)
        {
            this.gameObject = gameObject;
            enemyStatData = DB.GetEnemyData(floor, name);
            skillDatas = DB.GetEnemySkillData(floor, name);
            skillLists = skill.GetSkillList(floor, name);
            currentHp = enemyStatData.hp;
            return new Enemy_Base(this);
        }

        public void Attack()
        {
            int[] weightArr = new int[5];
            foreach(SkillData skilldata in  skillDatas)
            {
                int i = 0;
                weightArr[i++] = skilldata.skillWeight;
            }
            int weight = Utility.WeightedRandom(weightArr);
            skillLists[weight].Invoke(skillDatas[weight],enemyStatData);
        }

        public void GetDamaged(float damage, AttackType attackType)
        {
            if(isDead) return;

            currentHp -= damage;
            if(currentHp <= 0)
            {
                isDead = true;
                UnityEngine.Object.Destroy(gameObject);
            }
        }
    }
}