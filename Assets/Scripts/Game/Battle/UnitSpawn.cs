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

    public GameObject[] playerStation = new GameObject[6];
    public GameObject[] enemyStation = new GameObject[6];
    public StationController[] playerStationController = new StationController[6];
    public StationController[] enemyStationController = new StationController[6];


    public HUDmanager[] playerHUD = new HUDmanager[6];
    public HUDmanager[] enemyHUD = new HUDmanager[6];



    Unit[] playerUnit = new Unit[6];
    Unit[] enemyUnit = new Unit[6];

    GameObject[] player = new GameObject[6];

    public int PlayerCount;
    public int EnemyCount;

    public void Awake()
    {
        PlayerCount = 0;
        EnemyCount = 0;
    }

    /// <summary>
    /// 플레이어 유닛을 스폰하는 함수
    /// </summary>
    [ContextMenu("Spawn Unit")]
    public void SpawnPlayerUnit()
    {
        player[PlayerCount] = Instantiate(playerPrefab, playerStation[PlayerCount].transform);
        playerUnit[PlayerCount] = player[PlayerCount].GetComponent<Unit>();
        playerHUD[PlayerCount].SetupHUD(playerUnit[PlayerCount]);
        playerUnit[PlayerCount].stat = ScriptableObject.CreateInstance<StatData>();
        playerUnit[PlayerCount].stat.hp = 1;
        playerUnit[PlayerCount].InitialSetting(playerHUD[PlayerCount]);
        playerStationController[PlayerCount].SetUp();

        battleManager.alivePlayer++;
        PlayerCount++;
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
        GameObject cloneEnemy = new GameObject($"{name}({EnemyCount})");
        SpriteRenderer image = cloneEnemy.AddComponent<SpriteRenderer>();
        cloneEnemy.AddComponent<BuffManager>();
        enemyUnit[EnemyCount] = cloneEnemy.AddComponent<Unit>();
        enemyUnit[EnemyCount].stat = ScriptableObject.CreateInstance<StatData>();

        // 게임 오브젝트의 위치 설정 (RectTransform을 고려)
        RectTransform enemyStationRect = enemyStation[EnemyCount].GetComponent<RectTransform>();
        RectTransform cloneEnemyRect = cloneEnemy.AddComponent<RectTransform>();

        // RectTransform의 부모 설정 및 위치 설정
        cloneEnemyRect.SetParent(enemyStationRect);

        // anchoredPosition을 사용하여 localPosition 대체
        cloneEnemyRect.anchoredPosition = Vector2.zero;
        cloneEnemyRect.localScale = new Vector3(5, 5, 1); // 스케일은 Transform처럼 설정 가능

        // SpriteRenderer 설정
        string sprite_name = Convert.ToString(floor) + "F_" + name;
        image.sprite = Resources.Load<Sprite>($"BattlePrefabs/EnemySprites/{sprite_name}");
        image.material = spriteOutline;

        // Unit 컴포넌트 초기 설정
        enemyUnit[EnemyCount].InitialSetting(enemyHUD[EnemyCount]);
        // 보스 여부에 따른 적 설정
        if (boss)
        {
            Boss bossPrefab = new Boss();
            bossPrefab.NewEnemy(floor, name, cloneEnemy, battleManager);
            enemyUnit[EnemyCount].EnemySetting(bossPrefab);
        }
        else
        {
            Enemy enemyPrefab = new Enemy();
            enemyPrefab.NewEnemy(floor, name, cloneEnemy, battleManager); // 팩토리 패턴으로 적 생성
            enemyUnit[EnemyCount].EnemySetting(enemyPrefab);
            
        }

        enemyStationController[EnemyCount].SetUp();
        EnemyCount++;
    }


    public Unit[] GetPlayerUnit() { return playerUnit; }
    public Unit[] GetEnemyUnit() { return enemyUnit; }
    public StationController[] GetPlayerStationController() { return playerStationController; }
    public StationController[] GetEnemyStationController() {  return enemyStationController; }
}
