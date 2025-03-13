using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Scripts.UserData;
using Scripts.Entity;
using Scripts.Data;


public class CharacterManager1 : MonoBehaviour
{
    public GameObject unitPrefab;
    MaterialPropertyBlock mpb;

    public Color outlineColor = Color.red;
    public int outlineSize = 2;

    public List<Transform> friendlyStation;
    public List<Transform> enemyStation;

    public List<Button> friendlyButton;
    public List<Button> enemyButton;

    public Transform turnStation;

    List<Character> friendlyCharacter;
    List<Character> enemyCharacter;

    List<GameObject> friendlyCharacterObject;
    List<GameObject> enemyCharacterObject;

    /// <summary>
    /// 이동하는 시간
    /// </summary>
    public float duration;

    public void Initialize()
    {   
        //아웃라인 초기화
        mpb = new MaterialPropertyBlock();
        unitPrefab.GetComponent<SpriteRenderer>().GetPropertyBlock(mpb);
        mpb.SetColor("_OutlineColor", outlineColor);
        mpb.SetFloat("_OutlineSize", outlineSize);
    }

    /// <summary>
    /// Create Test Party and print Character's info
    /// </summary>
    public void TestParty()
    {
        Party partyInstance = Party.CreateInstance();  // Party 생성

        friendlyCharacter = partyInstance.GetCharacters();  // 파티 리스트 가져오기

        friendlyCharacterObject = new List<GameObject>();
        enemyCharacterObject = new List<GameObject>();

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
                friendlyCharacterObject.Add(Instantiate(unitPrefab, friendlyStation[position]));
                character.position = position;

                friendlyButton[position].interactable = true;
            }
            else
            {
                friendlyCharacterObject.Add(new GameObject());
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
                enemyCharacterObject.Add(Instantiate(unitPrefab, enemyStation[position]));
                character.position = position;

                enemyButton[position].interactable = true;
            }
            else
            {
                enemyCharacterObject.Add(new GameObject());
            }
            position++;
        }
        return enemyCharacter;
    }

    public IEnumerator MoveCenter(Character character)
    {
        bool isFriendly = true;
        int index = character.position;

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
        int index = character.position;

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

    public void OnOutline(Character character)
    {
        mpb.SetFloat("_Outline", 1f);
        if (character.isFriendly)
            friendlyCharacterObject[character.position].GetComponent<SpriteRenderer>().SetPropertyBlock(mpb);
        else
            enemyCharacterObject[character.position].GetComponent<SpriteRenderer>().SetPropertyBlock(mpb);
    }
    public void OffOutline(Character character)
    {
        mpb.SetFloat("_Outline", 0f);
        if (character.isFriendly)
            friendlyCharacterObject[character.position].GetComponent<SpriteRenderer>().SetPropertyBlock(mpb);
        else
            enemyCharacterObject[character.position].GetComponent<SpriteRenderer>().SetPropertyBlock(mpb);
    }
}