using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonSelection : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private GameObject selectedButton;

    public void OnPointerEnter(PointerEventData eventData)
    {
        selectedButton = EventSystem.current.currentSelectedGameObject;
        EventSystem.current.SetSelectedGameObject(gameObject);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        EventSystem.current.SetSelectedGameObject(selectedButton);
    }

}
