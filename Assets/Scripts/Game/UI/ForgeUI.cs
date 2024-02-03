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
    public GameObject menuContainer, tableContainer;
    public GameObject askBuyContainer, askExitContainer, talkContainer;
    public GameObject bottomPanel;
    public GameObject menuFirstSelect, weaponFirstSelect, askBuyFirstSelect, askExitFirstSelect;

    public TextMeshProUGUI itemNameText, buttonDescriptionText, talkText;
    
    public GameObject[] typeButton = new GameObject[3];

    int currentType = 0;

    private Color blue = new(0, 1, 1, 0.5f), yellow = new(1, 1, 0, 0.5f);


    private void Awake()
    {
        UIManager.Instance.currentState = UIManager.State.ForgeMenu;
    }

    public void Menu(GameObject disableUI)
    {
        UIManager.Instance.SetUI(UIManager.State.ForgeMenu,
            menuContainer, disableUI, menuFirstSelect);

        bottomPanel.SetActive(true);
    }

    public void Weapon(GameObject disableUI)
    {
        UIManager.Instance.SetUI(UIManager.State.ForgeWeapon,
            tableContainer, disableUI, weaponFirstSelect);
    }

    public void OnXMove(InputValue value)
    {
        if (UIManager.Instance.currentState != UIManager.State.ForgeWeapon) return;

        int intvalue = (int)value.Get<float>();
        if (intvalue == 0) return;
        if (currentType + intvalue < 0 || currentType + intvalue > 2) return;

        SwitchType(currentType + intvalue);
    }

    public void SwitchType(int value)
    {
        typeButton[currentType].GetComponent<Image>().color = blue;
        typeButton[currentType = value].GetComponent<Image>().color = yellow;

        UIManager.Instance.SelectButton(weaponFirstSelect);
    }

    public void AskBuy(string itemName)
    {
        UIManager.Instance.SetUI(UIManager.State.ForgeAskBuy,
            askBuyContainer, null, askBuyFirstSelect);

        itemNameText.text = "You wanna buy " + itemName + "?";
    }

    public void Talk()
    {
        UIManager.Instance.SetUI(UIManager.State.ForgeTalk,
            talkContainer, menuContainer, null);

        bottomPanel.SetActive(false);

        int textNum = Random.Range(0, UIDB.talkList.Count);
        talkText.text = UIDB.talkList[textNum];
    }

    public void AskExit()
    {
        UIManager.Instance.SetUI(UIManager.State.ForgeAskExit,
            askExitContainer, null, askExitFirstSelect);
    }

    public void OnCancel()
    {
        switch (UIManager.Instance.currentState)
        {
            case UIManager.State.ForgeAskBuy:
                Weapon(askBuyContainer); break;

            case UIManager.State.ForgeWeapon:
                Menu(tableContainer); break;

            case UIManager.State.ForgeTalk:
                Menu(talkContainer); break;

            case UIManager.State.ForgeAskExit:
                Menu(askExitContainer); break;

            case UIManager.State.ForgeMenu:
                AskExit(); break;
        }
    }

    public void OnSubmit()
    {
        switch (UIManager.Instance.currentState)
        {
            case UIManager.State.ForgeTalk:
                Menu(talkContainer); break;
        }
    }

    public void OnClick()
    {
        switch (UIManager.Instance.currentState)
        {
            case UIManager.State.ForgeTalk:
                Menu(talkContainer); break;
        }
    }

    private void Update()
    {
        buttonDescriptionText.text = UIManager.Instance.GetSelectedButtonDescription();
    }

}
