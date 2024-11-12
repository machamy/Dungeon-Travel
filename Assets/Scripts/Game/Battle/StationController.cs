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

    public Unit unit;
    public int stationNumber;
    public bool isTarget;
    

    void Awake()
    {
        isTarget = false;
    }

    public void SetUp()
    {
        button = gameObject.GetComponent<Button>();
        unit = GetComponentInChildren<Unit>();
        button.enabled = true;
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

    public void Dead()
    {
        button.enabled = false;
    }
}
