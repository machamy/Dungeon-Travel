using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DungeonToBattleDataSO", menuName = "Custom/DungeonToBattleDataSO")]
public class DungeonToBattleDataSO : ScriptableObject
{
    public Dictionary<string, int> enemyList = new Dictionary<string, int>();
    public int floor;

    public void SetData(Dictionary<string, int> enemyList, int floor)
    {
        this.enemyList = enemyList;
        this.floor = floor;
    }

    public void DebugDungeonToBattle()
    {
        foreach (KeyValuePair<string, int> data in enemyList)
        {
            Debug.Log($"{data.Key},{data.Value}");
        }
    }
}
