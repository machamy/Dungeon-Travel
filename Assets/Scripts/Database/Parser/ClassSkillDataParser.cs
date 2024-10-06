using Scripts.Data;
using Scripts.Entity;
using System;
using System.Data;
using System.Linq;
using UnityEngine;

namespace Database.Parser
{
    public class ClassSkillDataParser : IDataParser<SkillData[]>
    {    
        private enum SkillDataType : int
        {
            무기유형 = 0,
            랭크 = 1,
            유형 = 2,
            이름 = 3,

            최소_물리_데미지 = 6,

            최소_속성_데미지 = 8,
            버프디버프비율 = 10,
            MP소모 = 11,
  
            LastIdx = 11,
        
            debuff_start = 36
        }
        
        private ClassType classType;
        
        public ClassSkillDataParser SetClassType(ClassType classType)
        {
            this.classType = classType;
            return this;
        }
        
        public SkillData[] Parse(DataTable sheet, string[] header, int colNum)
        {
             SkillData[] skills = new SkillData[sheet.Rows.Count + 1];
        for (int i = 1; i < sheet.Rows.Count; i++)
        {
            SkillData skill = ScriptableObject.CreateInstance<SkillData>();
            var row = Array.ConvertAll(sheet.Rows[i].ItemArray,
                p => (p ?? String.Empty).ToString());
            if (row[(int)SkillDataType.이름] == "")
                continue;

            skill.classType = classType;

            skill.weaponType = row[(int)SkillDataType.무기유형];

            skill.rank = Convert.ToInt32(row[(int)SkillDataType.랭크]);
            skill.rawType = row[(int)SkillDataType.유형];
            skill.name = row[(int)SkillDataType.이름];
            skill.skillName = skill.name;

            
            // skill.physicsDamage = Convert.ToSingle(row[(int)SkillDataType.최소_물리_데미지] == string.Empty ? "0" : row[(int)SkillDataType.최소_물리_데미지]);
            // skill.propertyDamage = Convert.ToSingle(row[(int)SkillDataType.최소_속성_데미지] == string.Empty ? "0" : row[(int)SkillDataType.최소_속성_데미지]);
            skill.minPhysicsDamage = new SkillCalculateElement(row[(int)SkillDataType.최소_물리_데미지]);
            skill.minPhysicsDamage = new SkillCalculateElement(row[(int)SkillDataType.최소_물리_데미지 + 1]);
            skill.minPhysicsDamage = new SkillCalculateElement(row[(int)SkillDataType.최소_속성_데미지]);
            skill.minPhysicsDamage = new SkillCalculateElement(row[(int)SkillDataType.최소_속성_데미지 + 1]);
            
            string[] buff_debuf =
                (row[(int)SkillDataType.버프디버프비율] == string.Empty ||  row[(int)SkillDataType.버프디버프비율] == "-" ? "0(0)" : row[(int)SkillDataType.버프디버프비율])
                .Replace("%","") // % 삭제
                .Replace(")","") // ) 삭제
                .Split("(");
            skill.buffRatio = Convert.ToSingle(buff_debuf[0]);
            skill.debuffRatio = Convert.ToSingle(buff_debuf[1]);

            skill.mpCost = new SkillCalculateElement(row[(int)SkillDataType.MP소모]);


            /*
             *  True/False 값 처리
             */
            var booleanArr = row.Skip((int)SkillDataType.LastIdx + 1).Select((a) => a.ToUpper() == "TRUE").ToArray();
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
            for (; (int)SkillDataType.LastIdx + 1 + idx < (int)SkillDataType.debuff_start; idx++)
            {
                string rawHeader = header[(int)SkillDataType.LastIdx + 1 + idx];
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
                    Debug.Log($"[DB::ParseClassSkill] {header[(int)SkillDataType.LastIdx+ 1 +idx]}({(int)SkillDataType.LastIdx+ 1 +idx}) 유효하지 않음");
                }
            }
            skill.attackType = attackType;

            DebuffType debuffType = DebuffType.None;
            for (; (int)SkillDataType.LastIdx + 1 + idx < header.Length - 1; idx++)
            {
                string rawHeader = header[(int)SkillDataType.LastIdx + 1 + idx];
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
                    Debug.Log($"[DB::ParseClassSkill] {header[(int)SkillDataType.LastIdx + 1 + idx]}({(int)SkillDataType.LastIdx + 1 + idx}) 유효하지 않음");
                }
            }
            skill.debuffType = debuffType;
            Debug.Log($"Register Skill {skill}");
        }

        return skills;
        }
    }
}