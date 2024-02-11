using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ActMenu : MonoBehaviour
{
    public enum Acting {Attack,Skill,Guard,Item}
    private enum ActState {ChooseAct, SetUpAttack, SetUpSkill, SkillTarget, SetUpItem, ItemTarget, Guard, EndTurn }

    public GameObject ActCanvas;
    public GameObject abxy, skillmenu, itemmenu;
    public Image[] skillnamePanelImage = new Image[4];

    private ActState aState;

    private int currenttarget, currentskill;
    private bool isChooseActworked;

    
    private void Awake()
    {
        isChooseActworked = false;
        
        abxy.SetActive(false);
        skillmenu.SetActive(false);
        itemmenu.SetActive(false);
    }

    private void Update()
    {
        if(aState == ActState.ChooseAct)
        {
            if (!isChooseActworked) { ChooseAct(); isChooseActworked = true; }

            if(Input.GetKeyDown(KeyCode.A))
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
                aState = ActState.Guard;
                Debug.Log("Press B");

                Guard();
            }
        }

        if(aState == ActState .SetUpAttack)
        {
            if(Input.GetKeyDown(KeyCode.UpArrow))
            {
                currenttarget ++;
                if (currenttarget >= 5) {currenttarget--;}

                Debug.Log(currenttarget);
            }

            if(Input.GetKeyDown(KeyCode.DownArrow))
            {
                currenttarget --;
                if(currenttarget<0){ currenttarget++;}

                Debug.Log(currenttarget);
            }

            if(Input.GetKeyDown(KeyCode.Return))
            {
                aState = ActState.EndTurn;

                Debug.Log("목표: " + currenttarget);
            }

            if(Input.GetKeyDown(KeyCode.Escape))
            {
                aState = ActState.ChooseAct;
                ChooseAct();
            }
        }

        if (aState == ActState.SetUpSkill)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                currentskill++;
                if (currentskill >= 4) { currentskill--; }
                ChageSkillColor();

                Debug.Log(currentskill);
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                currentskill--;
                if (currentskill <= -1) { currentskill++; }
                ChageSkillColor();

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

        if(aState == ActState.SkillTarget)
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
    }

    private void ChooseAct()
    {
        abxy.SetActive(true);
        skillmenu.SetActive(false);
        itemmenu.SetActive(false);
    }

    public void SetUpAttack()
    {
        abxy.SetActive(false);
        skillmenu.SetActive(false);
        itemmenu.SetActive(false);
        currenttarget = 0;
    }
    
    public void SetUpSkill()
    {
        Debug.Log("스킬메뉴");
        abxy.SetActive(false);
        skillmenu.SetActive(true);
        itemmenu.SetActive(false);

        ChageSkillColor();
        StartCoroutine(SkillMenuAnimation());
    }

    public void ChageSkillColor()
    {
        Color selectColor = Color.yellow;
        Color nonselectColor = Color.white;

        for (int i = 0; i <=3; i++)
        {
            skillnamePanelImage[i].color = nonselectColor;
        }
        skillnamePanelImage[currentskill].color = selectColor;
    }

    public void SetUpItem()
    {
        Debug.Log("아이템");
    }

    public void Guard()
    {
        Debug.Log("가드");
    }




    IEnumerator SkillMenuAnimation()
    {
        RectTransform skillRectTransform = skillmenu.GetComponent<RectTransform>();

        Vector2 startPosition = skillRectTransform.anchoredPosition;
        Vector2 targetPosition = new Vector2(-500f, 0f);

        float duration = 0.5f; // 애니메이션 지속 시간 설정
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            skillRectTransform.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 애니메이션 완료 후 최종 위치 설정
        skillRectTransform.anchoredPosition = targetPosition;
        yield break;
    }
}
