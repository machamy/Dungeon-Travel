using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Data;
using Scripts.Entity;
using Scripts.User;
using JetBrains.Annotations;
using System;

public class CreateUnit : MonoBehaviour
{
    [SerializeField]
    CharacterData[] playerData;
    public GameObject playerPrefab;

    [SerializeField]
    StatData[] enemyBattleStat;
    public GameObject enemyPrefab;

    [SerializeField]
    GameObject[] playerStation = new GameObject[6];
    [SerializeField]
    GameObject[] enemyStation = new GameObject[6];
    [SerializeField]
    StationController[] playerStationController = new StationController[6];
    [SerializeField]
    StationController[] enemyStationController = new StationController[6];
    [SerializeField]
    HUDmanager[] playerHUD = new HUDmanager[6];
    [SerializeField]
    HUDmanager[] enemyHUD = new HUDmanager[6];

    BattlePlayerUnit[] battlePlayerUnit;
    BattleEnemyUnit[] battleEnemyUnit;
    public int playerCount { get { return BattleManager.alivePlayer; } set { BattleManager.alivePlayer = value; } }
    public int enemyCount { get { return BattleManager.aliveEnemy; } set { BattleManager.aliveEnemy = value; } }


    void Awake()
    {
        // Resources 폴더에서 모든 CharacterStat ScriptableObject 로드
        playerData = Resources.LoadAll<CharacterData>("PlayerData"); // YourFolderName은 Resources 내부 폴더 경로
        battlePlayerUnit = new BattlePlayerUnit[playerData.Length];

        battleEnemyUnit = new BattleEnemyUnit[2]; // 나중에 여기서 함수하나 만들어서 적 스폰 데이터 받아오기

        if (playerData.Length > 0)
        {
            Debug.Log($"{playerData.Length} CharacterStat(s) loaded successfully.");
        }
        else
        {
            Debug.LogWarning("No CharacterStat assets found in the specified folder.");
        }
    }

    public void InitialUnitSpawn()
    {
        playerCount = 0;
        enemyCount = 0;

        for (int i = 0; i < playerData.Length; i++)
        {
            PlayerUnitSpawn(i);
        }
        for (int i = 0; i < battleEnemyUnit.Length; i++)
        {
            EnemyUnitSpawn(1, "토끼");
        }
    }

    public void PlayerUnitSpawn(int i)
    {
        GameObject player;
        if (playerData[i].position == -1) return;

        player = Instantiate(playerPrefab, playerStation[playerData[i].position].transform);
        battlePlayerUnit[i] = player.GetComponent<BattlePlayerUnit>();
        battlePlayerUnit[i].Initialize(playerData[i]);
        playerStationController[i].SetUp();
        playerHUD[i].Initialize(battlePlayerUnit[i]);
        playerCount++;
    }

    public void EnemyUnitSpawn(int floor, string name)
    {
        // 객체 생성 후 초기화
        GameObject cloneEnemy = new GameObject($"{name}({enemyCount})");
        SpriteRenderer image = cloneEnemy.AddComponent<SpriteRenderer>();
        cloneEnemy.AddComponent<BuffManager>();
        battleEnemyUnit[enemyCount] = cloneEnemy.AddComponent<BattleEnemyUnit>();
        battleEnemyUnit[enemyCount].isEnemy = true;
        battleEnemyUnit[enemyCount].Initialize(floor, name);

        // 트랜스폼 설정
        RectTransform enemyStationRect = enemyStation[enemyCount].GetComponent<RectTransform>();
        RectTransform cloneEnemyRect = cloneEnemy.AddComponent<RectTransform>();
        cloneEnemyRect.SetParent(enemyStationRect);
        cloneEnemyRect.localPosition = Vector2.zero;
        cloneEnemyRect.localScale = new Vector3(5, 5, 1);

        // 스프라이트 설정
        string sprite_name = Convert.ToString(floor) + "F_" + name;
        image.sprite = Resources.Load<Sprite>($"BattlePrefabs/EnemySprites/{sprite_name}");

        // HUD 설정
        enemyStationController[enemyCount].SetUp();
        enemyHUD[enemyCount].Initialize(battleEnemyUnit[enemyCount]);

        enemyCount++;
    }

    public BattlePlayerUnit[] GetPlayerUnit() { return battlePlayerUnit; }
    public BattleEnemyUnit[] GetEnemyUnit() {  return battleEnemyUnit; }
}
