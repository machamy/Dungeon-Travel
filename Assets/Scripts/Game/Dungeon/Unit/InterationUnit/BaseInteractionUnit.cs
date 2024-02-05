using System;
using System.Collections;
using System.Collections.Generic;
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
    /// 현재 "공격F"과 "사용E"에 대해서만 적용되어있다.
    /// </remarks>
    public abstract class BaseInteractionUnit : MonoBehaviour
    {
        /// <summary>
        /// 가능한 상호작용의 종류 Flag
        /// </summary>
        public InteractionType type;
        public float hp;

        /// <summary>
        /// 히든일 경우 포커스 될 시 표시하지 않는다
        /// </summary>
        public bool isHidden = false;

        protected bool isFocused = false;
        protected Material outline;


        public bool IsFocused
        {
            get => isFocused;
            set
            {
                isFocused = value;
                OnFocusEvent(value);
            }
        }

        public virtual void Start()
        {
            Shader shader = Shader.Find("Draw/OutlineShader");
            outline = new Material(shader);
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

        private Renderer renderer;
        private List<Material> materialList = new List<Material>();

        public void OnFocusEvent(bool val)
        {
            if (isHidden || this.renderer is null)
                return;
            renderer = GetComponent<Renderer>();

            materialList.Clear();
            materialList.AddRange(this.renderer.sharedMaterials);

            if (val)
                materialList.Add(outline);
            else
                materialList.Remove(outline);

            renderer.materials = materialList.ToArray();
        }

    }

}