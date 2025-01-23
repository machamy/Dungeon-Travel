using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class StationController : MonoBehaviour
{
    public Button button;

    public BattleUnit battleUnit;
    public int stationNumber;
    public bool isSelected;
    public bool isTarget;

    void OnEnable()
    {
        isTarget = false;
        button = GetComponent<Button>();
        button.enabled = false;
    }

    public void Initailize(BattleUnit battleUnit = null)
    {
        isTarget = false;
        if (battleUnit != null) this.battleUnit = battleUnit;
        Debug.Log(this.battleUnit);
        Debug.Log(battleUnit);
        button = GetComponent<Button>();
        button.enabled = true;

    }

    public void Select()
    {
        isSelected = true;
        battleUnit.UpdateOutline(true);
    }
    public void NonSelect()
    {
        isSelected = false;
        battleUnit.UpdateOutline(false);
    }

    public void Target()
    {
        if (battleUnit == null) return;
        isTarget = true;
        battleUnit.UpdateOutline(true);
    }

    public void NonTarget()
    {
        if (battleUnit == null) return;
        isTarget = false;

        battleUnit.UpdateOutline(false);
    }

    public void OnClick()
    {
        battleUnit.UpdateOutline(false);
        isTarget = true;
    }

    public BattlePlayerUnit GetBattlePlayerUnit()
    {
        BattlePlayerUnit battlePlayerUnit = null;
        if (battleUnit.gameObject.GetComponent<BattlePlayerUnit>() != null)
            battlePlayerUnit = battleUnit.gameObject.GetComponent<BattlePlayerUnit>();
        return battlePlayerUnit;
    }
}
