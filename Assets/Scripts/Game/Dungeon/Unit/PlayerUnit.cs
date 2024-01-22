using System;
using System.Collections;
using System.Collections.Generic;
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


        void Awake()
        {
            rigid = GetComponentInChildren<Rigidbody>();

        }

        private void Start()
        {
            focusUnit = null;
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
            Debug.Log($"[PlayerUnit::OnUse()] Execute to {focusUnit}");
            if (!focusUnit.type.HasFlag(InteractionType.Use))
                return;
            focusUnit.OnUsed(this);
        }

        void OnAttack()
        {
            if (focusUnit is null)
                return;
            Debug.Log($"[PlayerUnit::OnUse()] Execute to {focusUnit}");
            if (!focusUnit.type.HasFlag(InteractionType.Attack))
                return;
            focusUnit.OnAttacked(this, damage: 1.0f);
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