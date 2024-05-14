using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    private void Awake()
    {
        MainUI.Cancel += Cancel;
    }

    private void Cancel()
    {
        UIStack.Instance.PopUI();
    }
}
