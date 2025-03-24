using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using Scripts.UserData;
using Scripts.Entity;
using Scripts.Data;

public class BattleSystemManager1 : MonoBehaviour
{
    public BackgroundManager1 backgroundManager;
    public BattleUIManager1 battleUIManager;
    public StatusManager1 statusManager;
    public WorldManager1 worldManager;
    public CharacterManager1 characterManager;

    public List<UnitHolder> friendlyUnit;
    public List<UnitHolder> enemyUnit;

    Queue<UnitHolder> turnOrder;
    public bool isSymbolAttacked;

    Coroutine battleCoroutine;

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
        friendlyUnit = new List<UnitHolder>();
        enemyUnit = new List<UnitHolder>();
        // 매니저 초기화
        characterManager.Initialize(friendlyUnit, enemyUnit);
        backgroundManager.Initailize();
        statusManager.Inistialize(friendlyUnit, enemyUnit);
        battleUIManager.Initailize(friendlyUnit, enemyUnit);

        battleCoroutine = StartCoroutine(BattleCoroutine());
    }

    IEnumerator BattleCoroutine()
    {
        backgroundManager.addTurn();

        //첫번째 턴
        yield return FirstTurnOrder();
        while (turnOrder.Count > 0)
        {
            Debug.Log(turnOrder.Peek().name + " 턴 시작");
            if (turnOrder.Peek().isFriendly)
            {
                yield return FriendlyTurn(turnOrder.Dequeue());
            }
            else
            {
                yield return EnemyTurn(turnOrder.Dequeue());
            }
            yield return BattleCheck();
        }
        backgroundManager.addTurn();

        //2번째 턴부터
        while (true)
        {
            yield return GenericTurnOrder();
            while(turnOrder.Count > 0)
            {
                Debug.Log(turnOrder.Peek().name + " 턴 시작");
                if (turnOrder.Peek().isFriendly)
                {
                    yield return FriendlyTurn(turnOrder.Dequeue());
                }
                else
                {
                    yield return EnemyTurn(turnOrder.Dequeue());
                }
                yield return BattleCheck();
            }
            backgroundManager.addTurn();
        }
    }

    /// <summary>
    /// BattleCoroutine 중 승리 또는 패배 조건 달성 시 호출
    /// </summary>
    /// <returns></returns>
    IEnumerator BattleCheck()
    {
        int friendlyCount = 0;
        foreach(UnitHolder unit in friendlyUnit)
        {
            if (unit != null)
            {
                if (unit.hp <= 0)
                {
                    unit.isDead = true;
                    friendlyCount--;
                }
                friendlyCount++;
            }
        }

        int enemyCount = 0;
        foreach(UnitHolder unit in enemyUnit)
        {
            if (unit != null)
            {
                if (unit.hp <= 0)
                {
                    unit.isDead = true;
                    enemyCount--;
                }
                enemyCount++;
            }
        }

        if(enemyCount == 0)
        {
            yield return Win();
        }
        if(friendlyCount == 0)
        {
            yield return Lose();
        }

        yield return null;
    }
    IEnumerator FirstTurnOrder()
    {
        turnOrder = new Queue<UnitHolder>();
        List<UnitHolder> friendlyTurnOrder = new List<UnitHolder>();
        List<UnitHolder> enemyTurnOrder = new List<UnitHolder>();

        foreach (UnitHolder unit in friendlyUnit)
        {
            if (unit != null)
            {
                friendlyTurnOrder.Add(unit);
            }
        }
        // 민첩성(FinalStat.agi) 기준 내림차순 정렬 (큰 값이 먼저 오도록)
        friendlyTurnOrder.Sort((a, b) => b.agi.CompareTo(a.agi));

        foreach (UnitHolder unit in enemyUnit)
        {
            if (unit != null)
            {
                enemyTurnOrder.Add(unit);
            }
        }
        // 민첩성(FinalStat.agi) 기준 내림차순 정렬 (큰 값이 먼저 오도록)
        enemyTurnOrder.Sort((a, b) => b.agi.CompareTo(a.agi));

        if (isSymbolAttacked) //100% 확률로 아군 턴
        {
            foreach (UnitHolder unit in friendlyTurnOrder)
            {
                turnOrder.Enqueue(unit);
            }
            foreach (UnitHolder unit in enemyTurnOrder)
            {
                turnOrder.Enqueue(unit);
            }
        }
        else
        {
            if (Random.value < 0.3f) // 30% 확률로 일반적인 턴
            {
                List<UnitHolder> genericTurnOrder = new List<UnitHolder>();
                genericTurnOrder.AddRange(friendlyTurnOrder);
                genericTurnOrder.AddRange(enemyTurnOrder);
                // 민첩성(FinalStat.agi) 기준 내림차순 정렬 (큰 값이 먼저 오도록)
                genericTurnOrder.Sort((a, b) => b.agi.CompareTo(a.agi));

                foreach (UnitHolder unit in genericTurnOrder)
                {
                    turnOrder.Enqueue(unit);
                }
            }
            else //70% 확률로 적 턴 먼저
            {
                foreach (UnitHolder unit in enemyTurnOrder)
                {
                    turnOrder.Enqueue(unit);
                }
                foreach (UnitHolder unit in friendlyTurnOrder)
                {
                    turnOrder.Enqueue(unit);
                }
            }
        }

        // 정렬된 리스트 확인용 출력 (디버깅용)
        foreach (var unit in turnOrder)
        {
            Debug.Log($"{unit.name} - AGI: {unit.agi}");
        }
        
        yield return null;
    }

    IEnumerator GenericTurnOrder()
    {
        turnOrder = new Queue<UnitHolder>();
        List<UnitHolder> genericTurnOrder = new List<UnitHolder>();
        genericTurnOrder.AddRange(friendlyUnit);
        genericTurnOrder.AddRange(enemyUnit);

        // 민첩성(FinalStat.agi) 기준 내림차순 정렬 (큰 값이 먼저 오도록)
        genericTurnOrder.Sort((a, b) => b.agi.CompareTo(a.agi));

        foreach (UnitHolder character in genericTurnOrder)
        {
            turnOrder.Enqueue(character);
        }

        yield return null;
    }

    IEnumerator FriendlyTurn(UnitHolder turnCharacter)
    {
        //캐릭터 가운데로 이동
        yield return characterManager.MoveCenter(turnCharacter);

        //캐릭터 행동 선택
        yield return battleUIManager.ActCoroutine();

        ///캐릭터 제자리로 이동
        yield return characterManager.MoveInplace(turnCharacter);
    }

    IEnumerator EnemyTurn(UnitHolder turnCharacter)
    {
        yield return null;
    }

    public IEnumerator Attack(UnitHolder turnCharacter, UnitHolder targetCharacter, SkillData useSkill = null)
    {
        targetCharacter.hp -= 20f;

        yield return null;
    }

    IEnumerator Win()
    {
        Debug.Log("승리");

        yield return statusManager.DestroyAll();
        yield return characterManager.Destroy();
        yield return backgroundManager.End();
        if (battleCoroutine != null) StopCoroutine(battleCoroutine);
    }

    IEnumerator Lose()
    {
        yield return backgroundManager.End();
        if (battleCoroutine != null) StopCoroutine(battleCoroutine);
    }
}
