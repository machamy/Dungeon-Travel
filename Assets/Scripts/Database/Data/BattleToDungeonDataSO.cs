using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BattleToDungeonDataSO", menuName = "Custom/BattleToDungeonDataSO")]
public class BattleToDungeonDataSO : ScriptableObject
{
    public bool isVictory;

    public void SetData(bool isVictory)
    {
        this.isVictory = isVictory;
    }
}
