using Scripts;
using Scripts.DebugConsole;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Entity;
using Scripts.Game;
using Scripts.User;

namespace Scripts.Manager
{
    /// <summary>
    /// 데이터매니저, 싱글턴, MonoBehaviour;
    /// 플레이어가 단독으로 가지고 있는 현재 데이터들을 담고 있다.
    /// </summary>
    public class DataManager : MonoBehaviour
    {
        public const string NAME = "@Data";

        private static DataManager instance = null;


        public Party party;
        public Inventory inventory;
        public Progress progress;
        public Config config;


        public static DataManager Instance
        {
            get
            {
                // 없을경우 생성
                if (instance is null)
                {
                    GameObject root = GameObject.Find(NAME);
                    if (root is null)
                    {
                        root = new GameObject { name = NAME };
                    }
                    instance = root.AddComponent<DataManager>();
                    instance.init();
                }

                return instance;
            }
        }

        public void init()
        {
            DontDestroyOnLoad(gameObject);

            party = Party.CreateInstance();
            inventory = Inventory.CreateInstance(27);
            progress = Progress.CreateInstance();
            config = Config.CreateInstance();
        }

    }
}

