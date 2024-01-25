using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActMenu : MonoBehaviour
{
    private GameObject ActMenuGO;
    public GameObject abxy, skillmenu, itemmenu;

    private void Awake()
    {
        ActMenuGO = this.gameObject;

        abxy.SetActive(true);
        skillmenu.SetActive(false);
        itemmenu.SetActive(false);
    }

    public void Attack()
    {
        Debug.Log("공격");
    }
    
    public void Skill()
    {
        Debug.Log("스킬");
    }

    public void Item()
    {
        Debug.Log("아이템");
    }

    public void Pass()
    {
        Debug.Log("패스");
    }
}
