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
    public enum ActState { Waiting, ChooseAct, SetUpAttack, SetUpSkill, SkillTarget, SetUpItem, ItemTarget, SetUpGuard, EndTurn }

    public GameObject ActCanvas;
    public GameObject abxy, skillmenu, itemmenu, guardmenu;

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

    private Unit[] units = new Unit[6];
    private SpriteOutline[] outlines = new SpriteOutline[6];

    private Unit turnunit;
    private SpriteOutline turnoutline;
    private BattleSkill[] playerskills = new BattleSkill[4];

    private int currenttarget, currentskill, currentitem;

    private Unit turnplayer;

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
    public void TurnStart(Unit player)
    {
        turnplayer = player;
        playerskills = player.skills;
        abxy.transform.position = player.transform.position;

        ChooseAct();
    }

    public void GetUnitComp(Unit[] getunits, SpriteOutline[] getoutlines)
    {
        units = getunits;
        outlines = getoutlines;

        if (outlines[0] == null) { Debug.Log("null"); }
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

        playername.text = turnplayer.unitName;

        for (int i = 0; i <= 3; i++)
        {
            skillname[i].text = playerskills[i].Name;
            skillcost[i].text = playerskills[i].Cost.ToString() + "MP";
        }
    }
    public void ChangeSkill_Info(int i)
    {
        skill_info.text = playerskills[i].Infomation;
        skill_property.text = playerskills[i].Property;
    }

    public void ChangePlayerOutline()
    {
        for(int i = 0; i <= 4; i++)
        {
            outlines[i].OffOutline();
        }
        outlines[currenttarget].OnOutline();
    }

    public void ChangeItemInfo(int i)
    {
        item_info.text = items.items[i].infomation;
        item_property.text = items.items[i].type.ToString();
    }
}
