using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using Scripts.UserData;
using Scripts.Entity;

public class BattleSystemManager1 : MonoBehaviour
{
    public BackgroundManager1 backgroundManager;
    public BattleUIManager1 battleUIManager;
    public StatusManager1 statusManager;
    public WorldManager1 worldManager;
    public CharacterManager1 characterManager;

    public List<Character> friendlyCharacter;
    public List<Character> enemyCharacter;

    Queue<Character> turnOrder;
    public bool isSymbolAttacked;

    private void Awake()
    {
        DB.Instance.UpdateDB();

        Initailize();
    }

    /// <summary>
    /// Initailize Battle Field
    /// </summary>
    void Initailize()
    {
        //테스트 파티 추가
        characterManager.TestParty();

        // 캔버스 초기화
        backgroundManager.Initailize();
        battleUIManager.gameObject.SetActive(true);
        statusManager.gameObject.SetActive(true);
        worldManager.gameObject.SetActive(true);

        //아군 소환
        friendlyCharacter = characterManager.SpawnFriendlyCharacter();
        //적 소환
        enemyCharacter = characterManager.SpawnEnemyCharacter();

        //상태창 연결
        statusManager.Inistialize(friendlyCharacter, enemyCharacter);

        StartCoroutine(BattleCoroutine());
    }

    IEnumerator BattleCoroutine()
    {
        yield return FirstTurnOrder();
        while (turnOrder.Count > 0)
        {
            if (turnOrder.Peek().isFriendly)
            {
                yield return FriendlyTurn(turnOrder.Dequeue());
            }
            else
            {
                yield return null;
            }
        }
        while (true)
        {
            yield return GenericTurnOrder();
            while(turnOrder.Count > 0)
            {
                if (turnOrder.Peek().isFriendly)
                {
                    yield return FriendlyTurn(turnOrder.Dequeue());
                }
                else
                {
                    yield return null;
                }
            }
        }
    }

    IEnumerator FirstTurnOrder()
    {
        turnOrder = new Queue<Character>();
        List<Character> friendlyTurnOrder = new List<Character>();
        List<Character> enemyTurnOrder = new List<Character>();

        foreach (Character character in friendlyCharacter)
        {
            if (character != null)
            {
                friendlyTurnOrder.Add(character);
            }
        }
        // 민첩성(FinalStat.agi) 기준 내림차순 정렬 (큰 값이 먼저 오도록)
        friendlyTurnOrder.Sort((a, b) => b.FinalStat.agi.CompareTo(a.FinalStat.agi));

        foreach (Character character in enemyCharacter)
        {
            if (character != null)
            {
                enemyTurnOrder.Add(character);
            }
        }
        // 민첩성(FinalStat.agi) 기준 내림차순 정렬 (큰 값이 먼저 오도록)
        enemyTurnOrder.Sort((a, b) => b.FinalStat.agi.CompareTo(a.agi));

        if (isSymbolAttacked) //100% 확률로 아군 턴
        {
            foreach (Character character in friendlyTurnOrder)
            {
                turnOrder.Enqueue(character);
            }
            foreach (Character character in enemyTurnOrder)
            {
                turnOrder.Enqueue(character);
            }
        }
        else
        {
            if (Random.value < 0.3f) // 30% 확률로 일반적인 턴
            {
                List<Character> genericTurnOrder = new List<Character>();
                genericTurnOrder.AddRange(friendlyTurnOrder);
                genericTurnOrder.AddRange(enemyTurnOrder);
                // 민첩성(FinalStat.agi) 기준 내림차순 정렬 (큰 값이 먼저 오도록)
                genericTurnOrder.Sort((a, b) => b.FinalStat.agi.CompareTo(a.FinalStat.agi));

                foreach (Character character in genericTurnOrder)
                {
                    turnOrder.Enqueue(character);
                }
            }
            else //70% 확률로 적 턴 먼저
            {
                foreach (Character character in enemyTurnOrder)
                {
                    turnOrder.Enqueue(character);
                }
                foreach (Character character in friendlyTurnOrder)
                {
                    turnOrder.Enqueue(character);
                }
            }
        }

        // 정렬된 리스트 확인용 출력 (디버깅용)
        foreach (var character in turnOrder)
        {
            Debug.Log($"{character.Name} - AGI: {character.FinalStat.agi}");
        }

        yield return null;
    }

    IEnumerator GenericTurnOrder()
    {
        turnOrder = new Queue<Character>();
        List<Character> genericTurnOrder = new List<Character>();
        genericTurnOrder.AddRange(friendlyCharacter);
        genericTurnOrder.AddRange(enemyCharacter);

        // 민첩성(FinalStat.agi) 기준 내림차순 정렬 (큰 값이 먼저 오도록)
        genericTurnOrder.Sort((a, b) => b.FinalStat.agi.CompareTo(a.FinalStat.agi));

        foreach (Character character in genericTurnOrder)
        {
            turnOrder.Enqueue(character);
        }

        yield return null;
    }

    IEnumerator FriendlyTurn(Character turnCharacter)
    {
        Debug.Log("턴시작");
        yield return characterManager.MoveCenter(turnCharacter);
        
        yield return characterManager.MoveInplace(turnCharacter);
    }
}
