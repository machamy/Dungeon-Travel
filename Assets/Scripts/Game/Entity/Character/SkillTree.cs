using Scripts.Data;
using Scripts.Entity;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Scripts.Game
{
    /// <summary>
    /// 스킬 트리 클래스. 이름 변경 가능
    /// </summary>
    /// <remarks>
    /// 
    /// </remarks>
    public class SkillTree
    {
        
        
        public int points;
        public int rank;

        public SkillTree(int rank = 1, int points = 0)
        {
            this.rank = rank;
            this.points = points;
            _skillLearnDictionary = new Dictionary<SkillData, bool>();
        }

        /// <summary>
        /// 클래스를 변경하고 모든 포인트를 돌려받는다.
        /// </summary>
        /// <param name="class"></param>
        public void SetClass(Class @class)
        {
            foreach (var skill in _skillLearnDictionary)
            {
                if (skill.Value)
                {
                    // 모든 포인트를 돌려받는다.
                    points += skill.Key.pointCost * skill.Key.skillLevel;
                    skill.Key.skillLevel = 0;
                }
            }
            _skillLearnDictionary.Clear();
            
            foreach (SkillData skillData in @class.GetSkillArr())
            {
                if(skillData == null)
                    continue;
                _skillLearnDictionary.Add(skillData,false);
            }
        }

        // 일단 딕셔너리로, TODO: 더 바뀔여지가 있다면 Node 클래스 작성
        private Dictionary<SkillData, bool> _skillLearnDictionary;

        /// <summary>
        /// 해당 스킬 데이터를 언락한다. 실패시 false
        /// </summary>
        /// <param name="skill">스킬</param>
        /// <returns>성공여부</returns>
        public bool Unlock(SkillData skill)
        {
            if(!_skillLearnDictionary.ContainsKey(skill))
            {
                Debug.LogError($"{skill.name} 이 없습니다.");
                return false;
            }

            if (skill.pointCost > points)
            {
                return false;
            }

            if (skill.rank > rank)
            {
                return false;
            }
            
            _skillLearnDictionary[skill] = true;
            return true;
        }

        public bool ForceUnlock(SkillData skill)
        {
            if(!_skillLearnDictionary.ContainsKey(skill))
            {
                Debug.LogError($"{skill.name} 이 없습니다.");
                return false;
            }
            
            _skillLearnDictionary[skill] = true;
            return true;
        }
        
        /// <summary>
        /// 스킬을 잠그고 포인트를 돌려받는다
        /// </summary>
        /// <param name="skill"></param>
        /// <returns></returns>
        public bool Lock(SkillData skill)
        {
            if(!_skillLearnDictionary.ContainsKey(skill))
            {
                Debug.LogError($"{skill.name} 이 없습니다.");
                return false;
            }
            _skillLearnDictionary[skill] = true;
            points += skill.pointCost * skill.skillLevel;
            skill.skillLevel = 0;
            return true;
        }

        /// <summary>
        /// 해당스킬을 배웠는지 여부
        /// </summary>
        /// <param name="skill"></param>
        /// <returns>스킬의 배움 여부 / 없는 스킬일경우에도 false</returns>
        public bool IsLearned(SkillData skill)
        {
            if(!_skillLearnDictionary.ContainsKey(skill))
            {
                Debug.LogError($"{skill.name} 이 없습니다.");
                return false;
            }

            return _skillLearnDictionary[skill];
        }

        /// <summary>
        /// 모든 스킬을 가져온다
        /// </summary>
        /// <returns>모든 SkillData의 List</returns>
        public List<SkillData> GetSkills()
        {
            return _skillLearnDictionary.Keys.ToList();
        }

        /// <summary>
        /// 해당 랭크의 모든 스킬을 가져온다
        /// </summary>
        /// <param name="rank">가져올 랭크</param>
        /// <returns>해당 랭크의 SkillData List</returns>
        public List<SkillData> GetSkillsByRank(int rank)
        {
            return _skillLearnDictionary.Keys.Where(s => s.rank == rank).ToList();
        }
    }
}