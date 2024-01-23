using System;
using Script.Data;
using Script.Global;

namespace Scripts.Data
{
    [Serializable]
    public class SkillData:IDBdata
    {
        public string atttackType;
        public int rank;
        public string skillType;
        public string name;
        public float damage;
        public float mpCost;
    }
}