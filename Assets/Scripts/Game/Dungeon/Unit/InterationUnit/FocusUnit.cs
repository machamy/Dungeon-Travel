using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Game.Dungeon.Unit
{
    public class FocusUnit : MonoBehaviour
    {
        protected Material outline;
        
        private Renderer _renderer;
        private List<Material> materialList = new List<Material>();
     
        /// <summary>
        /// 히든일 경우 포커스 될 시 표시하지 않는다
        /// </summary>
        public bool isHidden = false;
        
        public virtual void Start()
        {
            init();
        }

        public bool IsFocused
        {
            get => isFocused;
            set
            {
                isFocused = value;
                OnFocusEvent(value);
            }
        }
        
        protected bool isFocused = false;
        
        public virtual void OnFocusEvent(bool val)
        {
            if (isHidden || this._renderer is null)
                return;
            

            // materialList.Clear();
            // materialList.AddRange(this._renderer.sharedMaterials);
            //
            // if (val)
            //     materialList.Add(outline);
            // else
            //     materialList.Remove(outline);
            //
            // _renderer.materials = materialList.ToArray();
        }

        protected void init()
        {
            Shader shader = Shader.Find("Draw/OutlineShader");
            outline = new Material(shader);
            _renderer = GetComponent<Renderer>();
        }
    }
}