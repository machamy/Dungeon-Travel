
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using JetBrains.Annotations;
using Scripts.Data;
using Scripts.Entity;
using UnityEngine;
using UnityEngine.Events;


/// <summary>
/// 게임 DB가 담기는 싱글턴 클래스
/// TODO: 기획자에게 파일 받은 후 작업예정.
/// </summary>
public class DB
{
    private static DB instance;

    public static DB Instance
    {
        get
        {
            if (instance != null)
                return instance;
            instance = new DB();
            return instance;
        }
    }

    public UnityEvent OnDBUpdateEvent = new UnityEvent();

    private Dictionary<ClassType, StatData[]> classStatData = new();
    private Dictionary<ClassType, SkillData[]> classSkillData = new();
    private Dictionary<string, EquipmentData> Equipments = new();
    private Dictionary<string, EnemyStatData> enemyData = new();

    private string fileName = "Dungeon_Travel_stats.xlsx";

    private string DB_NAME_STAT = "STAT";
    private string DB_NAME_SKILL = "SKL";
    private string DB_NAME_EQUIPMENT = "EQUIPMENT";
    private string DB_NAME_ENEMY = "ENMY";


    public static StatData GetStatData(ClassType _class, int lv) 
    {
        return Instance.classStatData[_class][lv];
    }
    public static SkillData[] GetSkillDataArr(ClassType _class)
    {
        return Instance.classSkillData[_class];
    }
    public static EquipmentData GetEquipmentData(string name)
    {
        EquipmentData data;
        if (Instance.Equipments.TryGetValue(name, out data))
            return data;
        return null;
    }
    public static EnemyStatData GetEnemyData(string name)
    {
        EnemyStatData data;
        if (Instance.enemyData.TryGetValue(name, out data))
            return data;
        return null;
    }
    
    
    /// <summary>
    /// xlsx 에서 불어와 DB에 등록
    /// </summary>
    public void UpdateDB()
    {
        ExcelReader er = new ExcelReader(fileName);
        foreach (var table in er.Read())
        {
            string[] header = Array.ConvertAll(table.Rows[0].ItemArray,
                p => (p ?? String.Empty).ToString());
            int colNum;
            for (colNum = 0; colNum < header.Length; colNum++)
                if (header[colNum] == "EOF")
                    break;
            string[] sheetName = table.TableName.Split("_");

            if (sheetName.Any((e) => e.Equals(DB_NAME_STAT)))
            {
                classStatData.Add(ClassTypeHelper.FromCodename(sheetName[0]), ParseClassStat(table, header, colNum));
            }
            else if (sheetName.Any((e) => e.Equals(DB_NAME_SKILL)))
            {
                classSkillData.Add(ClassTypeHelper.FromCodename(sheetName[0]), ParseClassSkill(table, header, colNum));
            }
            else if (sheetName.Any((e) => e.Equals(DB_NAME_EQUIPMENT)))
            {
                var dict = ParseEquipment(table, header, colNum).ToDictionary((e => e.name), e => e);
                Equipments = dict;
            }
            else if (sheetName.Any((e) => e.Equals(DB_NAME_ENEMY)))
            {
                ParseEnemyData(table, header, colNum);
            }
        }
    }
    
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


