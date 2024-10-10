using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleInvoker : MonoBehaviour
{
    public BattleManager battleManager;

    public void OnBattleStart()
    {
        battleManager.gameObject.SetActive(true);
    }
    public void OnEndBattle()
    {
        battleManager.Destroy();
    }
}
