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

    public float height = 86.5f;
    public int amount = 5;
    public float posY = 0;
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
        posNInScreen = posN - (int)Mathf.Round(posY / height);

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

        float posX = transform.localPosition.x;
        transform.localPosition = new Vector3(posX, posY, 0);
    }

    public void Reset()
    {
        posN = posNInScreen = 0; posY = 0;
    }

}
