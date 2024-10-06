using Scripts.Data;
using System;
using System.Data;
using UnityEngine;

namespace Database.Parser
{
    public class ClassStatDataParser : IDataParser<StatData[]>
    {
        private enum StatDataType : int
        {
            lv = 0,
            HP = 1,
            MP = 2,
            ATK = 3,
            DEF = 4,
            MDEF = 5,
            HIT = 6,
            EVASE = 7,
            CRIT = 8,
            STRCOR = 9,
            MAGCOR = 10,
            STR = 11,
            VIT = 12,
            MAG = 13,
            AGI = 14,
            LUK = 15,
            STATUP = 16
        }
        
        public StatData[] Parse(DataTable sheet, string[] header, int colNum)
        {
            StatData[] stats = new StatData[sheet.Rows.Count + 1];
            stats[0] = ScriptableObject.CreateInstance<StatData>();
            for (int i = 1; i < sheet.Rows.Count; i++)
            {
                StatData stat = ScriptableObject.CreateInstance<StatData>();
                var row = Array.ConvertAll(sheet.Rows[i].ItemArray,
                    p => ((p ?? "0").ToString().Replace("%", "").Replace("-", "0")));
                // Debug.Log(row[(int)StatDataType.lv]);
                int lv = (int)int.Parse(row[(int)StatDataType.lv]);
                stats[lv] = stat;

                stat.hp = float.Parse(row[(int)StatDataType.HP]);
                stat.mp = float.Parse(row[(int)StatDataType.MP]);
                stat.atk = float.Parse(row[(int)StatDataType.ATK]);
                stat.def = float.Parse(row[(int)StatDataType.DEF]);
                stat.mdef = float.Parse(row[(int)StatDataType.MDEF]);

                stat.accuracy = float.Parse(row[(int)StatDataType.HIT]);
                stat.evase = float.Parse(row[(int)StatDataType.EVASE]);
                stat.critical = float.Parse(row[(int)StatDataType.CRIT]);
                stat.strWeight = float.Parse(row[(int)StatDataType.STRCOR]);
                stat.magWeight = float.Parse(row[(int)StatDataType.MAGCOR]);

                stat.str = float.Parse(row[(int)StatDataType.STR]);
                stat.vit = float.Parse(row[(int)StatDataType.VIT]);
                stat.mag = float.Parse(row[(int)StatDataType.MAG]);
                stat.agi = float.Parse(row[(int)StatDataType.AGI]);
                stat.luk = float.Parse(row[(int)StatDataType.LUK]);
                stat.statUp = float.Parse(row[(int)StatDataType.STATUP]);
            }
            return stats;
        }
    }
}