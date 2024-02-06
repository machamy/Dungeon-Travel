using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace Scripts.Game.Dungeon.Unit
{
    /// <summary>
    /// 앞으로 가면 실내로, 반대면 실외로 간다.
    /// </summary>
    public class IndoorPortal : BaseInteractionUnit
    {
        [FormerlySerializedAs("Destination")] public IndoorPortal InsidePortal;
        public bool isActive;
        
        public override void Start()
        {
            base.Start();
            type |= InteractionType.Intersect;
        }
        
        public override bool OnIntersect(PlayerUnit unit)
        {
            base.OnIntersect(unit);

            return false;
        }
        
        /// <summary>
        /// 나갈때 한번 발동 되어야함
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        public override bool OnIntersectOut(PlayerUnit unit)
        {
            base.OnIntersectOut(unit);
            if (!isActive)
                return false;
            Debug.Log($"{name} : Activated {unit.name}");
            // InsidePortal.isActive = false;
            //isActive = false;
            float angle = Mathf.Abs(Vector3.Angle(transform.right, unit.MoveVector));
            Debug.Log(angle);
            if (angle < 90f)
            {
                // 포탈의 방향으로 이동 => 순간이동
                Vector3 delta = InsidePortal.transform.position - transform.position;
                unit.gameObject.transform.position += delta;
                Debug.Log(delta);
                return true;
            }

            // 이동방향이 포탈 방향 아님 => 순간이동 X
            return false;
        }
        
        /// <summary>
        /// 쿨타임 후 활성화
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
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