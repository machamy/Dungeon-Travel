using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Data;
using Scripts.Entity;
using Scripts.User;
using JetBrains.Annotations;

public class CreateUnit : MonoBehaviour
{
    [SerializeField]
    BattleStat[] playerBattleStat;
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
        playerBattleStat = Resources.LoadAll<BattleStat>("PlayerStat"); // YourFolderName은 Resources 내부 폴더 경로
        battlePlayerUnit = new BattlePlayerUnit[playerBattleStat.Length];

        enemyBattleStat = new StatData[0];
        battleEnemyUnit = new BattleEnemyUnit[enemyBattleStat.Length];

        if (playerBattleStat.Length > 0)
        {
            Debug.Log($"{playerBattleStat.Length} CharacterStat(s) loaded successfully.");
        }
        else
        {
            Debug.LogWarning("No CharacterStat assets found in the specified folder.");
        }
    }

    public void InitialUnitSpawn()
    {
        playerCount = 0;
        enemyCount = 1;

        for (int i = 0; i < playerBattleStat.Length; i++)
        {
            PlayerUnitSpawn(i);
        }
        for (int i = 0; i < enemyBattleStat.Length; i++)
        {
            EnemyUnitSpawn(1, "도적선봉대");
        }
    }

    public void PlayerUnitSpawn(int i)
    {
        GameObject player;
        if (playerBattleStat[i].position == -1) return;

        player = Instantiate(playerPrefab, playerStation[playerBattleStat[i].position].transform);
        battlePlayerUnit[i] = player.GetComponent<BattlePlayerUnit>();
        battlePlayerUnit[i].Initialize(playerBattleStat[i]);
        playerStationController[i].SetUp();
        playerHUD[i].Initialize(battlePlayerUnit[i]);
        playerCount++;
    }

    public void EnemyUnitSpawn(int floor, string name)
    {
        enemyCount++;
    }

    public BattlePlayerUnit[] GetPlayerUnit()
    {
        return battlePlayerUnit;
    }
    public BattleEnemyUnit[] GetEnemyUnit()
    {
        return battleEnemyUnit;
    }
}
