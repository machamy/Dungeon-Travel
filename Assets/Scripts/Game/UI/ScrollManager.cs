using Scripts.Manager;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using static InputManager;

public class ScrollManager : MonoBehaviour
{
    public static ScrollManager Instance { get; private set; }
    private void Awake() => Instance = this; //temporary singleton

    public int height = 80, amount = 6;
    public int posY = 0;
    public int posN = 0, posNInScreen = 0;

    private GameObject selectedButton;

    private void OnEnable()
    {
        InputManager.Navigate += Navigate;
    }

    private void OnDisable()
    {
        InputManager.Navigate -= Navigate;
    }

    private void Navigate()
    {
        selectedButton = UIManager.Instance.GetSelectedButton();

        posN = selectedButton.transform.parent.parent.name == "Content" ?
            selectedButton.transform.parent.GetSiblingIndex() : posN;
        posNInScreen = posN - posY / height;

        if (posNInScreen == amount + 1)
        {
            posY += height;
            posNInScreen = amount;
        }
        else if (posNInScreen == -1)
        {
            posY -= height;
            posNInScreen = 0;
        }

        if (posN == 0) posY = posNInScreen = 0;

        transform.localPosition = new Vector3(316.5f, posY, 0);
    }

    public void Reset()
    {
        posN = posNInScreen = posY = 0;
    }

}
