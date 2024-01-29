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
    public GameObject behaviourContainer, tableContainer;
    public GameObject askBuyContainer, askExitContainer, talkContainer;
    public GameObject bottomPanel;
    public GameObject behaviourFirstSelect, weaponFirstSelect, askBuyFirstSelect, askExitFirstSelect;

    public GameObject itemNameText;
    public GameObject buttonDescriptionText;
    public GameObject talkText;
    
    public GameObject[] typeButton = new GameObject[3];

    int currentType = 0;

    
    private Color blue = new(0, 1, 1, 0.5f), yellow = new(1, 1, 0, 0.5f);


    public void SelectBehaviour(GameObject disableUI)
    {
        UIManager.Instance.SetUI(UIManager.State.SelectBehaviour,
            behaviourContainer, disableUI, behaviourFirstSelect);

        bottomPanel.SetActive(true);
    }

    public void BuyWeapon(GameObject disableUI)
    {
        UIManager.Instance.SetUI(UIManager.State.BuyWeapon,
            tableContainer, disableUI, weaponFirstSelect, true);
    }

    public void OnXNavigate(InputValue value)
    {
        if (UIManager.Instance.currentState != UIManager.State.BuyWeapon) return;

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

    public void AskBuyItem(string itemName)
    {
        UIManager.Instance.SetUI(UIManager.State.AskBuyItem,
            askBuyContainer, null, askBuyFirstSelect);

        itemNameText.GetComponent<TextMeshProUGUI>().text = "You wanna buy " + itemName + "?";
    }

    public void Talk()
    {
        UIManager.Instance.SetUI(UIManager.State.Talk,
            talkContainer, behaviourContainer, null, true);

        bottomPanel.SetActive(false);

        int textNum = Random.Range(0, UIDB.talkList.Count);
        talkText.GetComponent<TextMeshProUGUI>().text = UIDB.talkList[textNum];
    }

    public void AskExit()
    {
        UIManager.Instance.SetUI(UIManager.State.AskExit,
            askExitContainer, null, askExitFirstSelect);
    }

    public void OnCancel()
    {
        switch (UIManager.Instance.currentState)
        {
            case UIManager.State.AskBuyItem:
                BuyWeapon(askBuyContainer); break;

            case UIManager.State.BuyWeapon:
                SelectBehaviour(tableContainer); break;

            case UIManager.State.Talk:
                SelectBehaviour(talkContainer); break;

            case UIManager.State.AskExit:
                SelectBehaviour(askExitContainer); break;

            case UIManager.State.SelectBehaviour:
                AskExit(); break;
        }
    }

    public void OnSubmit()
    {
        switch (UIManager.Instance.currentState)
        {
            case UIManager.State.Talk:
                SelectBehaviour(talkContainer); break;
        }
    }

    public void OnClick()
    {
        switch (UIManager.Instance.currentState)
        {
            case UIManager.State.Talk:
                SelectBehaviour(talkContainer); break;
        }
    }

    private void Update()
    {
        buttonDescriptionText.GetComponent<TextMeshProUGUI>().text =
            UIManager.Instance.GetSelectedButtonDescription();
    }

}
