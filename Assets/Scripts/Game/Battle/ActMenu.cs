using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using Scripts.Data;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;
using Unity.VisualScripting;
using System;
using System.Collections.Generic;
using UnityEditor.SearchService;
using System.Security.Cryptography;
using UnityEditor.Playables;
using Scripts;

public class ActMenu : MonoBehaviour
{
    private EventSystem eventSystem;
    private BattleManager battleManager;
    public GameObject abxy, skillMenu, itemMenu, guardMenu;

    public UnityEngine.UI.Button[] playerStation;
    public UnityEngine.UI.Button[] enemyStation;

    public UnityEngine.UI.Button[] abxyButtons; // a(attack),b(guard),x(item),y(skill) 순서
    public UnityEngine.UI.Button[] skillButtons; // skill_1, skill_2, skill_3, skill_4, back 순서
    public UnityEngine.UI.Button[] itemButtons; // item_1, item_2, item_3, back 순서 
    public UnityEngine.UI.Button[] guardButtons; // o,x 순서

    public StationController[] playerStationController;
    public StationController[] enemyStationController;

    public TextMeshProUGUI playerName;
    public TextMeshProUGUI[] skillName;
    public TextMeshProUGUI[] skillCost;
    public TextMeshProUGUI skillInfo;
    public TextMeshProUGUI skillProperty;
    public UnityEngine.UI.Button[] skillbuttons;

    public Items items;
    public TextMeshProUGUI[] itemName;
    public TextMeshProUGUI[] itemAmount;
    public TextMeshProUGUI itemInfo;
    public TextMeshProUGUI itemProperty;
    public UnityEngine.UI.Button[] itembuttons;

    private BattlePlayerUnit[] playerUnits = new BattlePlayerUnit[6];
    private BattleEnemyUnit[] enemyUnits = new BattleEnemyUnit[4];

    private SkillData useSkill;

    public BattleUnit turnUnit;


    private void Awake()
    {
        abxy.SetActive(false);
        skillMenu.SetActive(false);
        itemMenu.SetActive(false);
        guardMenu.SetActive(false);
    }

    public void Initailize(BattleManager battleManager, CreateUnit creatUnit)
    {
        this.battleManager = battleManager;
        playerUnits = creatUnit.GetPlayerUnit();
        enemyUnits = creatUnit.GetEnemyUnit();
        playerStationController = creatUnit.GetPlayerStationController();
        enemyStationController = creatUnit.GetEnemyStationController();
    }

    /// <summary>
    /// 현재 턴을 부여받는 플레이어를 받으면 턴을 실행
    /// </summary>
    /// <param name="player"></param>
    public void TurnStart(BattleUnit turnUnit)
    {
        this.turnUnit = turnUnit;
        SkillSetting();
        ChooseAct();
    }

    private void SkillSetting()
    {
        playerName.text = turnUnit.data.unitName;

        for (int skillNum = 0; skillNum < turnUnit.data.skill.Length; skillNum++)
        {
            if (turnUnit.data.skill[skillNum] != null)
            {
                skillName[skillNum].text = turnUnit.data.skill[skillNum].skillName;
                skillCost[skillNum].text = turnUnit.data.skill[skillNum].cost.ToString() + " MP";
            }
            else
            {
                skillName[skillNum].text = "empty";
                skillCost[skillNum].text = "0 MP";
            }
        }
    }

    public void ChooseAct()
    {
        abxy.SetActive(true);
        skillMenu.SetActive(false);
        itemMenu.SetActive(false);
        guardMenu.SetActive(false);
        abxyButtons[0].Select();
    }

    public void Attack()
    {
        abxy.SetActive(false);
        useSkill = null;

        int i = 0;
        while(i<6)
        {
            if (enemyStation[i].enabled == true) break;
            i++;
        }
        enemyStation[i].Select();
    }

    public void Skill()
    {
        abxy.SetActive(false);
        skillMenu.SetActive(true);
        skillButtons[0].Select();
        useSkill = null;
    }

    public void Item()
    {
        abxy.SetActive(false);
        itemMenu.SetActive(true);
        itemButtons[0].Select();
        ChangeItemInfo(0);
    }

    public void Guard()
    {
        abxy.SetActive(false);
        guardMenu.SetActive(true);
        guardButtons[0].Select();
    }

    public void OnGuard()
    {
        turnUnit.Guard();
        battleManager.EndSmallTurn();
    }

    public void SkillSelect(int skillNumber)
    {
        useSkill = turnUnit.skillData[skillNumber];
    }

    StationController[] targetStation;
    List<BattleUnit> targetUnit = new List<BattleUnit>();
    bool isEnemyTarget = false;
    public int targetInt{ get; set; }

