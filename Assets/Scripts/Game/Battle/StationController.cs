using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class StationController : MonoBehaviour
{
    private ActMenu actMenu;
    public SpriteRenderer unitSprite;
    public Unit unit;
    private bool isAlive;
    public int stationNumber;

    public void SetUp()
    {
        if (transform.childCount > 0)
        {
            unitSprite = GetComponentInChildren<SpriteRenderer>();
            unit = GetComponentInChildren<Unit>();
            isAlive = true;
        }
        else isAlive = false;
    }

    public void GetActMenu(ActMenu actMenu)
    {
        this.actMenu = actMenu;
    }

    public void Select()
    {
        if (isAlive) unit.UpdateOutline(true);
    }
    public void NonSelect()
    {
        if (isAlive) unit.UpdateOutline(false);
    }

    public void onClick()
    {
        if (isAlive)
        {
            if (stationNumber < 6) actMenu.SetTarget(stationNumber);
            else actMenu.SetTarget(stationNumber);
        }
    }
}
