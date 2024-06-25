using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;

public class ActMenu : MonoBehaviour
{
    public enum Acting { Attack, Skill, Guard, Item }
    public enum ActState { Waiting , ChooseAct, SetUpAttack, SetUpSkill, SkillTarget, SetUpItem, ItemTarget, SetUpGuard, EndTurn }

    public EventSystem eventSystem;
    public GameObject ActCanvas;
    public GameObject abxy, skillmenu, itemmenu, guardmenu;
    public UnityEngine.UI.Button[] PlayerStation;

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
 
    public ActState aState;

    private Unit[] playerUnits = new Unit[6];
    private Unit[] enemyUnits = new Unit[4];

    private BattleSkill[] playerskills = new BattleSkill[4];

    private int turnPlayerNum;
    private Unit turnPlayerUnit;

    private void Awake()
    {
        abxy.SetActive(false);
        skillmenu.SetActive(false);
        itemmenu.SetActive(false);
        guardmenu.SetActive(false);
        
    }

    /// <summary>
    /// 현재 턴을 부여받는 플레이어를 받으면 턴을 실행
    /// </summary>
    /// <param name="player"></param>
    public void TurnStart(int playerNum)
    {
        turnPlayerNum = playerNum;
        turnPlayerUnit = playerUnits[playerNum];
        playerskills = turnPlayerUnit.skills;
        //abxy.transform.position = player.transform.position;

        ChooseAct();
    }

    public void GetUnits(Unit[] playerunits, Unit[] enemyunits)
    {
        playerUnits = playerunits;
        enemyUnits = enemyunits;
    }

    private void ChooseAct()
    {
        abxy.SetActive(true);
        skillmenu.SetActive(false);
        itemmenu.SetActive(false);
        guardmenu.SetActive(false);
    }

    public void SetUpSkill()
    {
        Debug.Log("스킬메뉴");

        playername.text = turnPlayerUnit.unitName;

        int skillNumber = 0;
        while (playerskills[skillNumber] != null)
        {
            skillname[skillNumber].text = playerskills[skillNumber].Name;
            skillcost[skillNumber].text = playerskills[skillNumber].Cost.ToString() + "MP";
            skillNumber++;
        }
    }

    /// <summary>
    /// 턴 부여받은 유닛의 스킬로 바꿈
    /// </summary>
    /// <param name="i"></param>
    public void ChangeSkill_Info(int skillNumber)
    {
        skill_info.text = playerskills[skillNumber].Infomation;
        skill_property.text = playerskills[skillNumber].Property;
    }

    /// <summary>
    /// 공격 스킬이면 상대를 타겟으로 정하고 아니면 우리팀을 타겟으로 정함
    /// </summary>
    /// <param name="skill_ID"></param>
    public void Skill_Type_check(int skill_ID)
    {   
        for(int i = 0; i<5; i++) { PlayerStation[i].interactable = true; }
        if (playerskills[skill_ID].isAttack) {}
        else { PlayerStation[0].Select();}
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