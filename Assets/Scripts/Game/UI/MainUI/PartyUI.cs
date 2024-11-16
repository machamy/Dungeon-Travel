using Scripts.Manager;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static InputManager;

public class PartyUI : MonoBehaviour
{

    public GameObject characterButton;
    public GameObject partyButton;
    public TMP_Text[] partyName = new TMP_Text[6];

    private GameObject selectedCharacter;
    private Color lightblue = new(0, 1, 1, 0.5f), blue = new(0, 0.5f, 1, 0.5f);

    public enum UIDepth
    {
        Character, Party
    }
    private UIDepth depth = UIDepth.Character;

    private void OnEnable()
    {
        UIManager.Instance.SelectButton(characterButton);
        InputManager.Cancel += Cancel;
    }

    private void OnDisable()
    {
        InputManager.Cancel -= Cancel;
    }

    private void Cancel()
    {
        switch (depth)
        {
            case UIDepth.Character:
                UIStack.Instance.PopUI();
                break;
            case UIDepth.Party:
                UIManager.Instance.SelectButton(selectedCharacter);
                selectedCharacter.GetComponent<Image>().color = Color.white;
                depth--;
                break;
        }
    }

    public void Party()
    {
        depth = UIDepth.Party;
        selectedCharacter = UIManager.Instance.GetSelectedButton();
        selectedCharacter.GetComponent<Image>().color = blue;
        UIManager.Instance.SelectButton(partyButton);
    }

    public void PartySelect(int num)
    {
        //partyName[num].text = selectedCharacter.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text;
        partyName[num].text = "test";
        Cancel();
    }
}
