using Scripts.Game.Dungeon;
using Scripts.Game.Dungeon.Unit;
using Scripts.Manager;
using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Manager
{
    /// <summary>
    /// HUD 정보 와 같이 던전 씬 전체에 해당되는 정보 관리
    /// </summary>
    public class DungeonManager : MonoBehaviour
    {
        [Header("Prefab")]
        [SerializeField]
        private GameObject dungeonHUDPrefab;
        [FormerlySerializedAs("minimapCamera")] [SerializeField]
        private GameObject minimapCameraPrefab;
        
        [Header("Variables")]
        public DungeonHUD dungeonHUD;
        public Canvas Canvas;
        public PlayerUnit player;

        public int floor;
        public string name;
        
        public void Start()
        {
            dungeonHUD = Instantiate(dungeonHUDPrefab, Canvas.transform).GetComponent<DungeonHUD>();
            var mapCam = Instantiate(minimapCameraPrefab);
            player = FindObjectOfType<PlayerUnit>().GetComponent<PlayerUnit>();

            mapCam.GetComponent<QuraterviewCamera>().target = player.transform;
            InitHUD(dungeonHUD);
            InitPlayer(player);
        }

        public void InitHUD(DungeonHUD dungeonHUD)
        {
            dungeonHUD.nameTMP.text = $"{floor}F {name}";
            dungeonHUD.dayTMP.text = $"D-{UserDataManager.Instance.progress.day}";
        }

        public void InitPlayer(PlayerUnit player)
        {
            
        }
    }
}