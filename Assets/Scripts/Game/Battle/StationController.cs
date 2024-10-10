using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class StationController : MonoBehaviour
{
    Button button;

    public SpriteRenderer unitSprite;
    public Unit unit;
    public int stationNumber;
    public bool isTarget;

    bool isActive;
    

    void Awake()
    {
        isActive = false;
        isTarget = false;
    }

    public void SetUp()
    {
        button = gameObject.GetComponent<Button>();

        unitSprite = GetComponentInChildren<SpriteRenderer>();
        unit = GetComponentInChildren<Unit>();
        ActiveButton(true);
        isActive = true;
    }

    void ActiveButton(bool active)
    {
        button.enabled = active;
    }

    public void Select()
    {
        unit.UpdateOutline(true);
    }
    public void NonSelect()
    {
        unit.UpdateOutline(false);
    }

    public void OnClick()
    {
        unit.UpdateOutline(false);
        isTarget = true;
    }

    void Cancel()
    {
        unit.UpdateOutline(false);
        isTarget = false;
    }
}
