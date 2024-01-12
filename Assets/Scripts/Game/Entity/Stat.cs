using System;

namespace Scripts.Player
{
    
    [System.Serializable]
    public class Stat : ICloneable
    {
        public float hp;
        public float mp;
        public float atk;
        public float def;
        public object Clone()
        {
            Stat clone = new Stat();
            clone.hp = hp;
            clone.mp = mp;
            clone.atk = atk;
            clone.def = def;
            return clone;
        }
    }
    
}