using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Scripts.Game.Dungeon.Unit
{
    public class BreakableUnit : BaseInteractionUnit
    {
        public UnityEvent BreakEvent;

        public override void Start()
        {
            base.Start();
        }

        /// <summary>
        /// 공격받을시 실행되는 메소드
        /// </summary>
        /// <param name="pu"></param>
        /// <param name="damage"></param>
        public override void OnAttacked(PlayerUnit pu, float damage)
        {
            hp -= damage;
            if (hp < 0)
            {
                hp = 0;
                OnBreak(pu);
            }
        }

        public void OnBreak(PlayerUnit pu)
        {
            Debug.Log($"[BreakableUnit::OnBreak] {name} Broken!");
            BreakEvent.Invoke();
            gameObject.SetActive(false);
        }
    }
}