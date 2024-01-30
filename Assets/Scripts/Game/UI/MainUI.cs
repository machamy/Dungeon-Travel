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
    public GameObject mainMenuContainer, menuContainer, itemContainer;
    public GameObject placeFirstSelect, menuFirstSelect;

    public GameObject itemButtonPrefab, itemButtonParent;

    private string selectedButtonName, selectedItemName, currentItemName;
    public TextMeshProUGUI itemDescriptionText;

    public Image dungeonCircle, guildCircle, shopCircle;
    private Color red = new(1, 0, 0, 0.5f), yellow = new(1, 1, 0, 0.5f);


    private void Awake()
    {
        UIManager.Instance.currentState = UIManager.State.MainPlace;
    }

    public void Place(GameObject disableUI)
    {
        UIManager.Instance.SetUI(UIManager.State.MainPlace,
            null, disableUI, placeFirstSelect);
    }

    public void Menu(GameObject disableUI)
    {
        UIManager.Instance.SetUI(UIManager.State.MainMenu,
            menuContainer, disableUI, menuFirstSelect);
    }

    public void OnMenu()
    {
        if (UIManager.Instance.currentState != UIManager.State.MainPlace) return;
        mainMenuContainer.SetActive(true);
        Menu(null);
    }

    public void Item(GameObject disableUI)
    {
        UIManager.Instance.SetUI(UIManager.State.MainItem,
            itemContainer, disableUI, null);

        UIManager.Instance.GetInventoryItem(itemButtonPrefab, itemButtonParent);
    }

    public void SelfClickToSelect()
    {
        UIManager.Instance.SelectButton(gameObject);
    }

    public void OnCancel()
    {
        switch (UIManager.Instance.currentState)
        {
            case UIManager.State.MainMenu:
                Place(mainMenuContainer); break;
            case UIManager.State.MainItem:
                Menu(itemContainer); break;
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

        itemDescriptionText.text = UIManager.Instance.GetSelectedItemDescription();
    }

}
