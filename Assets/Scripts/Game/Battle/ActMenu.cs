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

    public Image[] skillnamePanelImage = new Image[4];
    public TextMeshProUGUI[] skillname = new TextMeshProUGUI[4];
    public TextMeshProUGUI[] skillcost = new TextMeshProUGUI[4];
    public TextMeshProUGUI skill_Info;
    public TextMeshProUGUI skillProperty;

    public Image[] itemnamePanelImage = new Image[3];
    public GameObject[] itemamountPanel = new GameObject[3];

    private bool guard_OX;
    public TextMeshProUGUI guard_O, guard_X;

    public ActState aState;

    private Unit[] units = new Unit[6];
    private SpriteOutline[] outlines = new SpriteOutline[6];

    private Unit turnunit;
    private SpriteOutline turnoutline;
    private BattleSkill[] battleSkills = new BattleSkill[4];

    private int currenttarget, currentskill, currentitem;
    private bool isChooseActworked;

    private int currentturn = 0;

    private void Awake()
    {
        isChooseActworked = false;
        guard_OX = true;

        abxy.SetActive(false);
        skillmenu.SetActive(false);
        itemmenu.SetActive(false);
        guardmenu.SetActive(false);
    }

    public void GetUnitComp(Unit[] getunits, SpriteOutline[] getoutlines)
    {
        units = getunits;
        outlines = getoutlines;
        if (outlines[0] == null) { Debug.Log("null"); }

        battleSkills = units[currentturn].skills;
    }

    private void Update()
    {
        if (aState == ActState.ChooseAct)
        {
            if (!isChooseActworked) { ChooseAct(); isChooseActworked = true; }

            if (Input.GetKeyDown(KeyCode.A))
            {
                aState = ActState.SetUpAttack;
                Debug.Log("Press A");

                SetUpAttack();
            }

            if (Input.GetKeyDown(KeyCode.Y))
            {
                aState = ActState.SetUpSkill;
                Debug.Log("Press Y");

                SetUpSkill();
            }

            if (Input.GetKeyDown(KeyCode.X))
            {
                aState = ActState.SetUpItem;
                Debug.Log("Press X");

                SetUpItem();
            }

            if (Input.GetKeyDown(KeyCode.B))
            {
                aState = ActState.SetUpGuard;
                Debug.Log("Press B");

                SetUpGuard();
            }
        }

        if (aState == ActState.SetUpAttack)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                currenttarget++;
                if (currenttarget >= 5) { currenttarget--; }

                Debug.Log(currenttarget);
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                currenttarget--;
                if (currenttarget < 0) { currenttarget++; }

                Debug.Log(currenttarget);
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {
                aState = ActState.EndTurn;

                Debug.Log("목표: " + currenttarget);
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                aState = ActState.ChooseAct;
                ChooseAct();
            }
        }

        if (aState == ActState.SetUpSkill)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                currentskill++;
                if (currentskill >= 4) { currentskill--; }
                ChangeSkill();

                Debug.Log(currentskill);
            }

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                currentskill--;
                if (currentskill <= -1) { currentskill++; }
                ChangeSkill();

                Debug.Log(currentskill);
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {
                aState = ActState.SkillTarget;

                Debug.Log("스킬 " + currentskill);
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                aState = ActState.ChooseAct;
                ChooseAct();
            }
        }

        if (aState == ActState.SkillTarget)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                currenttarget++;
                if (currenttarget >= 5) { currenttarget--; }

                Debug.Log(currenttarget);
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                currenttarget--;
                if (currenttarget < 0) { currenttarget++; }

                Debug.Log(currenttarget);
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {
                aState = ActState.EndTurn;

                Debug.Log("목표: " + currenttarget);
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                aState = ActState.SetUpSkill;
                SetUpSkill();
            }
        }

        if (aState == ActState.SetUpItem)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                currentitem++;
                if (currentitem >= 3) { currentitem--; }
                ChangeItemColor();

                Debug.Log(currentitem);
            }

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                currentitem--;
                if (currentitem <= -1) { currentitem++; }
                ChangeItemColor();

                Debug.Log(currentitem);
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {

                

                Debug.Log("아이템 " + currentitem);

                ChangePlayerOutline();
                itemmenu.SetActive(false);

                aState = ActState.ItemTarget;
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                aState = ActState.ChooseAct;
                ChooseAct();
            }
        }

        if(aState == ActState.ItemTarget)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                currenttarget++;
                if (currenttarget >= 5) { currenttarget--; }

                ChangePlayerOutline();
                Debug.Log(currenttarget);
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                currenttarget--;
                if (currenttarget < 0) { currenttarget++; }

                ChangePlayerOutline();
                Debug.Log(currenttarget);
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {
                aState = ActState.EndTurn;

                Debug.Log("목표: " + currenttarget);
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                itemmenu.SetActive(true);
                aState = ActState.SetUpItem;
                SetUpItem();
                
            }
        }

        if(aState == ActState.SetUpGuard)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                guard_OX = !guard_OX;
                ChangeGuardColor();
                Debug.Log(guard_OX);
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                guard_OX = !guard_OX;
                ChangeGuardColor();
                Debug.Log(guard_OX);
            }
            if(Input.GetKeyDown(KeyCode.Return))
            {
                if(guard_OX)
                {
                   Debug.Log(guard_OX);
                   units[currentturn].isguard = guard_OX;

                    aState = ActState.EndTurn;
                }
                else
                {
                    aState = ActState.ChooseAct;
                    ChooseAct();
                }
            }
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                aState = ActState.ChooseAct;
                ChooseAct();
            }
        }
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

        ChangeSkill();
        StartCoroutine(ApearMenuAnimation(skillmenu));
    }

    public void ChangeSkill()
    {
        Color selectColor = Color.yellow;
        Color nonselectColor = Color.white;

        for (int i = 0; i <= 3; i++)
        {
            skillnamePanelImage[i].color = nonselectColor;
        }

        skillnamePanelImage[currentskill].color = selectColor;
        skill_Info.text = battleSkills[currentskill].Infomation;
        skillProperty.text = battleSkills[currentskill].Property;
    }

    public void SetUpItem()
    {
        currenttarget = 0;

        Debug.Log("아이템");
        abxy.SetActive(false);
        skillmenu.SetActive(false);
        itemmenu.SetActive(true);
        guardmenu.SetActive(false);

        ChangeItemColor();
        StartCoroutine(ApearMenuAnimation(itemmenu));
    }
    
    public void ChangeItemColor()
    {
        Color selectColor = Color.yellow;
        Color nonselectColor = Color.white;

        for (int i = 0; i <= 2; i++)
        {
            itemnamePanelImage[i].color = nonselectColor;
            itemamountPanel[i].SetActive(false);
        }
        itemnamePanelImage[currentitem].color = selectColor;
        itemamountPanel[currentitem].SetActive(true);
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

        ChangeGuardColor();
    }

    public void ChangeGuardColor()
    {
        if(guard_OX)
        {
            guard_O.fontStyle = FontStyles.Bold;
            guard_O.color = Color.yellow;

            guard_X.fontStyle = FontStyles.Normal;
            guard_X.color = Color.red;
            
        }
        else
        {
            guard_O.fontStyle = FontStyles.Normal;
            guard_O.color = Color.red;

            guard_X.fontStyle = FontStyles.Bold;
            guard_X.color = Color.yellow;
        }
    }




    IEnumerator ApearMenuAnimation(GameObject menu)
    {
        RectTransform ObjRectTransform = menu.GetComponent<RectTransform>();

        Vector2 startPosition = ObjRectTransform.anchoredPosition;
        Vector2 targetPosition = new Vector2(-500f, 0f);

        float duration = 0.5f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            ObjRectTransform.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        ObjRectTransform.anchoredPosition = targetPosition;
        yield break;
    }
}
