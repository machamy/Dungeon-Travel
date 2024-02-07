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
    public GameObject placeContainer, mainMenuContainer, menuContainer, partyContainer,
        itemContainer, configContainer;
    public GameObject blockPartyPanel, partyBlackPanel;
    public GameObject placeFirstSelect, menuFirstSelect, useDeleteFirstSelect,
        itemPartyFirstSelect, configFirstSelect, videoFirstSelect;

    public GameObject itemButtonPrefab, itemButtonParent;

    private string selectedButtonName;
    private GameObject selectedItem; 
    public TextMeshProUGUI buttonDescriptionText, itemDescriptionText;

    public Image dungeonCircle, guildCircle, shopCircle;
    private Color red = new(1, 0, 0, 0.5f), yellow = new(1, 1, 0, 0.5f),
        lightblue = new(0, 1, 1, 0.5f), blue = new(0, 0.5f, 1, 0.5f);


    private void Awake()
    {
        UIManager.Instance.currentState = UIDB.State.Main_Place;
    }

    public void Place(GameObject disableUI)
    {
        UIManager.Instance.SetUI(UIDB.State.Main_Place,
            placeContainer, disableUI, placeFirstSelect);
    }

    public void Menu(GameObject disableUI)
    {
        UIManager.Instance.SetUI(UIDB.State.Main_Menu,
            menuContainer, disableUI, menuFirstSelect);
        partyContainer.SetActive(true);
    }

    public void OnMenu()
    {
        if (UIManager.Instance.currentState != UIDB.State.Main_Place) return;
        mainMenuContainer.SetActive(true);
        Menu(placeContainer);
    }


    public void Item(GameObject disableUI)
    {
        UIManager.Instance.SetUI(UIDB.State.Main_Item,
            itemContainer, disableUI, null);

        if (disableUI != null)
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
            partyBlackPanel, blockPartyPanel, itemPartyFirstSelect);
    }

    public void Config(GameObject disableUI)
    {
        UIManager.Instance.SetUI(UIDB.State.Main_Config,
            configContainer, disableUI, configFirstSelect);
        partyContainer.SetActive(false);
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
                Place(mainMenuContainer); break;

            case UIDB.State.Main_Item:
                Menu(itemContainer); break;

            case UIDB.State.Main_ItemUseDelete:
                Item(null);
                selectedItem.GetComponent<Image>().color = lightblue; break;

            case UIDB.State.Main_ItemParty:
                ItemUseDelete(partyBlackPanel); break;

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
