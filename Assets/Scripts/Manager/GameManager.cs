using System;
using System.Collections;
using System.Collections.Generic;
using Scripts.DebugConsole;
using Scripts.Game.Dungeon.Unit;
using Scripts.Manager;

using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Scripts.Manager
{
    /// <summary>
    /// 게임매니저, 싱글턴, MonoBehavior
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        public const string NAME = "@Game";
        private static GameManager instance;

        public QuraterviewCamera qCamera;

        public InputActionMap PlayerActionMap;
        public InputActionMap UIActionMap;


        
        
        public static GameManager Instance
        {
            get
            {
                // 없을경우 생성
                if (instance == null)
                {
                    GameObject root = GameObject.Find(NAME);
                    if (root == null)
                    {
                        root = new GameObject { name = NAME };
                    }

                    instance = root.AddComponent<GameManager>();
                }

                return instance;
            }
        }

        public void init()
        {
            PartyManager = new PartyManager();
            PartyManager.RegisterTestParty();
            
            CommandManager cm = CommandManager.Instance; // commandManager 생성
            DontDestroyOnLoad(gameObject);
            InputActionClass input = new InputActionClass();
            PlayerActionMap = input.DungeonPlayer;

            SceneManager.sceneUnloaded += SceneUnloaded;
            
            DB.Instance.UpdateDB();
        }

        /// <summary>
        /// 씬 사이에 저장되지 않는 변수를 초기화한다.
        /// </summary>
        /// <param name="scene"></param>
        public void SceneUnloaded(Scene scene)
        {
            fadeImage = null;
        }

        public PartyManager PartyManager;

        private void Awake()
        {
            if (instance != null)
                Destroy(this);
            else
            {
                init();
            }
        }



        public void LoadScene(string name)
        {
            SceneManager.LoadScene(name);
        }
        
        public static IEnumerator DoNextFrame(Action a)
        {
            yield return null;
            a.Invoke();
        }


        private Image fadeImage;

        public Image FadeImage
        {
            get
            {
                if (!fadeImage)
                {
                    GameObject imageGO = new GameObject("Fade");
                    imageGO.transform.parent = FindObjectOfType<Canvas>().transform;
                    fadeImage = imageGO.AddComponent<Image>();

                    imageGO.layer = 5; // UI로
                    
                    RectTransform rectTransform = fadeImage.GetComponent<RectTransform>();
                    rectTransform.offsetMin = new Vector2(-20, -20);
                    rectTransform.offsetMax = new Vector2(20, 20);
                    rectTransform.anchorMin = new Vector2(0, 0);
                    rectTransform.anchorMax = new Vector2(1, 1);

                    fadeImage.color = new Color(0,0,0,0);
                    fadeImage.raycastTarget = false;
                }
                return fadeImage;
            }
        }
        
        /// <summary>
        /// 페이드인/아웃을 해주는 코루틴
        /// </summary>
        /// <param name="time">시간</param>
        /// <param name="start">시작 투명도 0.0 ~ 1.0</param>
        /// <param name="end">종료 투명도 0.0 ~ 1.0</param>
        /// <param name="callback">코루틴 종료시 실행되는 콜백. 생략 가능</param>
        /// <returns></returns>
        public IEnumerator Fade(float time, float start, float end, Action callback = null)
        {
            float currentTime = 0;
            Color c;
            while (currentTime < time)
            {
                currentTime += Time.deltaTime;
                c = FadeImage.color;
                c.a = Mathf.Lerp(start,end,currentTime/time);
                FadeImage.color = c;
                yield return null;
            }
            c = FadeImage.color;
            c.a = end;
            FadeImage.color = c;
            
            callback?.Invoke();
        }
    }
}