using System;
using System.Collections;
using System.Collections.Generic;
using Scripts.Manager;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Scripts.Game.Dungeon.Unit
{
    public class PlayerUnit : MonoBehaviour
    {
        [SerializeField] protected float speed;
        [SerializeField] protected float interactionRange;
        [SerializeField] protected QuraterviewCamera Camera;

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
            transform.position += moveVec * (speed * Time.deltaTime);
            rigid.velocity = Vector3.zero;
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
            moveVec = Camera.GetWorldDiretion(new Vector3(inputVec.x, 0, inputVec.y));//new Vector3(inputVec.x, 0, inputVec.y);
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
            Ray r = new Ray(transform.position, transform.forward);
            RaycastHit[] hiArr = Physics.RaycastAll(r, interactionRange);
            bool found = false;
            if (hiArr.Length != 0)
            {
                foreach (var hi in hiArr)
                {
                    //TODO: 벽이나 기타 장애물 확인 코드 필요 (tag 등 이용)
                    if (hi.collider.gameObject.TryGetComponent(out BaseInteractionUnit iu))
                    {
                        FocusUnit = iu;
                        found = true;
                        break;
                    }
                }
            }

            if (!found)
                FocusUnit = null;


            //디버그용 그리기
            Debug.DrawRay(r.origin, r.direction * (interactionRange * 1), Color.green);
        }


        private void OnTriggerStay(Collider other)
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
                // 기존과 같으면 무시
                if (focusUnit == value)
                    return;

                // 기존 포커스 없을 시
                if (focusUnit is not null)
                    focusUnit.IsFocused = false;

                focusUnit = value;

                // 포커스 해제시(변경 X)
                if (value is null)
                    return;

                // 포커스 변경시
                focusUnit.IsFocused = true;
            }
        }
    }
}