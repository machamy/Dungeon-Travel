using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Scripts.Data;
using UnityEngine;

namespace Scripts.Entity
{
    public enum ClassType : int
    {
        Null, Paladin, Fighter, Ranger, Cleric, Wizard
        //, Rouges
    }

    public static class ClassTypeHelper
    {
        private static Dictionary<ClassType, string> codenames = new Dictionary<ClassType, string>()
        {
            {ClassType.Null, "NLL"},
            {ClassType.Paladin, "PLD"},
            {ClassType.Fighter, "FGT"},
            {ClassType.Cleric, "CLC"},
            {ClassType.Ranger, "RNG"},
            {ClassType.Wizard, "WIZ"}
        };
        public static string GetCodename(this ClassType type)
        {
            string result;
            if(codenames.TryGetValue(type, out result))
                return result;
            return "NLL";
        }
        public static ClassType FromCodename(string codename)
        {
            var result = codenames.FirstOrDefault(e => (e.Value == codename)).Key;
            return result;
        }
    }
    
    public class Class
    {
        /// <summary>
        /// 0은 비어있는 클래스.
        /// </summary>
        public static List<Class> ClassList;

        public ClassType type;
        public string className;
        public string name => className;

        private SkillData[] SkillArray;

        public Class(ClassType type, string name)
        {
            this.type = type;
            this.className = name;
        }

        public SkillData[] GetSkillArr()
        {
            if (SkillArray == null)
            {
                SkillArray = DB.GetSkillDataArr(this.type);
            }

            return SkillArray;
        }

        public static void InitClassList()
        {
            ClassList = new List<Class>();
            ClassList.Add(new Class(ClassType.Null, "Null"));
            ClassList.Add(new Class(ClassType.Paladin, "Paladin"));
            ClassList.Add(new Class(ClassType.Fighter, "Fighter"));
            ClassList.Add(new Class(ClassType.Cleric, "Cleric"));
            ClassList.Add(new Class(ClassType.Ranger, "Ranger"));
            ClassList.Add(new Class(ClassType.Wizard, "Wizard"));
        }
    }
}