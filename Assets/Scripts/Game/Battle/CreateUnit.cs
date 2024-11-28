using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Data;
using Scripts.Entity;
using Scripts.User;

public class CreateUnit : MonoBehaviour
{
    public GameObject unitPrefab; // 유닛 프리팹

    private void Start()
    {
        Party party = Party.CreateInstance();

        // 파티 멤버를 기반으로 유닛 생성
        foreach (Character character in party.GetCharacters())
        {
            SpawnPlayerUnit(character);
        }
    }

    public void SpawnPlayerUnit(Character character)
    {
        // 유닛 프리팹 생성
        GameObject unitObject = Instantiate(unitPrefab, RandomPosition(), Quaternion.identity);

        // Unit 컴포넌트 가져오기
        BattlePlayerUnit playerUnit = unitObject.GetComponent<BattlePlayerUnit>();
        if (playerUnit == null)
        {
            playerUnit = unitObject.AddComponent<BattlePlayerUnit>();
        }

        // 유닛 초기화
        playerUnit.Initialize(character);
    }

    public void SpawnEnemyUnit(Character character)
    {
        // 유닛 프리팹 생성
        GameObject unitObject = Instantiate(unitPrefab, RandomPosition(), Quaternion.identity);

        // Unit 컴포넌트 가져오기
        BattleEnemyUnit enemyUnit = unitObject.GetComponent<BattleEnemyUnit>();
        if (enemyUnit == null)
        {
            enemyUnit = unitObject.AddComponent<BattleEnemyUnit>();
        }

        // 유닛 초기화
        enemyUnit.Initialize(character);
    }

    private Vector3 RandomPosition()
    {
        return new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
    }
}
