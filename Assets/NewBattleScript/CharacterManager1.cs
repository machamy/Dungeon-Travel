using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Scripts.UserData;
using Scripts.Entity;
using Scripts.Data;
using Unity.VisualScripting;


public class CharacterManager1 : MonoBehaviour
{
    public BattleUIManager1 battleUIManager;

    public GameObject unitPrefab;

    public Color outlineColor = Color.red;
    public int outlineSize = 2;

    public List<Transform> friendlyStation;
    public List<Transform> enemyStation;

    public List<Button> friendlyButton;
    public List<Button> enemyButton;

    public Transform turnStation;

    List<UnitHolder> friendlyUnit;
    List<UnitHolder> enemyUnit;

    public List<SkillData> skillList;

    public GameObject indicater;
    /// <summary>
    /// 이동하는 시간
    /// </summary>
    public float duration;

    public void Initialize(List<UnitHolder> friendlyUnit, List<UnitHolder> enemyUnit)
    {
        this.friendlyUnit = friendlyUnit;
        this.enemyUnit = enemyUnit;
        TestParty();
        friendlyUnit = SpawnFriendlyCharacter();
        enemyUnit = SpawnEnemyCharacter();
    }

    /// <summary>
    /// Create Test Party and print Character's info
    /// </summary>
    public void TestParty()
    {
        Party partyInstance = Party.CreateInstance();  // Party 생성
        List<Character> partyCharacter = partyInstance.GetCharacters();// 파티 리스트 가져오기

        ClassType classType = ClassType.Ranger;
        SkillData[] skillArr = DB.GetSkillDataArr(classType);
        Debug.Log(skillArr.Length);
        for(int i = 0; i < skillArr.Length; i++)
        {
            if (skillArr[i] == null) continue;
            Debug.Log(skillArr[i].ToString());
        }

        int index = 0;
        foreach(Character character in partyCharacter)
        {
            if(character != null)
            {
                friendlyUnit.Add(new UnitHolder());
                friendlyUnit[index].skillData.Add(skillList[0]);
                friendlyUnit[index].skillData.Add(skillList[1]);
                friendlyUnit[index].skillData.Add(skillList[2]);
                friendlyUnit[index].skillData.Add(skillList[3]);
                friendlyUnit[index].SetCharacter(character);
            }
            index++;
        }
        
        foreach (UnitHolder character in friendlyUnit)
        {
            Debug.Log($"캐릭터 이름: {character.name}, 클래스: {character.className}, 레벨: {character.lv}");
            int i = 0;
        }
    }
    public List<UnitHolder> SpawnFriendlyCharacter()
    {
        int position = 0;
        foreach (UnitHolder unit in friendlyUnit)
        {
            if (unit != null)
            {
                unit.gameObject = Instantiate(unitPrefab, friendlyStation[position]);
                unit.position = position;
                unit.isFriendly = true;
                unit.isDead = false;

                friendlyButton[position].enabled = true;
            }
            position++;
        }
        return friendlyUnit;
    }
    public List<UnitHolder> SpawnEnemyCharacter()
    {
        int position = 0;
        UnitHolder testEnemy = new UnitHolder();
        Character tempCharacter = new Character("Rabbit", 20);
        tempCharacter.rawBaseStat = ScriptableObject.CreateInstance<StatData>();
        tempCharacter.rawBaseStat.hp = 20;
        tempCharacter.rawBaseStat.mp = 20;
        tempCharacter.rawBaseStat.agi = 10;
        testEnemy.SetCharacter(tempCharacter);
        testEnemy.hp = 20;
        testEnemy.mp = 20;
        testEnemy.agi = 10;

        enemyUnit.Add(testEnemy);

        testEnemy = new UnitHolder();
        tempCharacter = new Character("Rabbit", 19);
        tempCharacter.rawBaseStat = ScriptableObject.CreateInstance<StatData>();
        tempCharacter.rawBaseStat.hp = 19;
        tempCharacter.rawBaseStat.mp = 19;
        tempCharacter.rawBaseStat.agi = 10;
        testEnemy.SetCharacter(tempCharacter);
        testEnemy.hp = 19;
        testEnemy.mp = 19;
        testEnemy.agi = 10;

        enemyUnit.Add(testEnemy);

        foreach (UnitHolder unit in enemyUnit)
        {
            if (unit != null)
            {
                unit.gameObject = Instantiate(unitPrefab, enemyStation[position]);
                unit.position = position;
                testEnemy.isFriendly = false;
                testEnemy.isDead = false;

                enemyButton[position].enabled = true;
            }
            position++;
        }
        return enemyUnit;
    }

    public void UnitDie(UnitHolder unit)
    {
        int index = friendlyUnit.IndexOf(unit);
        if (index == -1)
        {
            index = enemyUnit.IndexOf(unit);
            enemyButton[index].enabled = false;
            Destroy(unit.gameObject);
        }
        else
        {
            friendlyButton[index].enabled = false;
            Destroy(unit.gameObject);
        }
        unit.isDead = true;
    }
    public IEnumerator MoveCenter(UnitHolder character)
    {
        bool isFriendly = true;
        int index = character.position;

        Transform targetTransform = isFriendly ? friendlyUnit[index].gameObject.transform : enemyUnit[index].gameObject.transform;
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
    public IEnumerator MoveInplace(UnitHolder character)
    {
        bool isFriendly = true;
        int index = character.position;

        Transform targetTransform = isFriendly ? friendlyUnit[index].gameObject.transform : enemyUnit[index].gameObject.transform;
        Vector3 startPos = turnStation.position;
        Vector3 endPos = isFriendly ? friendlyStation[index].position : enemyStation[index].position;

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

    /// <summary>
    /// Inspector 창에서 연결
    /// 적 선택시만 적용
    /// </summary>
    public void MoveIndicater(int position)
    {
        indicater.SetActive(true);
        indicater.transform.position = enemyStation[position].position + new Vector3(0,1);
    }

    int targetPosition;
    public IEnumerator Target(bool isAttackSkill)
    {
        targetPosition = -1; //나중에 삭제

        // 적을 타겟으로하는 스킬
        if (isAttackSkill)
        {
            // 처음 select되어있는 캐릭터 정함
            foreach (UnitHolder target in enemyUnit)
            {
                if (target != null)
                {
                    if (target.isDead) continue; //죽어있으면 패스
                    enemyButton[target.position].Select();

                    break;
                }
            }
        }
        // 아군을 타겟으로 하는 스킬
        else
        {
            // 처음 select되어있는 캐릭터 정함
            foreach (UnitHolder target in friendlyUnit)
            {
                // 처음 select되어있는 캐릭터 정하기
                if (target != null)
                {
                    if (target.isDead) continue; //죽어있으면 패스
                    friendlyButton[target.position].Select();
                    break;
                }
            }
        }

        yield return new WaitUntil(() => targetPosition != -1);
        indicater.SetActive(false);
        battleUIManager.targetUnit = isAttackSkill ? enemyUnit[targetPosition] : friendlyUnit[targetPosition];
    }

    /// <summary>
    /// Inspector창에 버튼과 연결되어 있음
    /// </summary>
    /// <param name="targetPosition"></param>
    public void Target(int targetPosition)
    {
        this.targetPosition = targetPosition;
    }

    public IEnumerator Destroy()
    {
        foreach (Button button in friendlyButton) button.enabled = false;
        foreach (Button button in enemyButton) button.enabled = false;
        foreach (UnitHolder unit in friendlyUnit) Destroy(unit.gameObject);
        foreach (UnitHolder unit in enemyUnit) Destroy(unit.gameObject);
        yield return null;
    }
}