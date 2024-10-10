using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using Scripts.Data;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;
using Unity.VisualScripting;

public class ActMenu : MonoBehaviour
{
    private EventSystem eventSystem;
    UnitSpawn unitSpawn;
    private BattleManager battleManager;
    public GameObject abxy, skillMenu, itemMenu, guardMenu;

    public UnityEngine.UI.Button[] playerStation;
    public UnityEngine.UI.Button[] enemyStation;

    public UnityEngine.UI.Button[] abxyButtons; // a(attack),b(guard),x(item),y(skill) 순서
    public UnityEngine.UI.Button[] skillButtons; // skill_1, skill_2, skill_3, skill_4, back 순서
    public UnityEngine.UI.Button[] itemButtons; // item_1, item_2, item_3, back 순서 
    public UnityEngine.UI.Button[] guardButtons; // o,x 순서

    public StationController[] playerStationController = new StationController[6];
    public StationController[] enemyStationController = new StationController[6];

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

    private SkillData useSkill;
    private Unit targetUnit;

    public Unit turnPlayerUnit;

    private void Awake()
    {
        abxy.SetActive(false);
        skillMenu.SetActive(false);
        itemMenu.SetActive(false);
        guardMenu.SetActive(false);
    }

    public void SetUp(BattleManager battleManager, UnitSpawn unitSpawn)
    {
        this.battleManager = battleManager;
        this.unitSpawn = unitSpawn;
        playerUnits = unitSpawn.GetPlayerUnit();
        enemyUnits = unitSpawn.GetEnemyUnit();
        playerStationController = unitSpawn.GetPlayerStationController();
        enemyStationController = unitSpawn.GetEnemyStationController();
    }

    /// <summary>
    /// 현재 턴을 부여받는 플레이어를 받으면 턴을 실행
    /// </summary>
    /// <param name="player"></param>
    public void TurnStart(Unit turnUnit)
    {
        turnPlayerUnit = turnUnit;
        SkillSetting();
        ChooseAct();
    }

    private void SkillSetting()
    {
        playername.text = turnPlayerUnit.unitName;

        for (int skillNum = 0; skillNum < 4; skillNum++)
        {
            if (turnPlayerUnit.skills[skillNum] != null)
            {
                skillname[skillNum].text = turnPlayerUnit.skills[skillNum].skillName;
                //skillcost[skillNum].text = turnPlayerUnit.skills[skillNum].mpCost.GetMpCost(turnPlayerUnit.skills[skillNum]).ToString() + "MP";
            }
            else
            {
                skillname[skillNum].text = "empty";
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
        turnPlayerUnit.OnGuard();
    }

    public void Back()
    {
        abxy.SetActive(true);
        skillMenu.SetActive(false);
        itemMenu.SetActive(false);
        guardMenu.SetActive(false);
        abxyButtons[0].Select();
    }

    public void SkillSelect(int skillNumber)
    {
        useSkill = turnPlayerUnit.skills[skillNumber];
        if(useSkill.isBuff || useSkill.isHealing)
        {
            int i = 0;
            while (i < 6)
            {
                if (playerStation[i].enabled == true) break;
                i++;
            }
            playerStation[i].Select();
        }
        else
        {
            int i = 0;
            while (i < 6)
            {
                if (enemyStation[i].enabled == true) break;
                i++;
            }
            enemyStation[i].Select();
        }
        skillMenu.SetActive(false);
    }

    public void SetTarget()
    {
        for(int i = 0; i < 6; i++)
        {
            if (enemyStationController[i].isTarget == true)
            {
                Debug.Log(i + "->  타겟 ");
                enemyStationController[i].isTarget = false;
                targetUnit = enemyUnits[i];
            }
        }

        if(useSkill == null)
        {
            turnPlayerUnit.Attack(targetUnit);
        }
        else
        {
            turnPlayerUnit.Attack(targetUnit, useSkill);
        }

        battleManager.EndSmallTurn();
        abxyButtons[0].Select();
    }

    public void ChangeSkill_Info(int skillNumber) //스킬 선택할때 스킬 설명 보여줌
    {
        skill_info.text = turnPlayerUnit.skills[skillNumber].infomation;
        skill_property.text = turnPlayerUnit.skills[skillNumber].attackType.ToString();
    }
    public void ChangeItemInfo(int i)
    {
        item_info.text = items.items[i].infomation;
        item_property.text = items.items[i].type.ToString();
    }

    public void OnPlayerOutline(int outlineNumber) { if (playerUnits[outlineNumber] != null) playerUnits[outlineNumber].UpdateOutline(true); }
    public void OffPlayerOutline(int outlineNumber) { if (playerUnits[outlineNumber] != null) playerUnits[outlineNumber].UpdateOutline(false); }
    public void OnEnemyOutline(int outlineNumber) { if(enemyUnits[outlineNumber] != null) enemyUnits[outlineNumber].UpdateOutline(true); }
    public void OffEnemyOutline(int outlineNumber) { if (enemyUnits[outlineNumber] != null) enemyUnits[outlineNumber].UpdateOutline(false); }

    
}