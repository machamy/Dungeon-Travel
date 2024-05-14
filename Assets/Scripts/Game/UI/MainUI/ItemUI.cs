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
    private GameObject selectedItem, selectedCharacter;

    public GameObject useButton, characterButton;

    private Color lightblue = new(0, 1, 1, 0.5f), blue = new(0, 0.5f, 1, 0.5f);


    private void Awake()
    {
        GetInventoryItem(prefabParent);
    }

    public void CharacterClick()
    {
        selectedCharacter = UIManager.Instance.GetSelectedButton();
        selectedCharacter.GetComponent<Image>().color = blue;
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

    }
}
