using System.Collections;
using UnityEngine;

namespace Scripts.Game.Dungeon.Unit
{
    public class SimpleTrap : BaseInteractionUnit
    {
        public float CooldownTime;
        private float currentCooldown;
        
        public override bool OnIntersect(PlayerUnit unit)
        {
            if (currentCooldown > 0)
                return false;
            
            Debug.Log($"[SimpleTrap::OnIntersect] {name} activated to {unit.name}");
            StartCoroutine(CooldownCo(CooldownTime));
            return true;
        }

        private IEnumerator CooldownCo(float time)
        {
            currentCooldown = time;
            while (currentCooldown > 0)
            {
                currentCooldown -= Time.deltaTime;
                yield return null;
            }
            currentCooldown = 0;
        }
    }
}