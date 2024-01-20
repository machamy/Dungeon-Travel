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
    public GameObject askBuyPanel;
    public GameObject itemNameText;
    public GameObject behaviourFirstSelect, weaponFirstSelect, askBuyFirstSelect;
    public PlayerInput playerInput;
    public GameObject[] typeButton = new GameObject[5];
    string currentState = "SelectBehaviour";
    int currentType = 0;

    public InputActionReference mainNavigation, yNavigation;


    public void SelectBehaviour()
    {
        currentState = "SelectBehaviour";
        behaviourContainer.SetActive(true);
        typeContainer.SetActive(false);
        listContainer.SetActive(false);
        infoContainer.SetActive(false);
        EventSystem.current.SetSelectedGameObject(behaviourFirstSelect);
        ((InputSystemUIInputModule)EventSystem.current.currentInputModule).move = mainNavigation;
    }

    public void SelectWeapon()
    {
        currentState = "SelectWeapon";
        behaviourContainer.SetActive(false);
        askBuyPanel.SetActive(false);
        typeContainer.SetActive(true);
        listContainer.SetActive(true);
        infoContainer.SetActive(true);
        EventSystem.current.SetSelectedGameObject(weaponFirstSelect);
        ((InputSystemUIInputModule)EventSystem.current.currentInputModule).move = yNavigation;
    }

    public void OnSwitchType(InputValue value)
    {
        if (currentState != "SelectWeapon") return;
        int intvalue = (int)value.Get<float>();
        if (currentType + intvalue < 0 || currentType + intvalue > 4) return;
        EventSystem.current.SetSelectedGameObject(weaponFirstSelect);
        typeButton[currentType].GetComponent<Image>().color = new Color(0, 1, 1, 0.5f);
        typeButton[currentType += intvalue].GetComponent<Image>().color = new Color(1, 1, 0, 0.5f);
        
    }

    public void SwitchTypeByClick(int value)
    {
        typeButton[currentType].GetComponent<Image>().color = new Color(0, 1, 1, 0.5f);
        typeButton[currentType = value].GetComponent<Image>().color = new Color(1, 1, 0, 0.5f);
        EventSystem.current.SetSelectedGameObject(weaponFirstSelect);
    }

    public void AskBuyItem(string itemName)
    {
        currentState = "AskBuyItem";
        askBuyPanel.SetActive(true);
        itemNameText.GetComponent<TextMeshProUGUI>().text = "You wanna buy " + itemName + "?";
        EventSystem.current.SetSelectedGameObject(askBuyFirstSelect);
        ((InputSystemUIInputModule)EventSystem.current.currentInputModule).move = mainNavigation;
    }

    public void OnCancel()
    {
        switch (currentState)
        {
            case "AskBuyItem":
                SelectWeapon();
                break;
            case "SelectWeapon":
                SelectBehaviour();
                break;
        }
    }

}
