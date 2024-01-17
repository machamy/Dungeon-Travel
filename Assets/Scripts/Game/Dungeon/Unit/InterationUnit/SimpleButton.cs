using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Scripts.Game.Dungeon.Unit
{
    public class SimpleButton : BaseInteractionUnit
    {
        public UnityEvent ButtonUseEvent;

        public override void Start()
        {
            base.Start();
            type |= InteractionType.Use;
            ButtonUseEvent.AddListener(() => Debug.Log("[SimpleButton::OnUseEvent] Invoked."));
        }


        public override void OnUsed(PlayerUnit unit)
        {
            base.OnUsed(unit);
            ButtonUseEvent.Invoke();
        }
    }
}