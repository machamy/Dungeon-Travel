using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Player
{
    public class Class : ScriptableObject
    {
        public static List<Class> ClassList;

        public Stat defaultStat;
        public Stat growStat;
    }
}