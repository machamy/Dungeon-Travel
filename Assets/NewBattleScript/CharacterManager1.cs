using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Scripts.UserData;
using Scripts.Entity;
using Scripts.Data;

public class CharacterManager1 : MonoBehaviour
{
    public GameObject unitPrefab;

    public List<Transform> friendlyStation;
    public List<Transform> enemyStation;
    public Transform turnStation;

    List<Character> friendlyCharacter;
    List<Character> enemyCharacter;

    List<GameObject> friendlyCharacterObject;
    List<GameObject> enemyCharacterObject;

    /// <summary>
    /// 이동하는 시간
    /// </summary>
    public float duration;

    /// <summary>
    /// Create Test Party and print Character's info
    /// </summary>
    public void TestParty()
    {
        Party partyInstance = Party.CreateInstance();  // Party 생성

        friendlyCharacter = partyInstance.GetCharacters();  // 파티 리스트 가져오기

        for (int i = 0; i < 6; i++)
        {
            friendlyCharacterObject.Add(null);
            enemyCharacterObject.Add(null);
        }

        foreach (Character character in friendlyCharacter)
        {
            Debug.Log($"캐릭터 이름: {character.Name}, 클래스: {character._class.name}, 레벨: {character.LV}");
        }
    }

    public List<Character> SpawnFriendlyCharacter()
    {
        int position = 0;
        foreach (Character character in friendlyCharacter)
        {
            if (character != null)
            {
                friendlyCharacterObject[position] = Instantiate(unitPrefab, friendlyStation[position]);
            }
            position++;
        }
        return friendlyCharacter;
    }

    public List<Character> SpawnEnemyCharacter()
    {
        int position = 0;
        Character testEnemy;
        enemyCharacter = new List<Character>();

        testEnemy = new Character("Rabbit", 20);
        testEnemy.rawBaseStat = ScriptableObject.CreateInstance<StatData>();
        testEnemy.rawBaseStat.hp = 20;
        testEnemy.rawBaseStat.mp = 20;
        testEnemy.rawBaseStat.agi = 10;
        testEnemy.hp = 20;
        testEnemy.mp = 20;
        testEnemy.agi = 10;
        testEnemy.isFriendly = false;

        enemyCharacter.Add(testEnemy);

        testEnemy = new Character("Rabbit", 19);
        testEnemy.rawBaseStat = ScriptableObject.CreateInstance<StatData>();
        testEnemy.rawBaseStat.hp = 19;
        testEnemy.rawBaseStat.mp = 19;
        testEnemy.rawBaseStat.agi = 10;
        testEnemy.hp = 19;
        testEnemy.mp = 19;
        testEnemy.agi = 10;
        testEnemy.isFriendly = false;

        enemyCharacter.Add(testEnemy);

        foreach (Character character in enemyCharacter)
        {
            if (character != null)
            {
                enemyCharacterObject[position] = Instantiate(unitPrefab, enemyStation[position]);
            }
            position++;
        }
        return enemyCharacter;
    }

    public IEnumerator MoveCenter(Character character)
    {
        bool isFriendly = true;
        int index = friendlyCharacter.IndexOf(character);
        if (index == -1)
        {
            isFriendly = false;
            index = enemyCharacter.IndexOf(character);
        }

        if (index == -1) yield break; // 캐릭터가 없으면 종료

        Transform targetTransform = isFriendly ? friendlyCharacterObject[index].transform : enemyCharacterObject[index].transform;
        Vector3 startPos = targetTransform.position;
        Vector3 endPos = turnStation.position;

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration; // 0 ~ 1 값
            targetTransform.position = Vector3.Lerp(startPos, endPos, t);
            yield return null; // 한 프레임 대기
        }

        targetTransform.position = endPos; // 정확한 위치 보정
    }
    public IEnumerator MoveInplace(Character character)
    {
        bool isFriendly = true;
        int index = friendlyCharacter.IndexOf(character);
        if (index == -1)
        {
            isFriendly = false;
            index = enemyCharacter.IndexOf(character);
        }

        if (index == -1) yield break; // 캐릭터가 없으면 종료

        Transform targetTransform = isFriendly ? friendlyCharacterObject[index].transform : enemyCharacterObject[index].transform;
        Vector3 startPos = turnStation.position;
        Vector3 endPos = isFriendly? friendlyStation[index].position: enemyStation[index].position;

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration; // 0 ~ 1 값
            targetTransform.position = Vector3.Lerp(startPos, endPos, t);
            yield return null; // 한 프레임 대기
        }

        targetTransform.position = endPos; // 정확한 위치 보정
    }


}
