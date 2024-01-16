using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Scripts.Game.Dungeon.InterationUnit
{
    public class TestDoor : BaseInteractionUnit, IDoor
    {
        public float openTime;
        public bool isOpened;
        public MeshRenderer Renderer;
        public GameObject DoorObject;

        private Collider collider;
        void Start()
        {
            isHidden = true;
            collider = DoorObject.GetComponent<Collider>();
            Renderer = DoorObject.GetComponent<MeshRenderer>();
            Color c = Renderer.material.color;
            c.a = 0.5f;
            Renderer.material.color = c;
            
        }

        public void Open()
        {
            if (isOpened)
                return;
            
            StartCoroutine(OpenTimer(1.25f, 3.5f, 2.5f));
        }

        public void Close()
        {
            if (!isOpened)
                return;
            isOpened = false;
            collider.enabled = true;
            
            Color c = Renderer.material.color;
            c.a = 1f;
            Renderer.material.color = c;
        }

        IEnumerator OpenTimer(float openingTime, float time, float closingTime, Action calllback = null)
        {
            Debug.Log("[TestDoor::OpenTimer] opening");
            //문이 열리는 과정
            yield return StartCoroutine(FadeCo(1, 0, openingTime, 16));
            isOpened = true;
            collider.enabled = false;

            Debug.Log("[TestDoor::OpenTimer] opened");
            //열리는 시간
            yield return new WaitForSeconds(time);

            Debug.Log("[TestDoor::OpenTimer] closing");
            //문이 닫히는 과정
            yield return StartCoroutine(FadeCo(0, 1, closingTime, 16));
            Close();
            Debug.Log("[TestDoor::OpenTimer] closed");
            calllback?.Invoke();
        }


        /// <summary>
        /// 페이드 인/아웃을 관리함
        /// </summary>
        /// <param name="start">시작 투명도</param>
        /// <param name="end">종료 투명도</param>
        /// <param name="time">시간</param>
        /// <param name="count">프레임 수</param>
        /// <param name="callback">코루틴 종료시 실행할 함수</param>
        /// <returns></returns>
        IEnumerator FadeCo(float start, float end, float time, int count, Action callback = null)
        {
            var delay = new WaitForSeconds(time / count);
            Color firstColor = Renderer.material.color;
            float delta = (end - start);
            for (int i = 0; i < count; i++)
            {
                Color c = Renderer.material.color;
                c.a = start + i * delta / count;
                Renderer.material.color = c;
                yield return delay;
            }

            Renderer.material.color = firstColor;
            callback?.Invoke();
        }
    }
}