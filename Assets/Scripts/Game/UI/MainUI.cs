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
    public GameObject placeContainer, mainMenuContainer, menuContainer, itemContainer,
        useDeleteContainer;
    public GameObject blockPartyPanel;
    public GameObject placeFirstSelect, menuFirstSelect;

    public GameObject itemButtonPrefab, itemButtonParent;

    private string selectedButtonName, selectedItemName, currentItemName;
    public TextMeshProUGUI buttonDescriptionText, itemDescriptionText;

    public Image dungeonCircle, guildCircle, shopCircle;
    private Color red = new(1, 0, 0, 0.5f), yellow = new(1, 1, 0, 0.5f);


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
            case UIDB.State.Main_Menu:
                Place(mainMenuContainer); break;
            case UIDB.State.Main_Item:
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
