using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using UnityEngine;

namespace Scripts.Entity
{
    public enum ClassType : int
    {
        Null,Paladin, Fighter, Ranger, Cleric, Wizard
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

    [CreateAssetMenu(order = int.MaxValue)]
    public class Class : ScriptableObject
    {
        
        public static List<Class> ClassList;

        public ClassType type;
        public string className;


        
    }
}