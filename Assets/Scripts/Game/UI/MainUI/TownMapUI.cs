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
using System;
using UnityEngine.Events;
using System.Runtime.CompilerServices;
using static Scripts.Manager.UIManager;

public class TownMapUI : MonoBehaviour
{

    private string selectedButtonName;
    public GameObject mainMenuCanvas;

    //public TextMeshProUGUI topPanelText, buttonDescriptionText;

    public Image dungeonCircle, guildCircle, shopCircle;
    private Color red = new(1, 0, 0, 0.5f), yellow = new(1, 1, 0, 0.5f);

    private void Awake()
    {
        UIStack.Instance.PushUI(gameObject);
    }

    public void OnEnable()
    {
        UIManager.Menu += Menu;
        UIManager.Navigate += Navigate;
    }

    public void OnDisable()
    {
        UIManager.Menu -= Menu;
        UIManager.Navigate -= Navigate;
    }


    private void Menu()
    {
        UIStack.Instance.PushUI(mainMenuCanvas);
    }

    private void Navigate()
    {
        selectedButtonName = UIManager.Instance.GetSelectedButtonName();
        switch (selectedButtonName)
        {
            case "Dungeon":
                dungeonCircle.color = yellow;
                guildCircle.color = red;
                shopCircle.color = red; break;
            case "Guild":
                dungeonCircle.color = red;
                guildCircle.color = yellow;
                shopCircle.color = red; break;
            case "Shop":
                dungeonCircle.color = red;
                guildCircle.color = red;
                shopCircle.color = yellow; break;
        }

    }

}
