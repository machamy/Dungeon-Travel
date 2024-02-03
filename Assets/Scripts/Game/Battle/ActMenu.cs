using System.Collections;
using UnityEngine;

public class ActMenu : MonoBehaviour
{
    public enum Acting {Attack,Skill,Guard,Item}
    private enum ActState {ChoseAct, SetUpAttack, SetUpSkill, SkillTarget, SetUpItem, ItemTarget, Guard, EndTurn }

    public GameObject ActCanvas;
    public GameObject abxy, skillmenu, itemmenu;

    private ActState aState;

    private int currenttarget, currentskill;
    private bool isChoseActworked;

    private void Awake()
    {
        isChoseActworked = false;

        abxy.SetActive(false);
        skillmenu.SetActive(false);
        itemmenu.SetActive(false);

        aState = ActState.ChoseAct;
    }

    private void Update()
    {
        if(aState == ActState.ChoseAct)
        {
            if (!isChoseActworked) { ChoseAct(); isChoseActworked = true; }

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
                aState = ActState.ChoseAct;
                ChoseAct();
            }
        }

        if (aState == ActState.SetUpSkill)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                currentskill++;
                if (currentskill >= 4) { currentskill--; }

                Debug.Log(currentskill);
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                currentskill--;
                if (currentskill < 1) { currentskill++; }

                Debug.Log(currentskill);
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {
                aState = ActState.EndTurn;

                Debug.Log("목표: " + currentskill);
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                aState = ActState.ChoseAct;
                ChoseAct();
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

    private void ChoseAct()
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

        StartCoroutine(SkillMenuAnimation());
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
