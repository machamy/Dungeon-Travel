using Scripts.Entity;
using System;
using System.Linq;
using UnityEngine;

namespace Scripts.Data
{
    /// <summary>
    /// 스탯 계수 계산기
    /// </summary>
    public class SkillCalculateElement
    {
        private string raw;
        private Stat requiredStat;
        private float statWeight;
        private float lvWeight;
        
        /// <summary>
        /// 비워진값( - )인지
        /// </summary>
        public bool isNone { get; private set; }

        /// <summary>
        /// 스탯과 상관없는 피해인지
        /// </summary>
        public bool isRaw { get; private set; }
        private float rawDamage;
        /// <summary>
        /// mp룰 계산하는 식인지 판별
        /// </summary>
        public bool isMp { get; private set; }
        
        public SkillCalculateElement(string raw)
        {
            double rawValue;
            if (Double.TryParse(raw, out rawValue))
            {
                isRaw = true;
                rawDamage = (float) rawValue;
            }
            else
            {
                isRaw = false;
                if (raw == String.Empty || raw == "-")
                {
                    isNone = true;
                    statWeight = 0f;
                    return;
                }

                string parsedString = string.Concat(raw.Where(c => Char.IsLetterOrDigit(c) ||
                                                                          Char.IsWhiteSpace(c) ||
                                                                          c == '*' || c== '.' ||
                                                                          c == '+'));
                string[] statStatementArr = parsedString.Split("*");
                if (!Stat.TryParse(statStatementArr[0], out requiredStat))
                {
                    isMp = true;
                    requiredStat = Stat.HP;
                }

                if (isMp)
                {
                    string[] nums = statStatementArr[0].Split("+");
                    if (!float.TryParse(nums[0], out statWeight))
                    {
                        statWeight = 1.0f;
                    }
                    if (!float.TryParse(nums[1], out lvWeight))
                    {
                        statWeight = 0.0f;
                    }
                    this.raw = $"{statWeight:F3}(+{lvWeight:F3})";
                }
                else
                {
                    Debug.Log(requiredStat);
                    string[] nums = statStatementArr[1].Split("+");
                    if (!float.TryParse(nums[0], out statWeight))
                    {
                        statWeight = 1.0f;
                    }
                    if (!float.TryParse(nums[1], out lvWeight))
                    {
                        statWeight = 0.0f;
                    }
                    this.raw = $"{requiredStat}*{statWeight:F3}(+{lvWeight:F3})";
                }
            }
        }

        public SkillCalculateElement(Stat stat, float statWeight, float lvWeight)
        {
            requiredStat = stat;
            this.statWeight = statWeight;
            this.lvWeight = lvWeight;
            raw = $"{stat}*{statWeight:F3}(+{lvWeight:F3})";
        }

        public float Get(int skillLv, StatData finalStat)
        {
            if (isRaw)
                return rawDamage;
            return finalStat.Get(requiredStat) * statWeight + lvWeight * skillLv;
        }

        public float GetMpCost(SkillData skillData)
        {
            return statWeight + lvWeight * skillData.skillLevel;
        }

        public float GetRaw()
        {
            if(!isRaw)
                Debug.LogError("[SkillCalculateElement::GetRaw()] this is not raw damage");
            return rawDamage;
        }

        public override string ToString()
        {
            return raw;
        }
    }
}