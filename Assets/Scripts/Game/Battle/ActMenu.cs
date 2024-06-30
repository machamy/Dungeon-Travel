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
        abxyButtons[0].Select();
    }

    public void Skill()
    {
        abxy.SetActive(false);
        skillmenu.SetActive(true);
        skillButtons[0].Select();
        ChangeSkill_Info(0);
    }

    /// <summary>
    /// 턴 부여받은 유닛의 스킬로 바꿈
    /// </summary>
    /// <param name="i"></param>
    public void ChangeSkill_Info(int skillNumber)
    {
        skill_info.text = playerSkills[skillNumber].Infomation;
        skill_property.text = playerSkills[skillNumber].Property;
    }

    public void SkillSelect(int skillNumber)
    {
        skillmenu.SetActive(false);
        useSkill = playerSkills[skillNumber];
        enemyStation[0].Select();
    }

    public void SkillTarget(int targetNumber)
    {
        targetUnit = enemyUnits[targetNumber];
    }

    public void ChooseTarget(int setTarget)
    {
        // 0~5번은 플레이어 유닛, 100번부터 103은 적 유닛
        if (setTarget >= 100)
            targetUnit = enemyUnits[setTarget - 100];
        else
            targetUnit = playerUnits[setTarget];

        battleManager.Attack2(targetUnit,useSkill);
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