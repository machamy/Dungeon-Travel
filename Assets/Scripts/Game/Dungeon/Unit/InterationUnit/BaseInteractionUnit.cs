using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Scripts.Game.Dungeon.Unit
{
    /// <summary>
    /// 상호작용 종류 Flag
    /// </summary>
    [Flags]
    public enum InteractionType
    {
        None = 0,
        Use = 1 << 0,
        Attack = 1 << 1,
        Intersect = 1 << 2,
        
        All = 1 << 8 - 1
    }
    
    
    

    /// <summary>
    /// 기본적인 상호작용 유닛.
    /// </summary>
    /// <remarks>
    /// </remarks>
    public abstract class BaseInteractionUnit : FocusUnit
    {
        /// <summary>
        /// 가능한 상호작용의 종류 Flag
        /// </summary>
        public InteractionType type;
        public float hp;

        public bool isCurrentlyInteractable = true;

        /// <summary>
        /// 오브젝트의 중심 지정해줘야함. 아닐경우 기본 트랜스폼
        /// </summary>
        /// <remarks>
        /// UI가 뜨는 기준으로 삼는다.
        /// </remarks>
        public Transform center;

        public override void Start()
        {
            base.Start();
            center = transform;
        }

        public virtual void Update()
        {

        }

        /// <summary>
        /// 사용E되었을때 실행된다
        /// </summary>
        /// <param name="unit">사용한 PlayerUnit 주체</param>
        public virtual void OnUsed(PlayerUnit unit)
        {
            Debug.Log($"[BaseInteractionUnit::OnUsed] {gameObject.name}");
        }

        /// <summary>
        /// 공격F당했을때 실행된다
        /// </summary>
        /// TODO: 공격의 종류도 받아올 가능성?
        /// <param name="unit">사용한 PlayerUnit 주체</param>
        /// <param name="damage">공격 피해량</param>
        public virtual void OnAttacked(PlayerUnit unit, float damage)
        {
            Debug.Log($"[BaseInteractionUnit::OnAttacked] {gameObject.name}");
        }

        /// <summary>
        /// 겹쳤을때 실행된다
        /// </summary>
        /// <param name="unit">PlayerUnit 객체</param>
        /// <returns>상호작용 여부</returns>
        public virtual bool OnIntersect(PlayerUnit unit)
        {
            return false;
        }
        
        /// <summary>
        /// 겹치기에서 나갔을경우 실행된다
        /// </summary>
        /// <param name="unit">PlayerUnit 객체</param>
        /// <returns>상호작용 여부</returns>
        public virtual bool OnIntersectOut(PlayerUnit unit)
        {
            return false;
        }

    }

}