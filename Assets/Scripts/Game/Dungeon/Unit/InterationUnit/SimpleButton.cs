﻿using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Scripts.Game.Dungeon.InterationUnit
{
    public class SimpleButton : BaseInteractionUnit
    {
        public UnityEvent OnButtonUseEvent;

        public override void Start()
        {
            base.Start();
            type |= InteractionType.Use;
            OnButtonUseEvent.AddListener(() => Debug.Log("[SimpleButton::OnUseEvent] Invoked."));
        }


        public override void OnUsed(PlayerUnit unit)
        {
            base.OnUsed(unit);
            OnButtonUseEvent.Invoke();
        }
    }
}