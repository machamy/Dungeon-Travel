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

    public BattleUnit unit;
    public int stationNumber;
    public bool isSelected;
    public bool isTarget;
    

    void Awake()
    {
        isTarget = false;
    }

    public void SetUp()
    {
        button = gameObject.GetComponent<Button>();
        unit = GetComponentInChildren<BattleUnit>();
        button.enabled = true;
    }

    public void Select()
    {
        isSelected = true;
        unit.UpdateOutline(true);
    }
    public void NonSelect()
    {
        isSelected = false;
        unit.UpdateOutline(false);
    }

    public void Target()
    {
        if (this.enabled == false) return;
        isTarget = true;
        unit.UpdateOutline(true);
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
