using DG.Tweening;
using RealtimeCSG.Components;
using Scripts.Manager;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace Scripts.Game.Dungeon
{
    public class CutoutObject: MonoBehaviour
    {
        public Camera cam;
        public GameObject target;
        [FormerlySerializedAs("CutoutMask")] public LayerMask CutoutLayer;

        public float time = 2f;
        public float size = 5f;

        private bool IsLarge = false;
        
        private void Update()
        {
            if (cam is null)
            {
                cam = GameManager.Instance.qCamera.GetComponent<Camera>();
                return;
            }

            RaycastHit hit;
            
            if(Physics.Raycast(cam.transform.position,(target.transform.position-cam.transform.position).normalized, out hit, Mathf.Infinity,CutoutLayer))
            {
                if (hit.collider.tag.Contains("CutoutMask"))
                {
                    target.transform.DOScale(0, time);
                }
                else
                {
                    target.transform.DOScale(size, time);
                }
            }
        }
        

        
    }
}