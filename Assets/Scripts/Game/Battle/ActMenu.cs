using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using Scripts.Data;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;

public class ActMenu : MonoBehaviour
{
    public EventSystem eventSystem;
    private BattleManager battleManager;
    public GameObject ActCanvas;
    public GameObject abxy, skillmenu, itemmenu, guardmenu;

    public UnityEngine.UI.Button[] playerStation;
    public UnityEngine.UI.Button[] enemyStation;

    public UnityEngine.UI.Button[] abxyButtons; // a,b,x,y 순서
    public UnityEngine.UI.Button[] skillButtons; // skill_1, skill_2, skill_3, skill_4, back 순서
    public UnityEngine.UI.Button[] itemButtons; // item_1, item_2, item_3, back 순서 
    public UnityEngine.UI.Button[] guardButtons; // o,x 순서

    public TextMeshProUGUI playername;

    public TextMeshProUGUI[] skillname;
    public TextMeshProUGUI[] skillcost;
    public TextMeshProUGUI skill_info;
    public TextMeshProUGUI skill_property;
    public UnityEngine.UI.Button[] skillbuttons;

    public Items items;
    public GameObject[] itemamountPanel;
    public TextMeshProUGUI item_info;
    public TextMeshProUGUI item_property;
    public UnityEngine.UI.Button[] itembuttons;

    private Unit[] playerUnits = new Unit[6];
    private Unit[] enemyUnits = new Unit[4];

    private SkillData[] playerSkills;
    private SkillData useSkill;
    private Unit targetUnit;

    private int turnPlayerNum;
    private Unit turnPlayerUnit;
    private bool isSkill;

    private void Awake()
    {
        abxy.SetActive(false);
        skillmenu.SetActive(false);
        itemmenu.SetActive(false);
        guardmenu.SetActive(false);
        
    }

    public void SetBM(BattleManager battleManager)
    {
        this.battleManager = battleManager;
    }
    public void SetUnits(Unit[] playerunits, Unit[] enemyunits)
    {
        playerUnits = playerunits;
        enemyUnits = enemyunits;
    }

    /// <summary>
    /// 현재 턴을 부여받는 플레이어를 받으면 턴을 실행
    /// </summary>
    /// <param name="player"></param>
    public void TurnStart(int playerNum)
    {
        turnPlayerNum = playerNum;
        turnPlayerUnit = playerUnits[playerNum];
        playerSkills = turnPlayerUnit.skills;
        InitialSetting();
        ChooseAct();
    }

    private void InitialSetting()
    {
        playername.text = turnPlayerUnit.unitName;


        for (int skillNum = 0; skillNum < 4; skillNum++)
        {
            skillname[skillNum].text = playerSkills[skillNum].skillName;
            skillcost[skillNum].text = playerSkills[skillNum].mpCost.ToString() + "MP";
        }
    }

    public void ChooseAct()
    {
        abxy.SetActive(true);
        skillmenu.SetActive(false);
        itemmenu.SetActive(false);
        guardmenu.SetActive(false);
        isSkill = false;
        abxyButtons[0].Select();
    }

    public void Attack()
    {
        abxy.SetActive(false);
        enemyStation[0].Select();
    }

    public void Skill()
    {
        abxy.SetActive(false);
        skillmenu.SetActive(true);
        skillButtons[0].Select();
        ChangeSkill_Info(0);
    }

    public void SkillSelect(int skillNumber)
    {
        useSkill = playerSkills[skillNumber];
        if (turnPlayerUnit.enoughMP(useSkill.mpCost))
        {
            skillmenu.SetActive(false);
            isSkill = true;
            enemyStation[0].Select();
        }
        else
        {
            Debug.Log("마나가 부족합니다");
        }
    }

    public void SetTarget(int targetNum)
    {
        targetUnit = enemyUnits[targetNum];

        if (isSkill)
        {
            turnPlayerUnit.ConsumeMP(useSkill.mpCost);
            battleManager.Attack(turnPlayerUnit, targetUnit, useSkill);
        }
        else
        {
            battleManager.Attack(turnPlayerUnit, targetUnit, useSkill);
        }

        battleManager.EndSmallTurn();
        abxyButtons[0].Select();
    }

    public void ChangeSkill_Info(int skillNumber) //스킬 선택할때 스킬 설명 보여줌
    {
        skill_info.text = playerSkills[skillNumber].infomation;
        skill_property.text = playerSkills[skillNumber].attackType.ToString();
    }
    public void OnPlayerOutline(int outlineNumber) { if (playerUnits[outlineNumber] != null) playerUnits[outlineNumber].UpdateOutline(true); }
    public void OffPlayerOutline(int outlineNumber) { if (playerUnits[outlineNumber] != null) playerUnits[outlineNumber].UpdateOutline(false); }
    public void OnEnemyOutline(int outlineNumber) { if(enemyUnits[outlineNumber] != null) enemyUnits[outlineNumber].UpdateOutline(true); }
    public void OffEnemyOutline(int outlineNumber) { if (enemyUnits[outlineNumber] != null) enemyUnits[outlineNumber].UpdateOutline(false); }

    public void ChangeItemInfo(int i)
    {
        item_info.text = items.items[i].infomation;
        item_property.text = items.items[i].type.ToString();
    }
}