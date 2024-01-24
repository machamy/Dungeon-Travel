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
        Debug.Log("����");
    }
    
    public void Skill()
    {
        Debug.Log("��ų");
    }

    public void Item()
    {
        Debug.Log("������");
    }

    public void Pass()
    {
        Debug.Log("�н�");
    }
}
