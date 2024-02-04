using System;
using System.Collections;
using System.Collections.Generic;
using Scripts.Manager;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Scripts.Game.Dungeon.Unit
{
    public class PlayerUnit : MonoBehaviour
    {
        [SerializeField] protected float speed;
        [SerializeField] protected float interactionRange;
        //[SerializeField] protected QuraterviewCamera Camera;

        [SerializeField] internal PlayerInteractionBox PlayerInteractionBox;
        
        Rigidbody rigid;
        Vector3 moveVec;
        private GameManager gm;
        PlayerInput pInput;
        
        void Awake()
        {
            rigid = GetComponentInChildren<Rigidbody>();
            gm = GameManager.Instance;
            // pInput.GetComponent<PlayerInput>();
            
        }

        private void Start()
        {
            focusUnit = null;
            var pi = GetComponent<PlayerInput>();
            gm.PlayerActionMap = pi.currentActionMap;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            playerMove();

            playerTurn();
        }

        private void LateUpdate()
        {
            CheckInteraction();
        }

        void playerMove()
        {
            if (IsOnSlope())
            {
                var dir = GetSlopeDir(moveVec);
                transform.position += dir * (speed * Time.deltaTime);
                Debug.Log($"{dir.x} , {dir.y} , {dir.z}");
            }
            else
            {
                transform.position += moveVec * (speed * Time.deltaTime);
            }
            
            //rigid.velocity = Vector3.zero; 이거하면 계단에서 안내려옴
        }

        void playerTurn()
        {
            transform.LookAt(transform.position + moveVec);
            rigid.angularVelocity = Vector3.zero;
        }

        // Input System에서 사용. 패드,키보드 등의 입력을 vector2로 받아온다.
        void OnMove(InputValue value)
        {
            Vector2 inputVec = value.Get<Vector2>(); //이미 normalized된 녀석.
            moveVec = GameManager.Instance.qCamera.
                GetWorldDiretion(new Vector3(inputVec.x, 0, inputVec.y));//new Vector3(inputVec.x, 0, inputVec.y);
        }

        void OnUse()
        {
            if (focusUnit is null)
                return;
            if (!focusUnit.type.HasFlag(InteractionType.Use))
                return;
            Debug.Log($"[PlayerUnit::OnUse()] Execute to {focusUnit}");
            focusUnit.OnUsed(this);
        }

        void OnAttack()
        {
            if (focusUnit is null)
                return;
            if (!focusUnit.type.HasFlag(InteractionType.Attack))
                return;
            Debug.Log($"[PlayerUnit::OnUse()] Execute to {focusUnit}");
            focusUnit.OnAttacked(this, damage: 1.0f);
        }

        private RaycastHit slopeHit;
        public float minSlopeAngle;
        public float PlayerHeight;
        private bool IsOnSlope()
        {
            Debug.DrawRay(transform.position, Vector3.down * (PlayerHeight / 2 + 0.3f), Color.red);
            if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, PlayerHeight/2+0.3f))
            {
                float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
                return angle < minSlopeAngle;
            }

            return false;
        }

        private Vector3 GetSlopeDir(Vector3 moveVector){
            return Vector3.ProjectOnPlane(moveVector, slopeHit.normal);
        }

        
        /// <summary>
        /// 사용안함
        /// </summary>
        /// <param name="iu"></param>
        void OnIntersect(BaseInteractionUnit iu)
        {
            if (!focusUnit.type.HasFlag(InteractionType.Intersect))
                return;
            Debug.Log($"[PlayerUnit::OnIntersect(BaseInteractionUnit)] Execute to {iu.name}");
        }

        /// <summary>
        /// 상호작용 가능한 유닛을 focusUnit에 등록
        /// </summary>
        void CheckInteraction()
        {
            if (!PlayerInteractionBox.unitSet.Any())
            {
                FocusUnit = null;
                return;
            }
                
            var iter= PlayerInteractionBox.unitSet.GetEnumerator();
            iter.MoveNext();
            if (PlayerInteractionBox.unitSet.Count == 1)
            {
                FocusUnit = iter.Current;
            }else //if (PlayerInteractionBox.unitSet.Count > 1)
            {
                //상호작용 가능한 오브젝트가 여러개인 경우 가운데에 있는 오브젝트 선택
                var bu = GetInteractionByRay();
                
               if(bu is null)
                   FocusUnit = iter.Current;
               else
               {
                   FocusUnit = bu;
               }
            } 
        }
        BaseInteractionUnit GetInteractionByRay()
        {
            Ray r = new Ray(transform.position, transform.forward);
            RaycastHit[] hiArr = Physics.RaycastAll(r, interactionRange);
            bool found = false;
            
            BaseInteractionUnit minFocus = null;
            float minDistance = 100f;
            // int idx = 0;
            // Debug.Log(string.Join(", ",hiArr.Select((e)=>e.collider.gameObject.name)));
            foreach (var hi in hiArr)
            {
                // Debug.Log($"{hi.collider.gameObject.name} : {idx}");
                //TODO: 벽이나 기타 장애물 확인 코드 필요 (tag 등 이용)
                if (hi.collider.gameObject.TryGetComponent(out BaseInteractionUnit iu))
                {
                    if (minDistance > hi.distance)
                    {
                        minDistance = hi.distance;
                        minFocus = iu;
                        
                    }
                    // Debug.Log($"{iu.name} [{hiArr.Length}] : {hi.distance}");
                }
            }
            //디버그용 그리기
            Debug.DrawRay(r.origin, r.direction * (interactionRange * 1), Color.green);
            if (minDistance != 100f)
            {
                return minFocus;
            }
            return null;
        }


        /// <summary>
        /// 히트박스가 콜라이더에 닿아있을 경우
        /// </summary>
        /// <param name="other"></param>
        internal void OnStay(Collider other)
        {
            BaseInteractionUnit iu;
            if (other.transform.CompareTag("Interaction") && other.transform.TryGetComponent(out iu))
            {
                iu.OnIntersect(this);
            }
        }


        private BaseInteractionUnit focusUnit;

        /// <summary>
        /// 포커스 등록/해제 관리용 프라퍼티
        /// </summary>
        public BaseInteractionUnit FocusUnit
        {
            get => focusUnit;
            set
            {
                // 변경 X
                if (focusUnit == value)
                    return;
                
                if (focusUnit is not null)
                    focusUnit.IsFocused = false;

                focusUnit = value;

                // 포커스 해제시(변경 X)
                if (value is null)
                    return;

                // 포커스 변경시
                value.IsFocused = true;
            }
        }
    }
}