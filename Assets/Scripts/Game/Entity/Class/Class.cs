using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Scripts.Player
{

    public class Class : ScriptableObject
    {
        public static List<Class> ClassList;

        public int classId;
        public string className;
        
        public Stat defaultStat;
        public Stat growStat;
        
    }
}