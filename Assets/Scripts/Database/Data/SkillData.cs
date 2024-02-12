using System;
using Script.Data;
using Script.Global;
using UnityEngine;

namespace Scripts.Data
{
    [Serializable]
    public class SkillData:ScriptableObject, IDBdata
    {
        public string atttackType;
        public int rank;
        public string skillType;
        public string Skillname;
        public float damage;
        public float mpCost;
    }
}