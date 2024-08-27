using Scripts.Game.Dungeon.Unit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 맵 안에 배치된 몬스터들을 관리하는 컨트롤러.
/// [TODO 1] : 맵의 각 층마다 있는 몬스터 심볼을 다 관리해야한다. 각 층에 몬스터가 어떻게 구성되어있는지 따라서 층별로 반영할 수 있어야 한다.
/// [TODO 2] : 각 층의 몬스터들에 대해서 DB와 연결되어야 한다. 게임 다시 로드했을 때 몬스터 심볼 얼마나 남았냐도 관리해야 하기 때문.
/// </summary>
namespace Scripts.Game.Dungeon.Unit
{
    public enum MonsterFloor
    {
        Map_1F = 0,
        Map_2F,
        Map_3F,
        Map_4F,
        Map_5F,
        Map_6F,
    }

    public enum MonsterGroupNum
    {
        Group1 = 0,
        Group2,
        Group3,
        Group4,
    }

    public class AIController : MonoBehaviour
    {
        #region 싱글톤
        private static AIController instance = null;
        public static AIController Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject ac = GameObject.Find("AIController");
                    if (ac == null)
                    {
                        ac = new GameObject("AIController");
                    }

                    instance = ac.AddComponent<AIController>();
                }
                return instance;
            }
        }
        #endregion

        private List<AIBaseEntity> entities;

        public static bool IsGameStop { set; get; } = false;

        private void Awake()
        {
            entities = new List<AIBaseEntity>();
            entities.AddRange(FindObjectsOfType<AIBaseEntity>());

            foreach(AIBaseEntity primeEntity in entities)
            {
                MonsterUnit entity = primeEntity.GetComponent<MonsterUnit>();

                entity.Setup();
            }
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (IsGameStop == true) return;

            for (int i = 0; i < entities.Count; i++)
            {
                entities[i].Updated();
            }
        }

    }

}
