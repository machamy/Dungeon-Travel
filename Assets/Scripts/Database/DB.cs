using Database.Parser;
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
/// TODO: 파싱 함수 별도 클래스로 분리
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
    private List<Dictionary<string, StatData>> enemyDataList = new();
    private List<Dictionary<string, List<SkillData>>> enemySkillList = new();

    private string fileName = "Dungeon_Travel_stats.xlsx";

    #region 상수 필드
    public const string DB_NAME_STAT = "STAT";
    public const string DB_NAME_SKILL = "SKL";
    public const string DB_NAME_EQUIPMENT = "EQUIPMENT";
    public const string DB_NAME_ENEMY = "ENMY";
    public const string DB_NAME_ENEMYSKILL = "ENMYSKL";

    public const string UI_INTERACTION_NAME = "상호작용";

    #endregion




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
    /// <summary>
    /// 해당 층의 해당 이름의 몬스터를 가져온다.
    /// </summary>
    /// <param name="floor"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public static StatData GetEnemyData(int floor, string name)
    {
        StatData data;
        var floorData = Instance.enemyDataList[floor];
        if (floorData.TryGetValue(name, out data))
            return data;
        return null;
    }
    /// <summary>
    /// 해당 층의 해당 이름의 몬스터의 스킬을 가져온다.
    /// </summary>
    /// <param name="floor"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public static List<SkillData> GetEnemySkillData(int floor, string name)
    {
        List<SkillData> data;
        var floorData = Instance.enemySkillList[floor];
        if (floorData.TryGetValue(name, out data))
            return data;
        return null;
    }

    /// <summary>
    /// 해당 층의 모든 몬스터 데이터를 가져온다
    /// </summary>
    /// <param name="floor"></param>
    /// <returns></returns>
    public static Dictionary<string, StatData> GetEnemyAllData(int floor)
    {
        return Instance.enemyDataList[floor];
    }

    public void ClearDB()
    {
        classStatData = new();
        classSkillData = new();
        Equipments = new();
        enemyDataList = new();

        classSkillData.Add(ClassType.Null, new SkillData[] { });
        classStatData.Add(ClassType.Null, new StatData[] { });

    }

    /// <summary>
    /// xlsx 에서 불어와 DB에 등록
    /// </summary>
    public void UpdateDB()
    {
        ExcelReader er = new ExcelReader(fileName);
        
        ClassStatDataParser classStatDataParser = new ClassStatDataParser();
        ClassSkillDataParser classSkillDataParser = new ClassSkillDataParser();
        
        EnemySkillDataParser enemySkillDataParser = new EnemySkillDataParser();
        EnemyDataParser enemyDataParser = new EnemyDataParser();
        
        ClearDB(); // << 분리할수도 있음
        foreach (var table in er.Read())
        {
            string[] header = Array.ConvertAll(table.Rows[0].ItemArray,
                p => (p ?? String.Empty).ToString())
                .Where(h => !string.IsNullOrEmpty(h)) // 빈 문자열 필터링
                .ToArray();
            int colNum = -1; // 미사용
            // for (colNum = 0; colNum < header.Length; colNum++)
            //     if (header[colNum] == "EOF")
            //         break;
            string[] sheetName = table.TableName.Split("_");
            Debug.Log("[DB::UpdateDB] parsing sheet : " + table.TableName);
            switch (sheetName.Length)
            {
                case 1:
                    if (sheetName[0].Equals(DB_NAME_EQUIPMENT))
                    {
                        // TODO : 데이터 기다리는중 - machamy
                        // var dict = ParseEquipment(table, header, colNum).ToDictionary((e => e.name), e => e);
                        // Equipments = dict;
                    }
                    break;
                case 2:
                    if (sheetName[1].Equals(DB_NAME_STAT))
                    {
                        // Debug.Log(classStatData[ClassType.Paladin]);
                        classStatData.Add(ClassTypeHelper.FromCodename(sheetName[0]), classStatDataParser.Parse(table, header, colNum));
                    }
                    else if (sheetName[1].Equals(DB_NAME_SKILL))
                    {
                        ClassType classType = ClassTypeHelper.FromCodename(sheetName[0]);
                        classSkillData.Add(classType,
                            classSkillDataParser.SetClassType(classType).Parse(table, header, colNum));
                    }
                    else if (sheetName[1].Equals(DB_NAME_ENEMYSKILL))
                    {
                        int floor = int.Parse(sheetName[0].Replace("F", ""));
                        while (enemySkillList.Count <= floor)
                            enemySkillList.Add(new Dictionary<string, List<SkillData>>());
                        enemySkillList[floor] = enemySkillDataParser.Parse(table, header, colNum);
                    }
                    break;
                case 3:
                    if (sheetName[1].Equals(DB_NAME_ENEMY))
                    {
                        int floor = int.Parse(sheetName[0].Replace("F", ""));
                        while (enemyDataList.Count <= floor)
                            enemyDataList.Add(new Dictionary<string, StatData>());
                        enemyDataList[floor] = enemyDataParser.Parse(table, header, colNum);
                    }
                    break;
            }
        }

        OnEndUpdate();
    }

    private void OnEndUpdate()
    {
        Class.InitClassList();
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
        EquipmentData[] skills = new EquipmentData[sheet.Rows.Count + 1];
        return skills;
    }





    
}
