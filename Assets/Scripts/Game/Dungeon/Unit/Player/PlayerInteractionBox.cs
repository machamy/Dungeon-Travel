using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Game.Dungeon.Unit
{
    public class PlayerInteractionBox : MonoBehaviour
    {
        public PlayerUnit pu;
        public HashSet<BaseInteractionUnit> unitSet = new HashSet<BaseInteractionUnit>();

        private void OnTriggerEnter(Collider other)
        {
            BaseInteractionUnit unit;
            if (other.gameObject.TryGetComponent<BaseInteractionUnit>(out unit))
            {
                unitSet.Add(unit);
                Debug.Log($"in {unit}");
            }
        }
        private void OnTriggerExit(Collider other)
        {
            BaseInteractionUnit unit;
            if (other.gameObject.TryGetComponent<BaseInteractionUnit>(out unit))
            {
                unitSet.Remove(unit);
                Debug.Log($"exit {unit}");
            }
        }
    }
}