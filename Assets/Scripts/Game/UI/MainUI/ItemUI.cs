using Scripts.Manager;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    public GameObject itemButtonPrefab;
    public GameObject prefabParent;
    private GameObject selectedItem;

    public GameObject useButton, characterButton;
    public GameObject blackPanel;

    public TextMeshProUGUI itemDescriptionText;

    private Color lightblue = new(0, 1, 1, 0.5f), blue = new(0, 0.5f, 1, 0.5f);

    public enum UIDepth
    {
        ItemSelect, UseSelect, CharacterSelect
    } private UIDepth depth = UIDepth.ItemSelect;

    private void OnEnable()
    {
        GetInventoryItem(prefabParent);
        MainUI.Cancel += Cancel;
    }

    private void OnDisable()
    {
        MainUI.Cancel -= Cancel;
    }

    private void Cancel()
    {
        switch (depth)
        {
            case UIDepth.ItemSelect:
                UIStack.Instance.PopUI();
                break;
            case UIDepth.UseSelect:
                UIManager.Instance.SelectButton(selectedItem);
                selectedItem.GetComponent<Image>().color = lightblue;
                depth--;
                break;
            case UIDepth.CharacterSelect:
                UIManager.Instance.SelectButton(useButton);
                blackPanel.SetActive(false);
                depth--;
                break;
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
        depth = UIDepth.UseSelect;
        UseSelect();
    }

    private void UseSelect()
    {
        UIManager.Instance.SelectButton(useButton);
    }

    public void CharacterSelect()
    {
        UIManager.Instance.SelectButton(characterButton);
        blackPanel.SetActive(true);
        depth = UIDepth.CharacterSelect;
    }

    private void Update()
    {
        itemDescriptionText.text = UIManager.Instance.GetSelectedItemDescription();
    }
}
