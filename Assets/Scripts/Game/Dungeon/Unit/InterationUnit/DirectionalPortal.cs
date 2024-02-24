using Scripts.Manager;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace Scripts.Game.Dungeon.Unit
{
    /// <summary>
    /// 앞으로 가면 실내로, 반대면 실외로 간다.
    /// </summary>
    /// TODO: 페이드 아웃시 플레이어 못움직이게
    public class DirectionalPortal : BaseInteractionUnit
    {
        [FormerlySerializedAs("InsidePortal")] public DirectionalPortal Destination;
        public bool isActive = true;
        public float fadeTime = 0.0f;
        [Header("이동시 좌표 변형(곱연산)")] public Vector3 positionModifier = new Vector3(1, 1, 1);
        [Header("이동시 좌표 변형(합연산)")] public Vector3 rotaionModifier = new Vector3(0, 0, 0);
        
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
            // Debug.Log(angle);
            if (angle < 90f)
            {
                if (fadeTime > 0)
                {
                    unit.Pause();
                    StartCoroutine(GameManager.Instance.Fade(fadeTime, 0,1, () =>
                    {
                        unit.UnPause();
                        Teleport(unit);
                        StartCoroutine(GameManager.Instance.Fade(fadeTime, 1, 0));
                    }));
                }
                else
                {
                    Teleport(unit);
                }
                
                return true;
            }

            // 이동방향이 포탈 방향 아님 => 순간이동 X
            return false;
        }

        private void Teleport(PlayerUnit unit)
        {
            // 포탈의 방향으로 이동 => 순간이동
            var position = transform.position;
            // Vector3 portalPositionDelta = Destination.transform.position - position;
            Vector3 portalPlayerDelta = unit.gameObject.transform.position - position;
            Vector3 playerRotation = unit.transform.eulerAngles;
            portalPlayerDelta.Scale(positionModifier);
            playerRotation += rotaionModifier;
            
            unit.GetComponent<Rigidbody>().Move(Destination.transform.position + portalPlayerDelta, Quaternion.Euler(playerRotation));
            // unit.GetComponent<Rigidbody>().MovePosition(new Vector3(100,0,100));//, unit.transform.rotation);
            // Debug.Log(delta);
        }
        
        /// <summary>
        /// 쿨타임 후 활성화. 사용안함
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