using Scripts.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    private void OnEnable()
    {
        UIManager.Cancel += Cancel;
    }

    private void OnDisable()
    {
        UIManager.Cancel -= Cancel;
    }

    private void Cancel()
    {
        UIStack.Instance.PopUI();
    }
}
