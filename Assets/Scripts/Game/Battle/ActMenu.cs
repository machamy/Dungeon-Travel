using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
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

    private BattleSkill[] playerSkills = new BattleSkill[4];
    private BattleSkill useSkill;
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

        int skillNumber = 0;
        while (playerSkills[skillNumber] != null)
        {
            skillname[skillNumber].text = playerSkills[skillNumber].Name;
            skillcost[skillNumber].text = playerSkills[skillNumber].Cost.ToString() + "MP";
            skillNumber++;
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

    public void AttackTarget(int targetNumber)
    {
        targetUnit = enemyUnits[targetNumber];
        battleManager.Attack(targetUnit, 5, 0);
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
        skillmenu.SetActive(false);
        useSkill = playerSkills[skillNumber];
        if (turnPlayerUnit.enoughMP(useSkill.Cost))
        {
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
            turnPlayerUnit.ConsumeMP(useSkill.Cost);
            battleManager.Attack(targetUnit, 10f, 0);
        }
        else
        {
            battleManager.Attack(targetUnit,5f,0);
        }

        battleManager.EndSmallTurn();
        abxyButtons[0].Select();
    }

    public void ChangeSkill_Info(int skillNumber) //스킬 선택할때 스킬 설명 보여줌
    {
        skill_info.text = playerSkills[skillNumber].Infomation;
        skill_property.text = playerSkills[skillNumber].Property;
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