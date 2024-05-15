using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    private void OnEnable()
    {
        MainUI.Cancel += Cancel;
    }

    private void OnDisable()
    {
        MainUI.Cancel -= Cancel;
    }

    private void Cancel()
    {
        UIStack.Instance.PopUI();
    }
}
