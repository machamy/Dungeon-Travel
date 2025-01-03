using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.UserData;

namespace Scripts.Manager
{
    /// <summary>
    /// 유저데이터, MonoBehaviour;
    /// 플레이어가 단독으로 가지고 있는 현재 데이터들을 담고 있다.
    /// </summary>
    public class UserDataManager : MonoBehaviour
    {
        public const string NAME = "@UserData";

        static private UserDataManager instance;

        static public UserDataManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new UserDataManager();
                    instance.init();
                }

                return instance;
            }
        }

        public Party party;
        public Inventory inventory;
        public Progress progress;
        public Config config;

        private void Awake()
        {
            if (instance != null)
                Destroy(this);
            else
            {
                init();
            }
        }

        public void init()
        {
            DB.Instance.UpdateDB();

            party = Party.CreateInstance();
            inventory = Inventory.CreateInstance(27);
            progress = Progress.CreateInstance();
            config = Config.CreateInstance();

            DontDestroyOnLoad(gameObject);
        }
    }
}
