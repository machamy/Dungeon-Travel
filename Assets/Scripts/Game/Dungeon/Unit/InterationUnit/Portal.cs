using UnityEngine;

namespace Scripts.Game.Dungeon.Unit
{
    public class Portal : BaseInteractionUnit
    {
        public Transform Destination;
        
        public override void Start()
        {
            base.Start();
            type |= InteractionType.Intersect;
        }

        public override bool OnIntersect(PlayerUnit unit)
        {
            base.OnIntersect(unit);
            unit.gameObject.transform.position = Destination.position;
            return true;
        }
    }
}