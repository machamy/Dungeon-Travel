using Scripts.Manager;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Scripts.Manager.UIManager;

public class SkillUI : MonoBehaviour
{
    public GameObject itemButtonPrefab;
    public GameObject buttonParent;

    public GameObject characterButton, useButton;

    private GameObject skillButton;
    private GameObject selectedCharacter;
    private GameObject selectedSkill;

    public GameObject blackPanel;

    private Color lightblue = new(0, 1, 1, 0.5f), blue = new(0, 0.5f, 1, 0.5f);

    public enum UIDepth
    {
        Character, Skill, Use, UseCharacter
    }
    private UIDepth depth = UIDepth.Character;

    private void OnEnable()
    {
        UIManager.Instance.SelectButton(characterButton);
        GetInventoryItem(); //temp
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
            case UIDepth.Character:
                ScrollManager.Instance.posN = 0;
                UIStack.Instance.PopUI();
                break;
            case UIDepth.Skill:
                UIManager.Instance.SelectButton(selectedCharacter);
                selectedCharacter.GetComponent<Image>().color = lightblue;
                depth--;
                break;
            case UIDepth.Use:
                UIManager.Instance.SelectButton(selectedSkill);
                selectedSkill.GetComponent<Image>().color = lightblue;
                depth--;
                break;
            case UIDepth.UseCharacter:
                UIManager.Instance.SelectButton(useButton);
                blackPanel.SetActive(false);
                depth--;
                break;
        }
    }

    private void Navigate()
    {
        if (depth != UIDepth.Character) return;
        GetInventoryItem();
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
            buttonPrefab.transform.localScale = Vector3.one; //why????

            Button button = buttonPrefab.GetComponentInChildren<Button>();
            Navigation navigation = button.navigation;
            button.onClick.AddListener(delegate { SkillClick(); });

            buttonPrefab.name = itemName;
            buttonPrefab.GetComponentsInChildren<TextMeshProUGUI>()[1].text = itemName;

            if (posN == 0) skillButton = buttonPrefab.transform.GetChild(1).gameObject;

            /*if (posN == length - 1)
            {
                navigation.mode = Navigation.Mode.Explicit;
                navigation.selectOnUp = buttonPrefab.transform.parent.
                    GetChild(length - 2).GetComponentInChildren<Button>();
                button.navigation = navigation;
            }*/
            posN++;
        }
    }

    public void Skill()
    {
        depth = UIDepth.Skill;
        selectedCharacter = UIManager.Instance.GetSelectedButton();
        selectedCharacter.GetComponent<Image>().color = blue;
        UIManager.Instance.SelectButton(skillButton);
    }

    private void SkillClick()
    {
        selectedSkill = UIManager.Instance.GetSelectedButton();
        selectedSkill.GetComponent<Image>().color = blue;
        Use();
    }

    private void Use()
    {
        UIManager.Instance.SelectButton(useButton);
        depth = UIDepth.Use;
    }

    public void UseCharacter()
    {
        UIManager.Instance.SelectButton(characterButton);
        blackPanel.SetActive(true);
        depth = UIDepth.UseCharacter;
    }
}
