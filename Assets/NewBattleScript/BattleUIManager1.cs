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
    public Button attack, item, skill, guard;
    public Button previousButton;
    public TextMeshProUGUI nameText;
    public List<Transform> skillTransform;
    public List<Button> skillButton;
    public List<TextMeshProUGUI> skillNameText;
    public List<TextMeshProUGUI> skillCostText;
    public GameObject skillIndicater;

    public List<UnitHolder> friendlyUnit;
    public List<UnitHolder> enemyUnit;
    public UnitHolder turnUnit;
    public UnitHolder targetUnit;
    public SkillData useSkill;

    bool keepGoing;

    int skillIndex;

    public void Initailize(List<UnitHolder> friendlyUnit, List<UnitHolder> enemyUnit)
    {
        gameObject.SetActive(true);
        this.friendlyUnit = friendlyUnit;
        this.enemyUnit = enemyUnit;
        previousButton = attack;
        turnUnit = null;
    }


    /// <summary>
    /// 유니티 인스펙터창 button에서 호출
    /// </summary>
    /// <param name="actEnum"></param>
    public void ChangeStatus(int actEnum)
    {
        this.actEnum = (ActEnum)actEnum;
    }

    public void SetSkillIndex(int index)
    {
        skillIndex = index;
        
    }

    public void MoveSkillIndicater(int index)
    {
        skillIndicater.transform.position = skillTransform[index].position - new Vector3(15, 0, 0);
    }

    public IEnumerator ActCoroutine()
    {
        keepGoing = true;
        actEnum = ActEnum.Choose;
        previousButton.Select();
        
        while (keepGoing)
        {
            switch (actEnum)
            {
                case ActEnum.Choose:
                    {
                        actPanel.SetActive(true);
                        skillIndex = -2;
                        targetUnit = null;
                        yield return new WaitUntil(() => actEnum != ActEnum.Choose);
                        break;
                    }

                case ActEnum.Attack:
                    {
                        previousButton = attack;
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
                        previousButton = item;
                        break;
                    }
                case ActEnum.Skill:
                    {
                        previousButton = skill;
                        actPanel.SetActive(false);
                        skillPanel.SetActive(true);
                        yield return SkillSelect();
                        skillPanel.SetActive(false);
                        if (useSkill.isBuff)
                        {
                            yield return characterManager.Target(false);
                        }
                        else
                        {
                            yield return characterManager.Target(true);
                        }
                        
                        if(skillIndex != -1)
                        {
                            yield return battleSystemManager.Attack(turnUnit, targetUnit, useSkill);
                            keepGoing = false;
                        }
                        actEnum = ActEnum.Choose;
                        break;
                    }
                case ActEnum.Guard:
                    {
                        previousButton = guard;
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

    
    IEnumerator SkillSelect()
    {
        int index = 0;
        foreach(SkillData skill in turnUnit.skillData)
        {
            if (skill != null)
            {
                skillNameText[index].text = skill.name;
                skillCostText[index].text = skill.pointCost.ToString();
            }
            index++;
        }
        index = 0;
        foreach (SkillData skill in turnUnit.skillData)
        {
            if (skill != null)
            {
                MoveSkillIndicater(index);
                skillButton[index].Select();
                break;
            }
            index++;
        }


        skillPanel.SetActive(true);
        yield return new WaitUntil(() => skillIndex != -2);
        if (skillIndex != -1) useSkill = turnUnit.skillData[skillIndex];
    }

    IEnumerator Guard()
    {
        yield return null;
    }
}