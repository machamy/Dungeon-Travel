using Scripts.Manager;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Scripts.Manager.UIManager;

public class ItemUI : MonoBehaviour
{
    public GameObject itemButtonPrefab;
    public GameObject buttonParent;
    private GameObject selectedItem;

    public GameObject useButton, characterButton;
    public GameObject blackPanel;

    public TextMeshProUGUI itemDescriptionText;

    private Color lightblue = new(0, 1, 1, 0.5f), blue = new(0, 0.5f, 1, 0.5f);

    public enum UIDepth
    {
        Item, Use, Character
    } private UIDepth depth = UIDepth.Item;

    private void OnEnable()
    {
        GetInventoryItem();
        InputManager.Cancel += Cancel;
        InputManager.Navigate += Navigate;
    }

    private void OnDisable()
    {
        InputManager.Cancel -= Cancel;
        InputManager.Navigate -= Navigate;
    }

    private void Cancel()
    {
        switch (depth)
        {
            case UIDepth.Item:
                UIManager.Instance.ClearChildren(buttonParent);
                UIStack.Instance.PopUI();
                break;
            case UIDepth.Use:
                UIManager.Instance.SelectButton(selectedItem);
                selectedItem.GetComponent<Image>().color = lightblue;
                depth--;
                break;
            case UIDepth.Character:
                UIManager.Instance.SelectButton(useButton);
                blackPanel.SetActive(false);
                depth--;
                break;
        }
    }

    private void Navigate()
    {
        itemDescriptionText.text = UIManager.Instance.GetSelectedItemDescription();
    }


    public void GetInventoryItem()
    {
        int posN = 0; int length = UIDB.inventoryItemList.Count;
        foreach (string itemName in UIDB.inventoryItemList)
        {
            GameObject buttonPrefab = Instantiate(itemButtonPrefab);
            buttonPrefab.transform.SetParent(buttonParent.transform);

            Button button = buttonPrefab.GetComponentInChildren<Button>();
            Navigation navigation = button.navigation;
            button.onClick.AddListener(delegate { ItemClick(); });

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
        Use();
    }

    private void Use()
    {
        UIManager.Instance.SelectButton(useButton);
        depth = UIDepth.Use;
    }

    public void Character()
    {
        UIManager.Instance.SelectButton(characterButton);
        blackPanel.SetActive(true);
        depth = UIDepth.Character;
    }

}
