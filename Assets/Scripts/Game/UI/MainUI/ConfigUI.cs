using Scripts.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static InputManager;
using UnityEngine.UI;

public class ConfigUI : MonoBehaviour
{

    public GameObject videoButton;
    public GameObject resolutionButton;
    private GameObject selectedType;


    private Color lightblue = new(0, 1, 1, 0.5f), blue = new(0, 0.5f, 1, 0.5f);

    public enum UIDepth
    {
        Type, Detail
    }
    private UIDepth depth = UIDepth.Type;

    private void OnEnable()
    {
        UIManager.Instance.SelectButton(videoButton);
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
            case UIDepth.Type:
                UIStack.Instance.PopUI();
                break;
            case UIDepth.Detail:
                UIManager.Instance.SelectButton(selectedType);
                selectedType.GetComponent<Image>().color = lightblue;
                depth--;
                break;
        }
    }

    public void Video()
    {
        selectedType = UIManager.Instance.GetSelectedButton();
        selectedType.GetComponent<Image>().color = blue;
        UIManager.Instance.SelectButton(resolutionButton);
        depth = UIDepth.Detail;
    }
}
