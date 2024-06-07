using Scripts.Manager;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static InputManager;

public class EquipUI : MonoBehaviour
{

    public GameObject itemButtonPrefab;
    public GameObject buttonParent;

    public GameObject characterButton;
    public GameObject beforeAfterPanel;

    public GameObject tabContainer;

    private GameObject itemButton;
    private GameObject selectedCharacter;

    private Color lightblue = new(0, 1, 1, 0.5f), blue = new(0, 0.5f, 1, 0.5f), 
        yellow = new(1, 1, 0, 0.5f);
    private int tabIndex = 0; private const int maxIndex = 2;

    public enum UIDepth
    {
        Character, Item
    }
    private UIDepth depth = UIDepth.Character;

    private void OnEnable()
    {
        UIManager.Instance.SelectButton(characterButton);
        GetInventoryItem(); //temp
        InputManager.Cancel += Cancel;
        InputManager.Navigate += Navigate;
        InputManager.XMove += XMove;
    }

    private void OnDisable()
    {
        InputManager.Cancel -= Cancel;
        InputManager.Navigate -= Navigate;
        InputManager.XMove -= XMove;
    }

    private void Cancel()
    {
        switch (depth)
        {
            case UIDepth.Character:
                ScrollManager.Instance.posN = 0;
                UIStack.Instance.PopUI();
                break;
            case UIDepth.Item:
                beforeAfterPanel.SetActive(false);
                UIManager.Instance.SelectButton(selectedCharacter);
                selectedCharacter.GetComponent<Image>().color = lightblue;
                depth--;
                break;
        }
    }

    private void Navigate()
    {
        if (depth != UIDepth.Character) return;
        GetInventoryItem();
    }

    private void XMove(InputValue value)
    {
        MoveTab((int)value.Get<float>());
    }

    public void MoveTab(int axis)
    {
        if (tabIndex + axis < 0 || tabIndex + axis > maxIndex) return;

        tabContainer.transform.GetChild(tabIndex + 1).
            GetComponent<Image>().color = lightblue;
        tabContainer.transform.GetChild((tabIndex += axis) + 1).
            GetComponent<Image>().color = yellow;
        GetInventoryItem();

        if (depth == UIDepth.Item) UIManager.Instance.SelectButton(itemButton);
    }

    public void GetInventoryItem() //temp
    {
        int posN = 0; int length = UIDB.inventoryItemList.Count;

        UIManager.Instance.ClearChildren(buttonParent);
        ScrollManager.Instance.Reset();
        foreach (string itemName in UIDB.inventoryItemList)
        {
            GameObject buttonPrefab = Instantiate(itemButtonPrefab);
            buttonPrefab.transform.SetParent(buttonParent.transform);

            Button button = buttonPrefab.GetComponentInChildren<Button>();
            Navigation navigation = button.navigation;
            button.onClick.AddListener(delegate { ItemClick(); });

            buttonPrefab.name = itemName;
            buttonPrefab.GetComponentsInChildren<TextMeshProUGUI>()[1].text = itemName;

            if (posN == 0) itemButton = buttonPrefab.transform.GetChild(1).gameObject;

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

    public void Item()
    {
        depth = UIDepth.Item;
        beforeAfterPanel.SetActive(true);
        selectedCharacter = UIManager.Instance.GetSelectedButton();
        selectedCharacter.GetComponent<Image>().color = blue;
        UIManager.Instance.SelectButton(itemButton);
    }

    private void ItemClick()
    {
        
    }

    
}
