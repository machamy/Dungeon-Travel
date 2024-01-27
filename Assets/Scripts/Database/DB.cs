
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using JetBrains.Annotations;
using Scripts.Data;
using Scripts.Entity;
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
        적중률 = 6,
        회피율 = 7,
        크리율 = 8,
        근력보정 = 9,
        마법보정 = 10,
        lv2 = 11,
        STR = 12,
        VIT = 13,
        MAG = 14,
        AGI = 15,
        LUK = 16,
        VIT보정 = 17
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
            StatData stat = new StatData();
            var row = Array.ConvertAll(sheet.Rows[i].ItemArray,
                p => float.Parse((p ?? "0").ToString()));
            int lv = (int) row[0];
            stats[lv] = stat;
            
            stat.hp = row[(int)StatDataType.HP];
            stat.mp = row[(int)StatDataType.MP];
            stat.atk = row[(int)StatDataType.ATK];
            stat.def = row[(int)StatDataType.DEF];
            stat.mdef = row[(int)StatDataType.MDEF];
            
            stat.accuracy = row[(int)StatDataType.적중률];
            stat.dodge = row[(int)StatDataType.회피율];
            stat.critical = row[(int)StatDataType.크리율];
            stat.strWeight = row[(int)StatDataType.근력보정];
            stat.magWeight = row[(int)StatDataType.마법보정];
            
            stat.str = row[(int)StatDataType.STR];
            stat.vit = row[(int)StatDataType.VIT];
            stat.mag = row[(int)StatDataType.MAG];
            stat.agi = row[(int)StatDataType.AGI];
            stat.luk = row[(int)StatDataType.LUK];
            stat.vitWeight = row[(int)StatDataType.VIT보정];
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
            SkillData skill = new SkillData();
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
        적중률,
        회피율,
        크리율,
        STRCRET,
        MAGCRET,
        STR,
        VIT,
        MAG,
        AGI,
        LUK,
    }
    private enum EnemyWeakType
    {
        SLASH = 15,
        PENETRATE,
        SMASH,
        FLAME,
        ICE,
        WIND,
        LIGHTNING,
        SHINING,
        DARK,
    }
    private void ParseEnemyData(DataTable sheet, string[] header, int colNum)
    {
        for(int i=1; i<sheet.Rows.Count; i++)
        {
            EnemyStatData enemyStat = new EnemyStatData();
            var row = Array.ConvertAll(sheet.Rows[i].ItemArray,
                p => float.Parse((p ?? "0").ToString()));
            enemyStat.name = Convert.ToString(row[(int)EnemyStatType.NAME]);
            enemyStat.hp = row[(int)EnemyStatType.HP];
            enemyStat.atk = row[(int)EnemyStatType.ATK];
            enemyStat.def = row[(int)EnemyStatType.DEF];
            enemyStat.mdef = row[(int)EnemyStatType.MDEF];
            enemyStat.accuracy = row[(int)EnemyStatType.적중률];
            enemyStat.dodge = row[(int)EnemyStatType.회피율];
            enemyStat.critical = row[(int)EnemyStatType.크리율];
            enemyStat.strcret = row[(int)EnemyStatType.STRCRET];
            enemyStat.magcret = row[(int)EnemyStatType.MAGCRET];
            enemyStat.str = row[(int)EnemyStatType.STR];
            enemyStat.vit = row[(int)EnemyStatType.VIT];
            enemyStat.mag = row[(int)EnemyStatType.MAG];
            enemyStat.agi = row[(int)EnemyStatType.AGI];
            enemyStat.luk = row[(int)EnemyStatType.LUK];
            enemyData.Add(enemyStat.name, enemyStat);
        }
    }
}
