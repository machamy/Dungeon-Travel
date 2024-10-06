using Scripts.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;

namespace Database.Parser
{
    public class EnemySkillDataParser : IDataParser<Dictionary<string, List<SkillData>>>
    {
        private enum EnemySkillDataType
        {
            적이름 = 0,
            유형 = 1,
            스킬이름 = 2,
            최소물리데미지 = 3,
            최소속성데미지 = 5,
            비율 = 7,
            가중치 = 8,
            LastIdx = 8,
            debuff_start = 33,
        }
        
        public Dictionary<string, List<SkillData>> Parse(DataTable sheet, string[] header, int colNum) {
            Dictionary<string, List<SkillData>> skills = new Dictionary<string, List<SkillData>>();
            for (int i = 1; i < sheet.Rows.Count; i++)
            {
                SkillData skill = ScriptableObject.CreateInstance<SkillData>();
                var row = Array.ConvertAll(sheet.Rows[i].ItemArray,
                    p => (p ?? String.Empty).ToString());
                if (row[(int)EnemySkillDataType.스킬이름] == "")
                    continue;


                skill.enemyName = row[(int)EnemySkillDataType.적이름];
                skill.skillName = skill.name;

                // skill.physicsDamage = Convert.ToSingle(row[(int)EnemySkillDataType.물리데미지] == string.Empty ? "0" : row[(int)EnemySkillDataType.물리데미지]);
                // skill.propertyDamage = Convert.ToSingle(row[(int)EnemySkillDataType.속성데미지] == string.Empty ? "0" : row[(int)EnemySkillDataType.속성데미지]);
                skill.minPhysicsDamage = new SkillCalculateElement(row[(int)EnemySkillDataType.최소물리데미지]);
                skill.minPhysicsDamage = new SkillCalculateElement(row[(int)EnemySkillDataType.최소속성데미지 + 1]);
                skill.minPhysicsDamage = new SkillCalculateElement(row[(int)EnemySkillDataType.최소물리데미지]);
                skill.minPhysicsDamage = new SkillCalculateElement(row[(int)EnemySkillDataType.최소속성데미지 + 1]);
                
                
                skill.skillWeight = Convert.ToInt32(row[(int)EnemySkillDataType.가중치] == string.Empty || row[(int)EnemySkillDataType.가중치] == "-" ? "0" : row[(int)EnemySkillDataType.가중치]);
                string[] buff_debuf =
                    (row[(int)EnemySkillDataType.비율] == string.Empty ? "0(0)" : row[(int)EnemySkillDataType.비율])
                    .Replace("%","") // % 삭제
                    .Replace(")","") // ) 삭제
                    .Split("(");
                
                skill.buffRatio = Convert.ToSingle(buff_debuf[0]);
                skill.debuffRatio = Convert.ToSingle(buff_debuf[1]);
                
                /*
                 * True/False 값 처리
                 */
                
                var booleanArr = row.Skip((int)EnemySkillDataType.LastIdx + 1).Select((a) => a.ToUpper() == "TRUE").ToArray();
                int idx = 0;
                skill.isPassive = booleanArr[idx++];
                skill.isSelf = booleanArr[idx++];


                TargetType Ally = TargetType.None;
                if (booleanArr[idx++])
                    Ally |= TargetType.Single;
                if (booleanArr[idx++])
                    Ally |= TargetType.Front;
                if (booleanArr[idx++])
                    Ally |= TargetType.Back;
                if (booleanArr[idx++])
                    Ally |= TargetType.Area;

                TargetType Enemy = TargetType.None;
                if (booleanArr[idx++])
                    Enemy |= TargetType.Single;
                if (booleanArr[idx++])
                    Enemy |= TargetType.Front;
                if (booleanArr[idx++])
                    Enemy |= TargetType.Back;
                if (booleanArr[idx++])
                    Enemy |= TargetType.Area;

                skill.allyTargetType = Ally;
                skill.enemyTargetType = Enemy;

                skill.isBuff = booleanArr[idx++];
                skill.isDebuff = booleanArr[idx++];
                skill.isHealing = booleanArr[idx++];
                skill.isMelee = booleanArr[idx++];
                skill.isRanged = booleanArr[idx++];

                AttackType attackType = AttackType.None;
                for (; (int)EnemySkillDataType.LastIdx + 1 + idx < (int)EnemySkillDataType.debuff_start; idx++)
                {
                    string rawHeader = header[(int)EnemySkillDataType.LastIdx + 1 + idx];
                    if (rawHeader == String.Empty)
                        continue;
                    AttackType currentType = AttackTypeHelper.GetFromKorean(rawHeader);
                    if (currentType != AttackType.None)
                    {
                        if (booleanArr[idx])
                            attackType |= currentType;
                    }
                    else
                    {
                        Debug.Log($"[DB::ParseEnemySkillData] {header[(int)EnemySkillDataType.LastIdx + 1 + idx]}({(int)EnemySkillDataType.LastIdx + 1 + idx}) 유효하지 않음");
                    }
                }
                skill.attackType = attackType;

                DebuffType debuffType = DebuffType.None;
                for (; (int)EnemySkillDataType.LastIdx + 1 + idx < header.Length; idx++)
                {
                    string rawHeader = header[(int)EnemySkillDataType.LastIdx + 1 + idx];
                    if (rawHeader == String.Empty)
                        continue;
                    DebuffType currentType = DebuffTypeHelper.GetFromKorean(rawHeader);
                    if (currentType != DebuffType.None)
                    {
                        if (booleanArr[idx])
                            debuffType |= currentType;
                    }
                    else
                    {
                        Debug.Log($"[DB::ParseClassSkill] {header[(int)EnemySkillDataType.LastIdx + 1 + idx]}({(int)EnemySkillDataType.LastIdx + 1 + idx}) 유효하지 않음");
                    }
                }
                skill.debuffType = debuffType;

                if (!skills.ContainsKey(skill.enemyName))
                    skills.Add(skill.enemyName, new List<SkillData>());
                skills[skill.enemyName].Add(skill);

                Debug.Log($"Register EnemySkill {skill}");
            }

            return skills;
        }
    }
}