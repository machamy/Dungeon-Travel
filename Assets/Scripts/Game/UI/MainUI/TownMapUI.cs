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

    public GameObject[] placeSelectGroup = new GameObject[3]; //Dungeon, Guild, Shop
    private Color red = new(1, 0, 0, 0.5f), yellow = new(1, 1, 0, 0.5f);

    private void Awake()
    {
        UIStack.Instance.PushUI(gameObject);
    }

    public void OnEnable()
    {
        InputManager.Menu += Menu;
        InputManager.Navigate += Navigate;
    }

    public void OnDisable()
    {
        InputManager.Menu -= Menu;
        InputManager.Navigate -= Navigate;
    }


    public void Menu()
    {
        UIStack.Instance.PushUI(mainMenuCanvas);
    }

    private void Navigate()
    {
        selectedButtonName = UIManager.Instance.GetSelectedButtonName();
        switch (selectedButtonName)
        {
            case "Dungeon":
                placeSelectGroup[0].SetActive(true);
                placeSelectGroup[1].SetActive(false);
                placeSelectGroup[2].SetActive(false); break;
            case "Guild":
                placeSelectGroup[0].SetActive(false);
                placeSelectGroup[1].SetActive(true);
                placeSelectGroup[2].SetActive(false); break;
            case "Shop":
                placeSelectGroup[0].SetActive(false);
                placeSelectGroup[1].SetActive(false);
                placeSelectGroup[2].SetActive(true); break;
        }

    }

}
