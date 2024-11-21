using Script.Global;
using Scripts.Data;
using Scripts.Entity;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UnitSpawn : MonoBehaviour
{
    public BattleManager battleManager;
    public GameObject playerPrefab;
    public Material spriteOutline;

    public TempPlayerData[] tempPlayerData = new TempPlayerData[6];

    public GameObject[] playerStation = new GameObject[6];
    public GameObject[] enemyStation = new GameObject[6];
    public StationController[] playerStationController = new StationController[6];
    public StationController[] enemyStationController = new StationController[6];


    public HUDmanager[] playerHUD = new HUDmanager[6];
    public HUDmanager[] enemyHUD = new HUDmanager[6];



    Unit[] playerUnit = new Unit[6];
    Unit[] enemyUnit = new Unit[6];

    GameObject[] player = new GameObject[6];

    public int playerCount { get { return BattleManager.alivePlayer; } set { BattleManager.alivePlayer = value; } }
    public int enemyCount { get { return BattleManager.aliveEnemy; } set { BattleManager.aliveEnemy = value; } }

    public void Awake()
    {
        playerCount = 0;
        enemyCount = 0;
    }

    /// <summary>
    /// 플레이어 유닛을 스폰하는 함수
    /// </summary>
    [ContextMenu("Spawn Unit")]
    public void SpawnPlayerUnit()
    {
        for(int i = 0; i< tempPlayerData.Length; i++)
        {
            if (tempPlayerData[i] != null && tempPlayerData[i].position != 6)
            {
                player[i] = Instantiate(playerPrefab, playerStation[tempPlayerData[i].position].transform);
                playerUnit[i] = player[i].GetComponent<Unit>();
                playerUnit[i].tempPlayerData = tempPlayerData[i];
                //playerUnit[playerCount].stat = ScriptableObject.CreateInstance<StatData>();
                //playerUnit[playerCount].stat.hp = 1;
                playerUnit[i].InitialSetting(playerHUD[i], playerStationController[i]);
                playerHUD[i].SetupHUD(playerUnit[i]);
                playerStationController[i].SetUp();

                playerCount++;
            }
        }
    }

    /// <summary>
    /// 적을 스폰하는 함수
    /// </summary>
    /// <param name="floor"></param>
    /// <param name="name"></param>
    /// <param name="boss"> 만약 보스 객체라면 3번째 인자로 true </param>
    public void SpawnEnemyUnit(int floor, string name, bool boss = false) // 적 스폰하는 함수 프리펩으로 받아와서 생성
    {
        //게임오브젝트 생성 및 컴포넌트 추가
        GameObject cloneEnemy = new GameObject($"{name}({enemyCount})");
        SpriteRenderer image = cloneEnemy.AddComponent<SpriteRenderer>();
        cloneEnemy.AddComponent<BuffManager>();
        enemyUnit[enemyCount] = cloneEnemy.AddComponent<Unit>();
        enemyUnit[enemyCount].stat = ScriptableObject.CreateInstance<StatData>();

        // 게임 오브젝트의 위치 설정 (RectTransform을 고려)
        RectTransform enemyStationRect = enemyStation[enemyCount].GetComponent<RectTransform>();
        RectTransform cloneEnemyRect = cloneEnemy.AddComponent<RectTransform>();

        // RectTransform의 부모 설정 및 위치 설정
        cloneEnemyRect.SetParent(enemyStationRect);

        // anchoredPosition을 사용하여 localPosition 대체
        cloneEnemyRect.localPosition = Vector2.zero;
        cloneEnemyRect.localScale = new Vector3(5, 5, 1); // 스케일은 Transform처럼 설정 가능

        // SpriteRenderer 설정
        string sprite_name = Convert.ToString(floor) + "F_" + name;
        image.sprite = Resources.Load<Sprite>($"BattlePrefabs/EnemySprites/{sprite_name}");
        image.material = spriteOutline;

        // Unit 컴포넌트 초기 설정
        enemyUnit[enemyCount].InitialSetting(enemyHUD[enemyCount], enemyStationController[enemyCount]);
        // 보스 여부에 따른 적 설정
        if (boss)
        {
            Boss bossPrefab = new Boss();
            bossPrefab.NewEnemy(floor, name, cloneEnemy, battleManager);
            enemyUnit[enemyCount].EnemySetting(bossPrefab);
        }
        else
        {
            Enemy enemyPrefab = new Enemy();
            enemyPrefab.NewEnemy(floor, name, cloneEnemy, battleManager); // 팩토리 패턴으로 적 생성
            enemyUnit[enemyCount].EnemySetting(enemyPrefab);
            
        }

        enemyStationController[enemyCount].SetUp();
        enemyCount++;
    }


    public Unit[] GetPlayerUnit() { return playerUnit; }
    public Unit[] GetEnemyUnit() { return enemyUnit; }
    public StationController[] GetPlayerStationController() { return playerStationController; }
    public StationController[] GetEnemyStationController() {  return enemyStationController; }
}
