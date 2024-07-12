using Scripts.Manager;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class StatusUI : MonoBehaviour
{
    public GameObject[] tab = new GameObject[5];
    private int currentTab = 0;

    private void OnEnable()
    {
        InputManager.Cancel += Cancel;
        InputManager.XMove += XMove;
    }

    private void OnDisable()
    {
        InputManager.Cancel -= Cancel;
        InputManager.XMove -= XMove;
    }

    private void Cancel()
    {
        UIStack.Instance.PopUI();
    }


    private void XMove(InputValue value)
    {
        int axis = (int)value.Get<float>();
        if (axis == 0) return;

        tab[currentTab].SetActive(false);
        currentTab = (currentTab + axis + 5) % 5;
        tab[currentTab].SetActive(true);
    }
}
