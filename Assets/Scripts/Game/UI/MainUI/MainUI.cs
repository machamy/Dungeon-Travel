using Scripts.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainUI : MonoBehaviour
{

    public delegate void menu();
    public static event menu Menu;
    public delegate void cancel();
    public static event cancel Cancel;   

    public void OnMenu() => Menu();
    public void OnCancel() => Cancel();
    
    void Update()
    {
        //buttonDescriptionText.text = UIManager.Instance.GetSelectedButtonDescription();
        //itemDescriptionText.text = UIManager.Instance.GetSelectedItemDescription();
    }
}
