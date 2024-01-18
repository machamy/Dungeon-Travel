using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Scripts.Entity
{
    public enum ClassType : int
    {
        Paladin, Fighter, Ranger, Wizard, Cleric
        //, Rouge
    }

    [CreateAssetMenu(order = int.MaxValue)]
    public class Class : ScriptableObject
    {
        public static List<Class> ClassList;

        public ClassType type;
        public string className;
        
        public Stat defaultStat;
        public Stat growStat;
        
    }
}