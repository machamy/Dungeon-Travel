using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Scripts.Game.Dungeon.Unit
{
    public abstract class AIBaseEntity : MonoBehaviour
    {
        private static int nextEntityID = 0;

        private int id;
        public int ID
        {
            set
            {
                id = value;
                nextEntityID++;
            }
            get => id;
        }

        private string EntityName;
        // 그 외의 프리팹, material...

        public virtual void Setup(string name)
        {
            ID = nextEntityID;
            EntityName = name;
        }

        public void IDReset()
        {
            
        }

        public abstract void Updated();


    }

}