    /// <summary>
    /// 데이터 시트의 내용을 StatData배열로 받아온다.
    /// </summary>
    /// <param name="sheet"></param>
    /// <param name="header"></param>
    /// <param name="colNum"></param>
    /// <returns>StatData배열</returns>
    private StatData[] ParseClassStat(DataTable sheet, string[] header, int colNum)
    {
        StatData[] stats = new StatData[sheet.Rows.Count+1];
        stats[0] = new StatData();
        for (int i = 1 ; i <= sheet.Rows.Count; i++)
        {
            StatData stat = ScriptableObject.CreateInstance<StatData>();
            var row = Array.ConvertAll(sheet.Rows[i].ItemArray,
                p => float.Parse((p ?? "0").ToString()));
            int lv = (int) row[(int)StatDataType.lv];
            stats[lv] = stat;
            
            stat.hp = row[(int)StatDataType.HP];
            stat.mp = row[(int)StatDataType.MP];
            stat.atk = row[(int)StatDataType.ATK];
            stat.def = row[(int)StatDataType.DEF];
            stat.mdef = row[(int)StatDataType.MDEF];
            
            stat.accuracy = row[(int)StatDataType.HIT];
            stat.dodge = row[(int)StatDataType.EVASE];
            stat.critical = row[(int)StatDataType.CRIT];
            stat.strWeight = row[(int)StatDataType.STRCOR];
            stat.magWeight = row[(int)StatDataType.MAGCOR];
            
            stat.str = row[(int)StatDataType.STR];
            stat.vit = row[(int)StatDataType.VIT];
            stat.mag = row[(int)StatDataType.MAG];
            stat.agi = row[(int)StatDataType.AGI];
            stat.luk = row[(int)StatDataType.LUK];
            stat.vitWeight = row[(int)StatDataType.STATUP];
        }
        return stats;
    }
    private enum SkillDataType : int
    {
        무기유형 = 0,
        랭크 = 1,
        유형 = 2,
        이름 = 3,
        데미지 = 4,
        MP소모 = 5,
    }
    
    /// <summary>
    /// 데이터 시트의 내용을 스킬데이터[]로 받아온다.
    /// </summary>
    /// <param name="sheet"></param>
    /// <param name="header"></param>
    /// <param name="colNum"></param>
    /// <returns></returns>
    private SkillData[] ParseClassSkill(DataTable sheet, string[] header, int colNum)
    {
        SkillData[] skills = new SkillData[sheet.Rows.Count+1];
        for (int i = 1; i <= sheet.Rows.Count; i++)
        {
            SkillData skill = ScriptableObject.CreateInstance<SkillData>();
            var row = Array.ConvertAll(sheet.Rows[i].ItemArray,
                p => (p ?? String.Empty).ToString());
            skill.skillType = row[(int)SkillDataType.무기유형];
            skill.rank = Convert.ToInt32(row[(int)SkillDataType.랭크]);
            skill.atttackType = row[(int)SkillDataType.유형];
            skill.name = row[(int)SkillDataType.이름];
            skill.damage = Convert.ToSingle(row[(int)SkillDataType.데미지]);
            skill.mpCost = Convert.ToSingle(row[(int)SkillDataType.MP소모]);
        }

        return skills;
    }
    
    /// <summary>
    /// 데이터시트의 내용을 장비[]
    /// </summary>
    /// <param name="sheet"></param>
    /// <param name="header"></param>
    /// <param name="colNum"></param>
    /// <returns></returns>
    private EquipmentData[] ParseEquipment(DataTable sheet, string[] header, int colNum)
    {
        EquipmentData[] skills = new EquipmentData[sheet.Rows.Count+1];
        return skills;
    }

    private enum EnemyStatType
    {
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
    private void ParseEnemyData(DataTable sheet, string[] header, int colNum)
    {
        for(int i=1; i<sheet.Rows.Count; i++)
        {
            EnemyStatData enemyStat = ScriptableObject.CreateInstance<EnemyStatData>();
            var row = Array.ConvertAll(sheet.Rows[i].ItemArray, p => ((p ?? "0").ToString()));
            
            enemyStat.name = row[(int)EnemyStatType.NAME];
            enemyStat.hp = float.Parse(row[(int)EnemyStatType.HP]);
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
                if (header[idx] == "선공")
                    property |= EnemyProperty.Hostile;
                if (header[idx] == "행동패턴")
                    property |= EnemyProperty.Movement;
            }

            AttackType strongType = AttackType.None;
            AttackType weakType = AttackType.None;
            for (; idx < header.Length; idx++)
            {
                AttackType currentType = AttackTypeHelper.GetFromKorean(header[idx]);
                if(currentType == AttackType.None)
                {
                    if(row[idx].Contains("R"))
                        strongType |= currentType;
                    if(row[idx].Contains("W"))
                        weakType |= currentType;
                }
                else
                {
                    Debug.Log($"[DB::ParseEnemyData] {header[idx]} 유효하지 않음");
                }
            }
            
            enemyData.Add(enemyStat.name, enemyStat);
        }
    }
}
