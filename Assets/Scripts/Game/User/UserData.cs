using Scripts;
using Scripts.DebugConsole;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Entity;
using Scripts.Game;
using Scripts.User;

namespace Scripts.User
{
    /// <summary>
    /// 유저데이터, MonoBehaviour;
    /// 플레이어가 단독으로 가지고 있는 현재 데이터들을 담고 있다.
    /// </summary>
    public class UserData : MonoBehaviour
    {

        public Party party;
        public Inventory inventory;
        public Progress progress;
        public Config config;

        public UserData() { init(); }

        public void init()
        {
            party = Party.CreateInstance();
            inventory = Inventory.CreateInstance(27);
            progress = Progress.CreateInstance();
            config = Config.CreateInstance();
        }

    }
}

