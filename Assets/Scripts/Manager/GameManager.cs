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
                    instance.init();
                }

                return instance;
            }
        }

        public void init()
        {
            PartyManager = new PartyManager();
            CommandManager cm = CommandManager.Instance; // commandManager 생성
            DontDestroyOnLoad(gameObject);
            InputActionClass input = new InputActionClass();
            PlayerActionMap = input.DungeonPlayer;
            
           // DB.Instance.UpdateDB();
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
    }
}