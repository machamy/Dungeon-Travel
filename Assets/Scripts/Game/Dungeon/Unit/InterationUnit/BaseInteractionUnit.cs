using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Game.Dungeon.Unit
{
    [Flags]
    public enum InteractionType
    {
        None = 0,
        Use = 1 << 0,
        Attack = 1 << 1,
        All = 1 << 8 - 1
    }

    public class BaseInteractionUnit : MonoBehaviour
    {
        public InteractionType type;
        public float hp;

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

        public virtual void OnUsed(PlayerUnit unit)
        {
            Debug.Log($"[BaseInteractionUnit::OnUsed] {gameObject.name}");
        }

        public virtual void OnAttacked(PlayerUnit unit, float damage)
        {

        }

        private Renderer renderer;
        private List<Material> materialList = new List<Material>();

        public void OnFocusEvent(bool val)
        {
            if (isHidden)
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