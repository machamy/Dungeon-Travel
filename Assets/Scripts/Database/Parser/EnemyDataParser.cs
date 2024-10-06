using Scripts.Data;
using System;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

namespace Database.Parser
{
    public class EnemyDataParser : IDataParser<Dictionary<string, EnemyStatData>>
    {
        private enum EnemyStatType
        {
            INDEX,
            NAME,
            HP,
            ATK,
            DEF,
            MDEF,
            HIT,
            AVOID,
            CRIT,
            STRCRET,
            MAGCRET,
            STR,
            VIT,
            MAG,
            AGI,
            LUK,
        }
    
        public Dictionary<string, EnemyStatData> Parse(DataTable sheet, string[] header, int colNum)
        {
            Dictionary<string, EnemyStatData> result = new Dictionary<string, EnemyStatData>();

            for (int i = 1; i < sheet.Rows.Count; i++)
            {
                EnemyStatData enemyStat = ScriptableObject.CreateInstance<EnemyStatData>();
                var row = Array.ConvertAll(sheet.Rows[i].ItemArray, p => ((p ?? "0").ToString()));

                enemyStat.name = row[(int)EnemyStatType.NAME];
                enemyStat.hp = int.Parse(row[(int)EnemyStatType.HP]);
                enemyStat.atk = float.Parse(row[(int)EnemyStatType.ATK]);
                enemyStat.def = float.Parse(row[(int)EnemyStatType.DEF]);
                enemyStat.mdef = float.Parse(row[(int)EnemyStatType.MDEF]);

                enemyStat.accuracy = float.Parse(row[(int)EnemyStatType.HIT]);
                enemyStat.dodge = float.Parse(row[(int)EnemyStatType.AVOID]);
                enemyStat.critical = float.Parse(row[(int)EnemyStatType.CRIT]);
                enemyStat.strcret = float.Parse(row[(int)EnemyStatType.STRCRET]);
                enemyStat.magcret = float.Parse(row[(int)EnemyStatType.MAGCRET]);

                enemyStat.str = float.Parse(row[(int)EnemyStatType.STR]);
                enemyStat.vit = float.Parse(row[(int)EnemyStatType.VIT]);
                enemyStat.mag = float.Parse(row[(int)EnemyStatType.MAG]);
                enemyStat.agi = float.Parse(row[(int)EnemyStatType.AGI]);
                enemyStat.luk = float.Parse(row[(int)EnemyStatType.LUK]);

                // 현재 열 위치
                int idx = (int)EnemyStatType.LUK;
                EnemyProperty property = EnemyProperty.None;
                for (int delta = 0; delta < 2; delta++)
                {
                    idx += 1;
                    if (!row[idx].ToLower().Contains("true"))
                        continue;
                    if (header[idx] == "선공")
                        property |= EnemyProperty.Hostile;
                    else if (header[idx] == "행동패턴")
                        property |= EnemyProperty.Movement;
                }

                enemyStat.Property = property;

                AttackType registerType = AttackType.None;
                AttackType weakType = AttackType.None;
                for (idx++; idx < header.Length; idx++)
                {
                    string rawHeader = header[idx];
                    if (rawHeader == String.Empty)
                        continue;
                    AttackType currentType = AttackTypeHelper.GetFromKorean(rawHeader);
                    if (currentType != AttackType.None)
                    {
                        if (row[idx].Contains("R"))
                            registerType |= currentType;
                        if (row[idx].Contains("W"))
                            weakType |= currentType;
                    }
                    else
                    {
                        Debug.Log($"[DB::ParseEnemyData] {header[idx]} 유효하지 않음");
                    }
                }

                enemyStat.resistType = registerType;
                enemyStat.weakType = weakType;

                result.Add(enemyStat.name, enemyStat);
                Debug.Log($"[DB::ParseEnemyData] Added {enemyStat.name} : Property {enemyStat.Property} ResistType {enemyStat.resistType} WeakType {enemyStat.weakType}");

            }

            return result; 
        }
    }
}