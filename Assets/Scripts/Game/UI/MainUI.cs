using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;
using Scripts.Manager;

public class MainUI : MonoBehaviour
{
    public GameObject placeFirstSelect;

    string selectedButtonName = "DungeonCircle";

    public GameObject dungeonCircle, guildCircle, shopCircle;
    private Color red = new(1, 0, 0, 0.5f), yellow = new(1, 1, 0, 0.5f);

    public InputActionReference mainNavigation, yNavigation;

    public void SelectPlace()
    {
        UIManager.Instance.SetUI(UIManager.State.SelectPlace,
            null, null, placeFirstSelect, mainNavigation);
    }

    public void OnCancel()
    {
        switch (UIManager.Instance.currentState)
        {
            
        }
    }

    public void OnSubmit()
    {
        switch (UIManager.Instance.currentState)
        {
            
        }
    }

    public void OnClick()
    {
        switch (UIManager.Instance.currentState)
        {
            
        }
    }

    private void Update()
    {
        selectedButtonName = UIManager.Instance.GetSelectedButtonName();
        switch (selectedButtonName)
        {
            case "Dungeon":
                dungeonCircle.GetComponent<Image>().color = yellow;
                guildCircle.GetComponent<Image>().color = red;
                shopCircle.GetComponent<Image>().color = red; break;
            case "Guild":
                dungeonCircle.GetComponent<Image>().color = red;
                guildCircle.GetComponent<Image>().color = yellow;
                shopCircle.GetComponent<Image>().color = red; break;
            case "Shop":
                dungeonCircle.GetComponent<Image>().color = red;
                guildCircle.GetComponent<Image>().color = red;
                shopCircle.GetComponent<Image>().color = yellow; break;
        }
    }

}
