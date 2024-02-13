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
    public GameObject townMapContainer, menuContainer, characterContainer,
        itemContainer, equipContainer, tabContainer, configContainer;
    public GameObject mainMenuPanel, blockPartyPanel, partyBlackPanel, beforeAfterPanel;
    public GameObject placeFirstSelect, menuFirstSelect, useDeleteFirstSelect,
        characterFirstSelect, configFirstSelect, videoFirstSelect;

    public GameObject itemButtonPrefab, itemButtonParent, skillButtonParent,
        equipButtonParent;

    private string selectedButtonName;
    private GameObject selectedItem; 
    public TextMeshProUGUI buttonDescriptionText, itemDescriptionText;

    public Image dungeonCircle, guildCircle, shopCircle;
    private Color red = new(1, 0, 0, 0.5f), yellow = new(1, 1, 0, 0.5f),
        lightblue = new(0, 1, 1, 0.5f), blue = new(0, 0.5f, 1, 0.5f);

    private int currentTabIndex = 1;


    private void Awake()
    {
        UIManager.Instance.currentState = UIDB.State.Main_Place;
    }

    public void Place(GameObject disableUI)
    {
        UIManager.Instance.SetUI(UIDB.State.Main_Place,
            townMapContainer, disableUI, placeFirstSelect);
    }

    public void Menu(GameObject disableUI)
    {
        UIManager.Instance.SetUI(UIDB.State.Main_Menu,
            menuContainer, disableUI, menuFirstSelect);
        characterContainer.SetActive(true);
    }

    public void OnMenu()
    {
        if (UIManager.Instance.currentState != UIDB.State.Main_Place) return;
        mainMenuPanel.SetActive(true);
        Menu(townMapContainer);
    }


    public void Item(GameObject disableUI)
    {
        UIManager.Instance.SetUI(UIDB.State.Main_Item,
            itemContainer, disableUI, null);

        if (disableUI == menuContainer)
            UIManager.Instance.GetInventoryItem(itemButtonPrefab, itemButtonParent);
        else UIManager.Instance.SelectButton(selectedItem);
    }


    public void ItemUseDelete(GameObject disableUI)
    {
        UIManager.Instance.SetUI(UIDB.State.Main_ItemUseDelete,
            blockPartyPanel, disableUI, useDeleteFirstSelect);
    }

    public void ItemParty()
    {
        UIManager.Instance.SetUI(UIDB.State.Main_ItemParty,
            partyBlackPanel, blockPartyPanel, characterFirstSelect);
    }

    public void Equip(GameObject disableUI)
    {
        if (disableUI == menuContainer)
            UIManager.Instance.GetInventoryItem(itemButtonPrefab, equipButtonParent);

        UIManager.Instance.SetUI(UIDB.State.Main_Equip,
            equipContainer, disableUI, characterFirstSelect);
    }

    public void EquipItem()
    {
        UIManager.Instance.SetUI(UIDB.State.Main_EquipItem,
            beforeAfterPanel, null, equipButtonParent.transform.
            GetChild(0).GetChild(1).gameObject);
    }

    public void OnXMove(InputValue value)
    {
        if (UIManager.Instance.currentState != UIDB.State.Main_EquipItem) return;

        int index = UIManager.Instance.GetTabIndex(value, currentTabIndex, 3);
        if (index != -1) SwitchTab(index);
    }

    public void SwitchTab(int value)
    {
        tabContainer.transform.GetChild(currentTabIndex).
            GetComponent<Image>().color = lightblue;
        tabContainer.transform.GetChild(currentTabIndex = value).
            GetComponent<Image>().color = yellow;

        UIManager.Instance.SelectButton(equipButtonParent.transform.
            GetChild(0).GetChild(1).gameObject);
    }

    public void Config(GameObject disableUI)
    {
        UIManager.Instance.SetUI(UIDB.State.Main_Config,
            configContainer, disableUI, configFirstSelect);
        characterContainer.SetActive(false);
    }

    public void ConfigVideo()
    {
        UIManager.Instance.SetUI(UIDB.State.Main_ConfigVideo,
            null, null, videoFirstSelect);
    }

    public void OnCancel()
    {
        switch (UIManager.Instance.currentState)
        {
            case UIDB.State.Main_Menu:
                Place(mainMenuPanel); break;

            case UIDB.State.Main_Item:
                Menu(itemContainer); break;

            case UIDB.State.Main_ItemUseDelete:
                Item(null);
                selectedItem.GetComponent<Image>().color = lightblue; break;

            case UIDB.State.Main_ItemParty:
                ItemUseDelete(partyBlackPanel); break;

            case UIDB.State.Main_Equip:
                Menu(equipContainer); break;

            case UIDB.State.Main_EquipItem:
                Equip(beforeAfterPanel); break;

            case UIDB.State.Main_Config:
                Menu(configContainer); break;

            case UIDB.State.Main_ConfigVideo:
                Config(null); break;
        }
    }

    public void OnSubmit()
    {
        switch (UIManager.Instance.currentState)
        {
            case UIDB.State.Main_Item:
                selectedItem = UIManager.Instance.GetSelectedButton();
                selectedItem.GetComponent<Image>().color = blue;
                ItemUseDelete(null); break;
        }
    }

    public void OnClick()
    {
        switch (UIManager.Instance.currentState)
        {
            
        }
    }

    public void OnScrollWheel(InputValue value)
    {
        if (UIManager.Instance.currentState != UIDB.State.Main_Item) return;
        AutoScroll.Instance.ScrollWheel(value);
    }

    public void CharacterClick()
    {
        if (UIManager.Instance.currentState == UIDB.State.Main_Equip)
        {
            EquipItem();
        }
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
