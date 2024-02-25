using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ActMenu : MonoBehaviour
{
    public enum Acting { Attack, Skill, Guard, Item }
    public enum ActState { Waiting, ChooseAct, SetUpAttack, SetUpSkill, SkillTarget, SetUpItem, ItemTarget, SetUpGuard, EndTurn }

    public GameObject ActCanvas;
    public GameObject abxy, skillmenu, itemmenu, guardmenu;

    public TextMeshProUGUI[] skillname = new TextMeshProUGUI[4];
    public TextMeshProUGUI[] skillcost = new TextMeshProUGUI[4];
    public TextMeshProUGUI skill_Info;
    public TextMeshProUGUI skillProperty;

    public GameObject[] itemamountPanel = new GameObject[3];

    public ActState aState;

    private Unit[] units = new Unit[6];
    private SpriteOutline[] outlines = new SpriteOutline[6];

    private Unit turnunit;
    private SpriteOutline turnoutline;
    private BattleSkill[] battleSkills = new BattleSkill[4];

    private int currenttarget, currentskill, currentitem;

    private Unit turnplayer;

    private void Awake()
    {
        abxy.SetActive(false);
        skillmenu.SetActive(false);
        itemmenu.SetActive(false);
        guardmenu.SetActive(false);
    }

    public void TurnStart(Unit player)
    {
        turnplayer = player;
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

    public void SetUpAttack()
    {
        abxy.SetActive(false);
        skillmenu.SetActive(false);
        itemmenu.SetActive(false);
        guardmenu.SetActive(false);

        currenttarget = 0;
    }

    public void SetUpSkill()
    {
        currenttarget = 0;

        Debug.Log("스킬메뉴");
        abxy.SetActive(false);
        skillmenu.SetActive(true);
        itemmenu.SetActive(false);
        guardmenu.SetActive(false);

        for (int i = 0; i <= 3; i++)
        {
            skillname[i].text = battleSkills[i].Name;
            skillcost[i].text = battleSkills[i].Cost.ToString() + "MP";
        }
    }

    public void ChangeSkill()
    {
        Color selectColor = Color.yellow;
        Color nonselectColor = Color.white;

        skill_Info.text = battleSkills[currentskill].Infomation;
        skillProperty.text = battleSkills[currentskill].Property;
    }

    public void ChangePlayerOutline()
    {
        for(int i = 0; i <= 4; i++)
        {
            outlines[i].OffOutline();
        }
        outlines[currenttarget].OnOutline();
    }

    public void SetUpGuard()
    {
        Debug.Log("가드");
        abxy.SetActive(false);
        skillmenu.SetActive(false);
        itemmenu.SetActive(false);
        guardmenu.SetActive(true);
    }
}
