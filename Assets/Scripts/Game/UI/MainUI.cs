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
    public enum State
    {
        Place = 0,
        Menu = 1,
            Item = 10,
                ItemUseDelete = 100,
                    ItemUse = 1000,
            Skill = 20,
                SkillList = 200,
                    SkillUseDelete = 2000,
                        SkillUse = 20000,
            Equip = 30,
                EquipList = 300,
            Status = 40,
            Party = 50,
            Config = 60,
                ConfigType = 600,
                ConfigVideo = 601,
    } private State currentState = 0;

    public void ChangeState(int state) => currentState = (State)state;
    public void PrevState() => currentState = (State)((int)currentState / 10);




    public Button _MainMenu, _ItemUseDelete, _SkillUseDelete, _SkillList, _EquipList;

    public GameObject itemButtonPrefab;

    private string selectedButtonName;
    private GameObject selectedItem, selectedCharacter; 
    public TextMeshProUGUI topPanelText, buttonDescriptionText, itemDescriptionText;

    public Image dungeonCircle, guildCircle, shopCircle;
    private Color red = new(1, 0, 0, 0.5f), yellow = new(1, 1, 0, 0.5f),
        lightblue = new(0, 1, 1, 0.5f), blue = new(0, 0.5f, 1, 0.5f);

    private int currentTabIndex = 1;

    public void Menu(GameObject disableUI)
    {

        topPanelText.text = "MAIN MENU";
    }

    public void OnMenu() => _MainMenu.onClick.Invoke();


    public void Item(GameObject disableUI)
    {

        topPanelText.text = "ITEM";
    }

    public void Skill(GameObject disableUI)
    {
        topPanelText.text = "SKILL";
    }


    public void Equip(GameObject disableUI)
    {

        topPanelText.text = "EQUIP";
    }


    public void OnXMove(InputValue value)
    {
        if (currentState != State.EquipList) return;

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

    public void Config(GameObject disableUI)
    {

        topPanelText.text = "CONFIG";
    }


    public void OnCancel()
    {
        switch (currentState)
        {
            case State.Menu:
                break;

            case State.Item:
                break;

            case State.ItemUseDelete:
                selectedItem.GetComponent<Image>().color = lightblue; break;

            case State.ItemUse:
                break;

            case State.Skill:
                break;

            case State.SkillList:
                selectedCharacter.GetComponent<Image>().color = lightblue; break;

            case State.SkillUseDelete:
                selectedItem.GetComponent<Image>().color = lightblue; break;

            case State.SkillUse:
                break;

            case State.Equip:
                break;

            case State.EquipList:
                selectedCharacter.GetComponent<Image>().color = lightblue; break;

            case State.Config:
                break;

            case State.ConfigVideo:
                break;
        }
    }

    public void OnSubmit()
    {
        switch (currentState)
        {

        }
    }

    public void OnClick()
    {
        switch (currentState)
        {
            
        }
    }


    public void CharacterClick()
    {
        selectedCharacter = UIManager.Instance.GetSelectedButton();
        selectedCharacter.GetComponent<Image>().color = blue;

        switch (currentState)
        {
            case State.Equip:
                _EquipList.onClick.Invoke(); break;

            case State.Skill:
                _SkillList.onClick.Invoke(); break;
        }
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

        switch (currentState)
        {
            case State.Item:
                _ItemUseDelete.onClick.Invoke(); break;

            case State.Skill:
                _SkillUseDelete.onClick.Invoke(); break;
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