    /// <summary>
    /// 타겟 선택 후 클릭 시 작동
    /// </summary>
    public void SetTarget()
    {
        if (isEnemyTarget) targetStation = enemyStationController;
        else targetStation = playerStationController;

        // 타겟 유닛 설정
        for (int i = 0; i < 6; i++)
        {
            if (targetStation[i].isTarget)
            {
                if (isEnemyTarget) targetUnit.Add(enemyUnits[i]);
                else targetUnit.Add(playerUnits[i]);
            }
        }

        StartCoroutine(turnUnit.Attack(targetUnit));
        battleManager.EndSmallTurn();
    }

    /// <summary>
    /// event trigger으로 선택될 때 작동
    /// </summary>
    /// <param name="targetInt"></param>
    public void SelectTarget(int targetInt)
    {
        this.targetInt = targetInt;
        targetUnit = new List<BattleUnit>();
        for (int i = 0; i < 6; i++)
        {
            playerStationController[i].NonTarget();
            enemyStationController[i].NonTarget();
        }

        if(useSkill == null) isEnemyTarget = true;
        else
        {
            if (useSkill.isBuff)
            {
                targetStation = playerStationController;
                isEnemyTarget = false;
            }
            else
            {
                targetStation = enemyStationController;
                isEnemyTarget = true;
            }
        }

        //스킬공격
        if (useSkill != null)
        {

            // 대상 스테이션 설정
            switch (useSkill.targetType)
            {
                case TargetType.None: //대상 없음
                    {
                        break;
                    }
                case TargetType.Single: //싱글
                    {
                        targetStation[targetInt].Target();
                        break;
                    }
                case TargetType.Front: //전열
                    {
                        if (targetInt <= 2)
                        {
                            targetStation[0].Target();
                            targetStation[1].Target();
                            targetStation[2].Target();
                        }
                        else
                        {
                            targetStation[3].Target();
                            targetStation[4].Target();
                            targetStation[5].Target();
                        }
                        break;
                    }
                case TargetType.Back: //전후
                    {
                        if (targetInt <= 2)
                        {
                            targetStation[targetInt].Target();
                            targetStation[targetInt + 3].Target();
                        }
                        else
                        {
                            targetStation[targetInt].Target();
                            targetStation[targetInt - 3].Target();
                        }
                        break;
                    }
                case TargetType.Area: //광역
                    {
                        for (int i = 0; i < 6; i++)
                        {
                            targetStation[i].Target();
                        }
                        break;
                    }
            }
        }
        else { enemyStationController[targetInt].Target(); }
    }

    public List<BattlePlayerUnit> GetPlayerUnit(TargetType targetType)
    {
        List<BattlePlayerUnit> battlePlayerUnit = new List<BattlePlayerUnit>();
        switch (targetType)
        {
            case TargetType.Single:
                //int range = Scripts.Utility.WeightedRandom(20, 20, 20, 20, 20); // 나중에 살아있는 친구들만 선택하게 수정
                if(playerStationController[0].GetBattlePlayerUnit() != null) // 임시로 맨 앞에 있는 애만 공격하도록
                    battlePlayerUnit.Add(playerStationController[0].GetBattlePlayerUnit());
                break;
            case TargetType.All:
                for(int i = 0; i<6;i++)
                {
                    if (playerStationController[i].GetBattlePlayerUnit() != null)
                        battlePlayerUnit.Add(playerStationController[i].GetBattlePlayerUnit());
                }
                break;
            case TargetType.Front:
                for(int i = 0; i<3;i++)
                {
                    if (playerStationController[i].GetBattlePlayerUnit() != null)
                        battlePlayerUnit.Add(playerStationController[i].GetBattlePlayerUnit());
                }
                break;
            case TargetType.Back:
                for (int i = 3; i < 6; i++)
                {
                    if (playerStationController[i].GetBattlePlayerUnit() != null)
                        battlePlayerUnit.Add(playerStationController[i].GetBattlePlayerUnit());
                }
                break;
        }
        return battlePlayerUnit;
    }


    public void ChangeSkill_Info(int skillNumber) //스킬 선택할때 스킬 설명 보여줌
    {
        skillInfo.text = turnUnit.data.skill[skillNumber].infomation;
        skillProperty.text = turnUnit.data.skill[skillNumber].attackType.ToString();
    }
    public void ChangeItemInfo(int i)
    {
        itemInfo.text = items.items[i].infomation;
        itemProperty.text = items.items[i].type.ToString();
    }

    public void OnPlayerOutline(int outlineNumber) { if (playerUnits[outlineNumber] != null) playerUnits[outlineNumber].UpdateOutline(true); }
    public void OffPlayerOutline(int outlineNumber) { if (playerUnits[outlineNumber] != null) playerUnits[outlineNumber].UpdateOutline(false); }
    public void OnEnemyOutline(int outlineNumber) { if(enemyUnits[outlineNumber] != null) enemyUnits[outlineNumber].UpdateOutline(true); }
    public void OffEnemyOutline(int outlineNumber) { if (enemyUnits[outlineNumber] != null) enemyUnits[outlineNumber].UpdateOutline(false); }
}