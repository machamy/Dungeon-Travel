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
        [FormerlySerializedAs("target")] public GameObject maskObject;
        public GameObject maskCollider;
        [FormerlySerializedAs("CutoutMask")] public LayerMask CutoutLayer;

        public float time = 2f;
        public float size = 5f;

        private bool IsLarge = false;

        private void Start()
        {
            maskCollider.transform.localScale = Vector3.one * size;
        }

        private void Update()
        {
            if (cam is null)
            {
                cam = GameManager.Instance.qCamera.GetComponent<Camera>();
                return;
            }

            RaycastHit hit;
            
            if(Physics.Raycast(cam.transform.position,(maskObject.transform.position-cam.transform.position).normalized, out hit, Mathf.Infinity,CutoutLayer))
            {
                if (hit.collider.tag.Contains("CutoutMask"))
                {
                    maskObject.transform.DOScale(0, time);
                    // maskObject.GetComponent<MeshRenderer>().material.DOFade()
                }
                else
                {
                    maskObject.transform.DOScale(size, time);
                }
            }
        }
        

        
    }
}