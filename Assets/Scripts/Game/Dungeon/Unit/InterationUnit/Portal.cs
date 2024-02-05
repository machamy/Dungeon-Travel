using UnityEngine;

namespace Scripts.Game.Dungeon.Unit
{
    public class Portal : BaseInteractionUnit
    {
        public Portal Destination;
        
        public override void Start()
        {
            base.Start();
            type |= InteractionType.Intersect;
        }

        public override bool OnIntersect(PlayerUnit unit)
        {
            base.OnIntersect(unit);
            Vector3 delta = transform.position - Destination.transform.position;
            unit.gameObject.transform.position += delta;
            return true;
        }
    }
}