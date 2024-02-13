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

public class ForgeUI : MonoBehaviour
{
    public GameObject menuContainer, tableContainer, askBuyContainer,
        askExitContainer, talkContainer;
    public GameObject bottomPanel;
    public GameObject menuFirstSelect, weaponFirstSelect, askBuyFirstSelect, askExitFirstSelect;

    public TextMeshProUGUI itemNameText, buttonDescriptionText, talkText;

    public GameObject tabContainer;

    private int currentTabIndex = 1;

    private Color lightblue = new(0, 1, 1, 0.5f), yellow = new(1, 1, 0, 0.5f);


    private void Awake()
    {
        UIManager.Instance.currentState = UIDB.State.Forge_Menu;
    }

    public void Menu(GameObject disableUI)
    {
        UIManager.Instance.SetUI(UIDB.State.Forge_Menu,
            menuContainer, disableUI, menuFirstSelect);

        bottomPanel.SetActive(true);
    }

    public void Weapon(GameObject disableUI)
    {
        UIManager.Instance.SetUI(UIDB.State.Forge_Weapon,
            tableContainer, disableUI, weaponFirstSelect);
    }

    public void OnXMove(InputValue value)
    {
        if (UIManager.Instance.currentState != UIDB.State.Forge_Weapon) return;

        int index = UIManager.Instance.GetTabIndex(value, currentTabIndex, 3);
        if (index != -1) SwitchTab(index);
    }

    public void SwitchTab(int value)
    {
        tabContainer.transform.GetChild(currentTabIndex).
            GetComponent<Image>().color = lightblue;
        tabContainer.transform.GetChild(currentTabIndex = value).
            GetComponent<Image>().color = yellow;

        UIManager.Instance.SelectButton(weaponFirstSelect);
    }

    public void AskBuy(string itemName)
    {
        UIManager.Instance.SetUI(UIDB.State.Forge_AskBuy,
            askBuyContainer, null, askBuyFirstSelect);

        itemNameText.text = "You wanna buy " + itemName + "?";
    }

    public void Talk()
    {
        UIManager.Instance.SetUI(UIDB.State.Forge_Talk,
            talkContainer, menuContainer, null);

        bottomPanel.SetActive(false);

        int textNum = Random.Range(0, UIDB.talkList.Count);
        talkText.text = UIDB.talkList[textNum];
    }

    public void AskExit()
    {
        UIManager.Instance.SetUI(UIDB.State.Forge_AskExit,
            askExitContainer, null, askExitFirstSelect);
    }

    public void OnCancel()
    {
        switch (UIManager.Instance.currentState)
        {
            case UIDB.State.Forge_AskBuy:
                Weapon(askBuyContainer); break;

            case UIDB.State.Forge_Weapon:
                Menu(tableContainer); break;

            case UIDB.State.Forge_Talk:
                Menu(talkContainer); break;

            case UIDB.State.Forge_AskExit:
                Menu(askExitContainer); break;

            case UIDB.State.Forge_Menu:
                AskExit(); break;
        }
    }

    public void OnSubmit()
    {
        switch (UIManager.Instance.currentState)
        {
            case UIDB.State.Forge_Talk:
                Menu(talkContainer); break;
        }
    }

    public void OnClick()
    {
        switch (UIManager.Instance.currentState)
        {
            case UIDB.State.Forge_Talk:
                Menu(talkContainer); break;
        }
    }

    private void Update()
    {
        buttonDescriptionText.text = UIManager.Instance.GetSelectedButtonDescription();
    }

}
