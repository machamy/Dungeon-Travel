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

        void Start()
        {
            isHidden = true;
        }

        public void Open()
        {
            if (isOpened)
                return;
            Renderer = GetComponentInChildren<MeshRenderer>();
            StartCoroutine(OpenTimer(1.25f, 3.5f, 2.5f));
        }

        public void Close()
        {
            if (!isOpened)
                return;
            isOpened = false;
            Color c = Renderer.material.color;
            c.a = 1f;
            Renderer.material.color = c;
        }

        IEnumerator OpenTimer(float openingTime, float time, float closingTime, Action calllback = null)
        {
            //문이 열리는 과정
            yield return FadeCo(1, 0, 3.5f, 16);
            isOpened = true;

            //열리는 시간
            yield return new WaitForSeconds(time);

            //문이 닫히는 과정
            yield return FadeCo(0, 1, 3.5f, 16);
            Close();


            calllback.Invoke();
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