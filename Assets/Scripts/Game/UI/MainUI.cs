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

public class MainUI : MonoBehaviour
{


    public GameObject townMapCanvas, mainMenuCanvas;

    public GameObject mainMenuFirstSelect;

    public GameObject itemButtonPrefab;

    private string selectedButtonName;
    private GameObject selectedItem, selectedCharacter; 
    public TextMeshProUGUI topPanelText, buttonDescriptionText, itemDescriptionText;

    public Image dungeonCircle, guildCircle, shopCircle;
    private Color red = new(1, 0, 0, 0.5f), yellow = new(1, 1, 0, 0.5f),
        lightblue = new(0, 1, 1, 0.5f), blue = new(0, 0.5f, 1, 0.5f);

    private int currentTabIndex = 1;

    public void Awake()
    {
        UIManager.Instance.PushMenu(townMapCanvas);
    }

    public void OnMenu()
    {
        if (UIManager.Instance.menuStack.Count == 1)
        {
            UIManager.Instance.PushMenu(mainMenuCanvas);
            UIManager.Instance.SelectButton(mainMenuFirstSelect);
        }
    }


    public void OnXMove(InputValue value)
    {
        //if (currentState != State.EquipList) return;

        int index = UIManager.Instance.GetTabIndex(value, currentTabIndex, 3);
        if (index != -1) SwitchTab(index);
    }

    public void SwitchTab(int value)
    {
        /*tabContainer.transform.GetChild(currentTabIndex).
            GetComponent<Image>().color = lightblue;
        tabContainer.transform.GetChild(currentTabIndex = value).
            GetComponent<Image>().color = yellow;*/

    }


    public void OnCancel()
    {
        UIManager.Instance.PopMenu();
    }


    public void CharacterClick()
    {
        selectedCharacter = UIManager.Instance.GetSelectedButton();
        selectedCharacter.GetComponent<Image>().color = blue;


    }


    public void GetInventoryItem(GameObject parent)
    {
        UIManager.Instance.ClearChildren(parent);
        int posN = 0; int length = UIDB.inventoryItemList.Count;
        foreach (string itemName in UIDB.inventoryItemList)
        {
            GameObject buttonPrefab = Instantiate(itemButtonPrefab);
            buttonPrefab.transform.SetParent(parent.transform);

            Button button = buttonPrefab.GetComponentInChildren<Button>();
            Navigation navigation = button.navigation;
            button.onClick.AddListener(delegate { ItemClick(); } );

            buttonPrefab.name = itemName;
            buttonPrefab.GetComponentInChildren<TextMeshProUGUI>().text = itemName;

            if (posN == 0)
            {
                UIManager.Instance.SelectButton
                    (buttonPrefab.transform.GetChild(1).gameObject);
            }
            if (posN == length - 1)
            {
                navigation.mode = Navigation.Mode.Explicit;
                navigation.selectOnUp = buttonPrefab.transform.parent.
                    GetChild(length - 2).GetComponentInChildren<Button>();
                button.navigation = navigation;
            }
            posN++;
        }
    }

    private void ItemClick()
    {
        selectedItem = UIManager.Instance.GetSelectedButton();
        selectedItem.GetComponent<Image>().color = blue;

        
    }

    private void Update()
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

        buttonDescriptionText.text = UIManager.Instance.GetSelectedButtonDescription();
        itemDescriptionText.text = UIManager.Instance.GetSelectedItemDescription();
    }

}
