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

    private void Update()
    {
        posN = EventSystem.current.currentSelectedGameObject.
            transform.parent.GetSiblingIndex();
        posNInScreen = posN - posY / height;

        Debug.Log(posY + " " + posN + " " + posNInScreen);

        if (posNInScreen > amount)
        {
            posY += height;
            posNInScreen = amount;
        }
        else if (posNInScreen < 0)
        {
            posY -= height;
            posNInScreen = 0;
        }

        transform.localPosition = new Vector3(316.5f, posY, 0);
    }

    public void ScrollWheel(InputValue value)
    {
        int axis = (int)value.Get<Vector2>().y;
        if (axis == 0) return;
        axis /= Mathf.Abs(axis);

        int newN = posN - axis;
        int newY = posY - axis * height;
        if (newN == -1 || newN == transform.childCount) return;
        if (newY < 0 || newY > (transform.childCount - amount - 1) * height) newY = posY;

        UIManager.Instance.SelectButton(transform.GetChild(posN - axis).
            GetChild(1).gameObject);
        posY = newY;
        
    }

}