using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Scripts.Entity;
using Scripts.Data;

public class BattleUIManager1 : MonoBehaviour
{
    public enum ActEnum : int { Choose, Attack, Item, Skill, Guard, End}

    public ActEnum actEnum;
    public GameObject actPanel, skillPanel, itemPanel, guardPanel;

    public List<Character> friendlyCharacter;
    public List<Character> enemyCharacter;
    public Character turnCharacter;

    bool isTargetEnemy;
    public int targetPosition;
    bool keep;
    
    public void Initailize(List<Character> friendlyCharacter, List<Character> enemyCharacter)
    {
        gameObject.SetActive(true);
        this.friendlyCharacter = friendlyCharacter;
        this.enemyCharacter = enemyCharacter;
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
        isTargetEnemy = false;
        keep = true;
        actEnum = ActEnum.Choose;
        targetPosition = -1;

        actPanel.SetActive(true);
        skillPanel.SetActive(false);
        itemPanel.SetActive(false);
        guardPanel.SetActive(false);

        while (keep)
        {
            switch (actEnum)
            {
                case ActEnum.Choose:
                    {
                        break;
                    }

                case ActEnum.Attack:
                    {
                        isTargetEnemy = true;
                        yield return Target();
                        break;
                    }

                case ActEnum.Item:
                    {
                        
                        break;
                    }
                case ActEnum.Skill:
                    {
                        break;
                    }
                case ActEnum.Guard:
                    {
                        break;
                    }
                case ActEnum.End:
                    {
                        keep = false;
                        break;
                    }
                default:
                    {
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

    public Character target;
    IEnumerator Target()
    {
        actPanel.SetActive(false);
        targetPosition = 0; //나중에 삭제

        // 적을 타겟으로하는 스킬
        if (isTargetEnemy)
        {
            // 처음 select되어있는 캐릭터 정함
            foreach (Character target in enemyCharacter)
            {
                if (target != null)
                {
                    if (target.isDead) continue; //죽어있으면 패스
                    this.target = target;

                    break;
                }
            }
        }
        // 아군을 타겟으로 하는 스킬
        else
        {
            // 처음 select되어있는 캐릭터 정함
            foreach (Character target in friendlyCharacter)
            {
                // 처음 select되어있는 캐릭터 정하기
                if (target != null)
                {
                    if (target.isDead) continue; //죽어있으면 패스
                    this.target = target;

                    break;
                }
            }
        }

        bool cancel = false;
        while (targetPosition == -1 || !cancel)
        {
            


            yield return null;
        }
        actEnum = ActEnum.End;
    }
}