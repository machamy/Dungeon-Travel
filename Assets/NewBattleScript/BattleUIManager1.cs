using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Scripts.Entity;
using Scripts.Data;
using TMPro;

public class BattleUIManager1 : MonoBehaviour
{
    public BattleSystemManager1 battleSystemManager;
    public CharacterManager1 characterManager;
    public enum ActEnum : int { Choose, Attack, Item, Skill, Guard}

    public ActEnum actEnum;
    public GameObject actPanel, skillPanel, itemPanel, guardPanel;
    public Button attackButton, itemButton, skillButton, guardButton;
    public Button previousButton;

    public List<UnitHolder> friendlyUnit;
    public List<UnitHolder> enemyUnit;
    public UnitHolder turnUnit;
    public UnitHolder targetUnit;

    bool keepGoing;
    public void Initailize(List<UnitHolder> friendlyUnit, List<UnitHolder> enemyUnit)
    {
        gameObject.SetActive(true);
        this.friendlyUnit = friendlyUnit;
        this.enemyUnit = enemyUnit;
        previousButton = attackButton;
    }

    /// <summary>
    /// 유니티 인스펙터창 button에서 호출
    /// </summary>
    /// <param name="actEnum"></param>
    public void ChangeStatus(int actEnum)
    {
        this.actEnum = (ActEnum)actEnum;
    }

    public IEnumerator ActCoroutine()
    {
        keepGoing = true;
        actEnum = ActEnum.Choose;
        targetUnit = null;
        previousButton.Select();
        
        while (keepGoing)
        {
            switch (actEnum)
            {
                case ActEnum.Choose:
                    {
                        actPanel.SetActive(true);
                        yield return new WaitUntil(() => actEnum != ActEnum.Choose);
                        break;
                    }

                case ActEnum.Attack:
                    {
                        previousButton = attackButton;
                        actPanel.SetActive(false);
                        yield return characterManager.Target(true);
                        if (targetUnit != null)
                        {
                            yield return battleSystemManager.Attack(turnUnit, targetUnit);
                            keepGoing = false;
                        }
                        actEnum = ActEnum.Choose;
                        break;
                    }

                case ActEnum.Item:
                    {
                        previousButton = itemButton;
                        break;
                    }
                case ActEnum.Skill:
                    {
                        previousButton = skillButton;
                        break;
                    }
                case ActEnum.Guard:
                    {
                        previousButton = guardButton;
                        break;
                    }
            }
            yield return null;
        }
    }

    IEnumerator Item()
    {
        yield return null;
    }

    IEnumerator Skill()
    {
        yield return null;
    }

    IEnumerator Guard()
    {
        yield return null;
    }
}