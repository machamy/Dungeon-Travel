using Scripts.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    public GameObject itemButton;

    public GameObject itemCanvas;
    public GameObject skillCanvas;
    public GameObject equipCanvas;
    public GameObject statusCanvas;
    public GameObject partyCanvas;
    public GameObject configCanvas;

    private void OnEnable()
    {
        UIManager.Instance.SelectButton(itemButton);
        InputManager.Cancel += Cancel;
    }

    private void OnDisable()
    {
        InputManager.Cancel -= Cancel;
    }

    private void Cancel()
    {
        UIStack.Instance.PopUI();
    }

    public void Item()
    {
        UIStack.Instance.PushUI(itemCanvas);
    }

    public void Skill()
    {
        UIStack.Instance.PushUI(skillCanvas);
    }

    public void Equip()
    {
        UIStack.Instance.PushUI(equipCanvas);
    }

    public void Status()
    {
        UIStack.Instance.PushUI(statusCanvas);
    }

    public void Party()
    {
        UIStack.Instance.PushUI(partyCanvas);
    }

    public void Config()
    {
        UIStack.Instance.PushUI(configCanvas);
    }
}
