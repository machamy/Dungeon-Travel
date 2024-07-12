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
        
        public SkillCalculateElement(string raw)
        {

            string parsedString =  string.Concat(raw.Where(c => Char.IsDigit(c) ||
                                                                      Char.IsWhiteSpace(c) ||
                                                                      c == '*' ||
                                                                      c == '+'));
            string[] statStatementArr = parsedString.Split("*");
            if (!Stat.TryParse(statStatementArr[0], out requiredStat))
            {
                Debug.LogWarning($"{statStatementArr[0]} 파싱 실패.");
                requiredStat = Stat.HP;
            }

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

        public SkillCalculateElement(Stat stat, float statWeight, float lvWeight)
        {
            requiredStat = stat;
            this.statWeight = statWeight;
            this.lvWeight = lvWeight;
            raw = $"{stat}*{statWeight:F3}(+{lvWeight:F3})";
        }

        public float Get(int skillLv, StatData finalStat)
        {
            return finalStat.Get(requiredStat) * statWeight + lvWeight * skillLv;
        }

        public override string ToString()
        {
            return raw;
        }
    }
}