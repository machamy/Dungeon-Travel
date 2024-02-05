using System;
using System.Collections;
using UnityEngine;

namespace Scripts.Game.Dungeon.Unit
{
    public class SimplePortal : BaseInteractionUnit
    {
        public SimplePortal Destination;
        public bool isActive;
        
        public override void Start()
        {
            base.Start();
            type |= InteractionType.Intersect;
        }



        public override bool OnIntersect(PlayerUnit unit)
        {
            base.OnIntersect(unit);
            if (!isActive)
                return false;
            
            Destination.isActive = false;
            Vector3 delta = Destination.transform.position - transform.position;
            unit.gameObject.transform.position += delta;
            return true;
        }
        public override bool OnIntersectOut(PlayerUnit unit)
        {
            base.OnIntersectOut(unit);
            StartCoroutine(CooldownCo(0.5f));
            return true;
        }
        
        private IEnumerator CooldownCo(float time)
        {
            float currentCooldown = time;
            while (currentCooldown > 0)
            {
                currentCooldown -= Time.deltaTime;
                yield return null;
            }
            isActive = true;
        }
    }
}