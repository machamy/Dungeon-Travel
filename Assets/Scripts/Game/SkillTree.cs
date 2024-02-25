using Scripts.Data;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Game
{
    public class SkillTree
    {
        public int points;


        private Dictionary<SkillData, bool> _skillLearnDictionary;

        public void Unlock(SkillData skill)
        {
            if(_skillLearnDictionary.ContainsKey(skill))
                _skillLearnDictionary[skill] = true;
            else
            {
                Debug.LogError($"{skill.name} 이 없습니다.");
            }
        }
    }
}