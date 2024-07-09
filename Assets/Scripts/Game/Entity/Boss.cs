using Scripts.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Entity
{
    public class Boss
    {
        public EnemyStatData enemyStatData;
        private List<SkillData> skillDatas = new List<SkillData>();
        private List<Action<SkillData, EnemyStatData>> skillLists = new List<Action<SkillData, EnemyStatData>>();
        public GameObject gameObject = null;
        float currentHp;
        bool passiveTrigger = false;
        Enemy_Skill skill = new Enemy_Skill();
        public void NewBoss(int floor, string name,GameObject gameObject)
        {
            this.gameObject = gameObject;
            enemyStatData = DB.GetEnemyData(floor, name);
            skillDatas = DB.GetEnemySkillData(floor, name);
            skillLists = skill.GetSkillList(floor, name);
            currentHp = enemyStatData.hp;
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
            BuffManager buffManager = gameObject.GetComponent<BuffManager>();
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
            if (passiveTrigger == true && ((currentHp) / (enemyStatData.hp) < 0.5f))
            {
                skillLists[0].Invoke(skillDatas[0],enemyStatData); // 패시브 발동
                passiveTrigger = false;
            }
            else
            {
                int weight = Utility.WeightedRandom(weightArr);
                if (buffManager.debuffDic.ContainsKey(DebuffType.Silence)) // 침묵이라면  skillLists[0]에 저장되어 있는 기본공격만 하도록
                    weight = 0;
                skillLists[weight].Invoke(skillDatas[weight], enemyStatData);
            }
        }
    }
}

