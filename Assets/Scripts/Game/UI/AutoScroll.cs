using Scripts.Manager;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class AutoScroll : MonoBehaviour
{
    public static AutoScroll Instance { get; private set; }
    private void Awake() => Instance = this; //temporary singleton

    public int height = 80, amount = 6;
    public int posY = 0;
    public int posN, posNInScreen = 0;

    private GameObject selectedButton;

    private void Update()
    {
        selectedButton = UIManager.Instance.GetSelectedButton();

        posN = selectedButton.transform.parent.GetSiblingIndex();
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


}
