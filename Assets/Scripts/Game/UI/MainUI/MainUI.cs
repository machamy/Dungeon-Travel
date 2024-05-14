using Scripts.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainUI : MonoBehaviour
{

    public GameObject townMapCanvas, mainMenuCanvas;

    public void Awake()
    {
        UIStack.Instance.PushUI(townMapCanvas);
    }

    public void OnMenu()
    {
        if (UIStack.Instance.menuStack.Count == 1)
            UIStack.Instance.PushUI(mainMenuCanvas);
    }


    public void OnCancel()
    {
        UIStack.Instance.PopUI();
    }

    
    void Update()
    {
        //buttonDescriptionText.text = UIManager.Instance.GetSelectedButtonDescription();
        //itemDescriptionText.text = UIManager.Instance.GetSelectedItemDescription();
    }
}
