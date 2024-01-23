using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class MainUI : MonoBehaviour
{
    public GameObject placeFirstSelect;
    public PlayerInput playerInput;
    private State currentState = State.selectPlace;
    string beforeSelectButtonName = "DungeonCircle", selectButtonName = "DungeonCircle";

    public GameObject dungeonCircle, guildCircle, shopCircle;
    private Color red = new(1, 0, 0, 0.5f), yellow = new(1, 1, 0, 0.5f);

    public enum State
    {
        selectPlace
    }

    public void SelectPlace()
    {
        currentState = State.selectPlace;

        SelectButton(placeFirstSelect);
    }

    public void OnCancel()
    {
        switch (currentState)
        {
            
        }
    }

    public void OnSubmit()
    {
        switch (currentState)
        {
            
        }
    }

    public void OnClick()
    {
        switch (currentState)
        {
            
        }
    }

    private void Update()
    {
        if (EventSystem.current.currentSelectedGameObject != null)
            selectButtonName = EventSystem.current.currentSelectedGameObject.name;

        if (beforeSelectButtonName != selectButtonName)
        {
            if (selectButtonName == "Dungeon")
            {
                dungeonCircle.GetComponent<Image>().color = yellow;
                guildCircle.GetComponent<Image>().color = red;
                shopCircle.GetComponent<Image>().color = red;
            }
            else if (selectButtonName == "Guild")
            {
                dungeonCircle.GetComponent<Image>().color = red;
                guildCircle.GetComponent<Image>().color = yellow;
                shopCircle.GetComponent<Image>().color = red;
            }
            else if (selectButtonName == "Shop")
            {
                dungeonCircle.GetComponent<Image>().color = red;
                guildCircle.GetComponent<Image>().color = red;
                shopCircle.GetComponent<Image>().color = yellow;
            }

            beforeSelectButtonName = selectButtonName;
        }
           
        
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
