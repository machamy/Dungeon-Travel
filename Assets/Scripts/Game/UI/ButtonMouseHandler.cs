using Scripts.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonMouseHandler : MonoBehaviour, IPointerEnterHandler
{

    public void OnPointerEnter(PointerEventData eventData) =>
        UIManager.Instance.SelectButton(gameObject);

}
