using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class ForgeUI : MonoBehaviour
{
    public GameObject behaviourContainer, typeContainer, listContainer, infoContainer;
    public GameObject askBuyPanel, askExitPanel, talkPanel, bottomPanel;
    public GameObject itemNameText;
    public GameObject buttonDescriptionText;
    public GameObject talkText;
    public GameObject behaviourFirstSelect, weaponFirstSelect, askBuyFirstSelect, askExitFirstSelect;
    public PlayerInput playerInput;
    public GameObject[] typeButton = new GameObject[5];
    private State currentState = State.selectBehaviour;
    int currentType = 0;
    string selectButtonName;

    public InputActionReference mainNavigation, yNavigation;

    public enum State
    {
        selectBehaviour,
        selectWeapon,
        askBuyItem,
        Talk,
        askExit
    }

    public void SelectBehaviour()
    {
        currentState = State.selectBehaviour;

        behaviourContainer.SetActive(true);
        bottomPanel.SetActive(true);
        askExitPanel.SetActive(false);
        typeContainer.SetActive(false);
        listContainer.SetActive(false);
        infoContainer.SetActive(false);
        talkPanel.SetActive(false);

        SelectButton(behaviourFirstSelect);
        SelectNavigate(mainNavigation);
    }

    public void SelectWeapon()
    {
        currentState = State.selectWeapon;

        typeContainer.SetActive(true);
        listContainer.SetActive(true);
        infoContainer.SetActive(true);
        behaviourContainer.SetActive(false);
        askBuyPanel.SetActive(false);

        SelectButton(weaponFirstSelect);
        SelectNavigate(yNavigation);
    }

    public void OnSwitchType(InputValue value)
    {
        if (currentState != State.selectWeapon) return;

        int intvalue = (int)value.Get<float>();
        if (currentType + intvalue < 0 || currentType + intvalue > 4) return;

        SelectButton(weaponFirstSelect);
        typeButton[currentType].GetComponent<Image>().color = new Color(0, 1, 1, 0.5f);
        typeButton[currentType += intvalue].GetComponent<Image>().color = new Color(1, 1, 0, 0.5f);
        
    }

    public void SwitchTypeByClick(int value)
    {
        typeButton[currentType].GetComponent<Image>().color = new Color(0, 1, 1, 0.5f);
        typeButton[currentType = value].GetComponent<Image>().color = new Color(1, 1, 0, 0.5f);
        SelectButton(weaponFirstSelect);
    }

    public void AskBuyItem(string itemName)
    {
        currentState = State.askBuyItem;

        askBuyPanel.SetActive(true);

        itemNameText.GetComponent<TextMeshProUGUI>().text = "You wanna buy " + itemName + "?";
        SelectButton(askBuyFirstSelect);
        SelectNavigate(mainNavigation);
    }

    public void Talk()
    {
        currentState = State.Talk;

        talkPanel.SetActive(true);
        bottomPanel.SetActive(false);
        behaviourContainer.SetActive(false);

        EventSystem.current.SetSelectedGameObject(null);

        int textNum = Random.Range(0, UIDB.talkList.Count);
        talkText.GetComponent<TextMeshProUGUI>().text = UIDB.talkList[textNum];
    }

    public void AskExit()
    {
        currentState = State.askExit;

        askExitPanel.SetActive(true);

        SelectButton(askExitFirstSelect);
        SelectNavigate(mainNavigation);
    }

    public void OnCancel()
    {
        switch (currentState)
        {
            case State.askBuyItem:
                SelectWeapon(); break;
            case State.selectWeapon:
                SelectBehaviour(); break;
            case State.selectBehaviour:
                AskExit(); break;
            case State.Talk:
                SelectBehaviour(); break;
            case State.askExit:
                SelectBehaviour(); break;
        }
    }

    public void OnSubmit()
    {
        switch (currentState)
        {
            case State.Talk:
                SelectBehaviour(); break;
        }
    }

    public void OnClick()
    {
        switch (currentState)
        {
            case State.Talk:
                SelectBehaviour(); break;
        }
    }

    private void Update()
    {
        if (EventSystem.current.currentSelectedGameObject != null)
            selectButtonName = EventSystem.current.currentSelectedGameObject.name;

        if (UIDB.buttonDescription.ContainsKey(selectButtonName))
            buttonDescriptionText.GetComponent<TextMeshProUGUI>().text =
                UIDB.buttonDescription[selectButtonName];
    }

    public void SelectButton(GameObject button) =>
        StartCoroutine(WaitForSelectButton(button));

    private IEnumerator WaitForSelectButton(GameObject button)
    {
        yield return new WaitForSeconds(0.001f);
        EventSystem.current.SetSelectedGameObject(button);
    }

    public void SelectNavigate(InputActionReference navigation) =>
        ((InputSystemUIInputModule)EventSystem.current.currentInputModule).move = navigation;


}
