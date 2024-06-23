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
        public GameObject gameObject = null;
        float currentHp;
        bool passiveTrigger = false;
        public bool isDead;
        Enemy_Skill skill = new Enemy_Skill();
        public Enemy_Base NewEnemy(int floor, string name, GameObject gameObject)
        {
            this.gameObject = gameObject;
            enemyStatData = DB.GetEnemyData(floor, name);
            skillDatas = DB.GetEnemySkillData(floor, name);
            skillLists = skill.GetSkillList(floor, name);
            currentHp = enemyStatData.hp;
            isDead = false;
            return new Enemy_Base(this);
        }

        public void Attack()
        {
            BuffManager buffManager = gameObject.GetComponent<BuffManager>();
            //if (buffManager.debuffDic.ContainsKey(DebuffType.Stun)) // 기절이라면 공격 함수 실행 x
               // return;
            int[] weightArr = new int[5];
            foreach (SkillData skilldata in  skillDatas)
            {
                int i = 0;
                weightArr[i++] = skilldata.skillWeight;
            }
            int weight = Utility.WeightedRandom(weightArr);
            if (buffManager.debuffDic.ContainsKey(DebuffType.Silence)) // 침묵이라면  skillLists[0]에 저장되어 있는 기본공격만 하도록
                weight = 0;

            skillLists[weight].Invoke(skillDatas[weight],enemyStatData); // 함수 실행
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