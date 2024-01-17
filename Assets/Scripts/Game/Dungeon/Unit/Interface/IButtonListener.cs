using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Scripts.Game.Dungeon.Unit
{
    public interface IButtonListener
    {
        public void Do();
        public void Toggle();
    }
}