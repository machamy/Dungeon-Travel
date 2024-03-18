
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
    private List<Dictionary<string, EnemyStatData>> enemyDataList = new();
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
    public static EnemyStatData GetEnemyData(int floor, string name)
    {
        EnemyStatData data;
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
    public static Dictionary<string, EnemyStatData> GetEnemyAllData(int floor)
    {
        return Instance.enemyDataList[floor];
    }

    public void ClearDB()
    {
        classStatData = new();
        classSkillData = new();
        Equipments = new();
        enemyDataList = new();
        
        classSkillData.Add(ClassType.Null,new SkillData[]{});
        classStatData.Add(ClassType.Null,new StatData[]{});
        
    }
    
    /// <summary>
    /// xlsx 에서 불어와 DB에 등록
    /// </summary>
    public void UpdateDB()
    {
        ExcelReader er = new ExcelReader(fileName);
        ClearDB(); // << 분리할수도 있음
        foreach (var table in er.Read())
        {
            string[] header = Array.ConvertAll(table.Rows[0].ItemArray,
                p => (p ?? String.Empty).ToString());
            int colNum = -1; // 미사용
            // for (colNum = 0; colNum < header.Length; colNum++)
            //     if (header[colNum] == "EOF")
            //         break;
            string[] sheetName = table.TableName.Split("_");
            Debug.Log("[DB::UpdateDB] parsing sheet : "+table.TableName);
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
                        classStatData.Add(ClassTypeHelper.FromCodename(sheetName[0]), ParseClassStat(table, header, colNum));
                    }
                    else if (sheetName[1].Equals(DB_NAME_SKILL))
                    {
                        ClassType classType = ClassTypeHelper.FromCodename(sheetName[0]);
                        classSkillData.Add(classType, ParseClassSkill(table, header, colNum, classType));
                    }
                    else if (sheetName[1].Equals(DB_NAME_ENEMYSKILL))
                    {
                        int floor = int.Parse(sheetName[0].Replace("F", ""));
                        while (enemySkillList.Count <= floor)
                            enemySkillList.Add(new Dictionary<string, List<SkillData>>());
                        enemySkillList[floor] = ParseEnemySkillData(table, header, colNum);
                    }
                    break;
                case 3:
                    if (sheetName[1].Equals(DB_NAME_ENEMY))
                    {
                        int floor = int.Parse(sheetName[0].Replace("F", ""));
                        while(enemyDataList.Count <= floor)
                            enemyDataList.Add(new Dictionary<string, EnemyStatData>());
                        enemyDataList[floor] = ParseEnemyData(table, header, colNum);
                    }
                    break;
            }
        }
        
        EndDBupdate();
    }

    private void EndDBupdate()
    {
        Class.InitClassList();
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
        stats[0] = ScriptableObject.CreateInstance<StatData>();
        for (int i = 1 ; i < sheet.Rows.Count; i++)
        {
            StatData stat = ScriptableObject.CreateInstance<StatData>();
            var row = Array.ConvertAll(sheet.Rows[i].ItemArray,
                p => ((p ?? "0").ToString().Replace("%", "").Replace("-", "0")));
            // Debug.Log(row[(int)StatDataType.lv]);
            int lv = (int) int.Parse(row[(int)StatDataType.lv]);
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
    private enum SkillDataType : int
    {
        무기유형 = 0,
        랭크 = 1,
        유형 = 2,
        이름 = 3,
        물리_데미지 = 4,
        속성_데미지 = 5,
        MP소모 = 6,
  
        LastIdx = 6
    }
    
    /// <summary>
    /// 데이터 시트의 내용을 스킬데이터[]로 받아온다.
    /// </summary>
    /// <param name="sheet"></param>
    /// <param name="header"></param>
    /// <param name="colNum"></param>
    /// <returns></returns>
    private SkillData[] ParseClassSkill(DataTable sheet, string[] header, int colNum, ClassType classType = ClassType.Null)
    {
        SkillData[] skills = new SkillData[sheet.Rows.Count+1];
        for (int i = 1; i < sheet.Rows.Count; i++)
        {
            SkillData skill = ScriptableObject.CreateInstance<SkillData>();
            var row = Array.ConvertAll(sheet.Rows[i].ItemArray,
                p => (p ?? String.Empty).ToString());
            if(row[(int)SkillDataType.이름] == "")
                continue;

            skill.classType = classType;
            
            skill.weaponType = row[(int)SkillDataType.무기유형];
            
            skill.rank = Convert.ToInt32(row[(int)SkillDataType.랭크]);
            skill.rawType = row[(int)SkillDataType.유형];
            skill.name = row[(int)SkillDataType.이름];
            skill.skillName = skill.name;
            
            skill.physicsDamage = Convert.ToSingle(row[(int)SkillDataType.물리_데미지] == string.Empty ? "0" : row[(int)SkillDataType.물리_데미지]);
            skill.propertyDamage = Convert.ToSingle(row[(int)SkillDataType.속성_데미지] == string.Empty ? "0" : row[(int)SkillDataType.속성_데미지]);
            skill.mpCost = Convert.ToSingle(row[(int)SkillDataType.MP소모] == string.Empty ? "0" : row[(int)SkillDataType.MP소모]);

            var booleanArr = row.Skip((int)SkillDataType.LastIdx + 1).Select((a) => a.ToUpper() == "TRUE").ToArray();
            int idx = 0;
            skill.isPassive = booleanArr[idx++];
            skill.isSelf = booleanArr[idx++];

            
            TargetType Ally = TargetType.None;
            if(booleanArr[idx++])
                Ally |= TargetType.Single;
            if(booleanArr[idx++])
                Ally |= TargetType.Front;
            if(booleanArr[idx++])
                Ally |= TargetType.Back;
            if(booleanArr[idx++])
                Ally |= TargetType.Area;
            
            TargetType Enemy = TargetType.None;
            if(booleanArr[idx++])
                Enemy |= TargetType.Single;
            if(booleanArr[idx++])
                Enemy |= TargetType.Front;
            if(booleanArr[idx++])
                Enemy |= TargetType.Back;
            if(booleanArr[idx++])
                Enemy |= TargetType.Area;

            skill.allyTargetType = Ally;
            skill.enemyTargetType = Enemy;
            
            skill.isBuff = booleanArr[idx++];
            skill.isDebuff = booleanArr[idx++];
            skill.isHealing = booleanArr[idx++];
            skill.isMelee= booleanArr[idx++];
            skill.isRanged= booleanArr[idx++];
            
            AttackType attackType = AttackType.None;
            for (; (int)SkillDataType.LastIdx+ 1+ idx < header.Length; idx++)
            {
                string rawHeader = header[(int)SkillDataType.LastIdx + 1 + idx];
                if(rawHeader == String.Empty)
                    continue;
                AttackType currentType = AttackTypeHelper.GetFromKorean(rawHeader);
                if(currentType != AttackType.None)
                {
                    if(booleanArr[idx])
                        attackType |= currentType;
                }
                else
                {
                    Debug.Log($"[DB::ParseEnemyData] {header[(int)SkillDataType.LastIdx+ 1 +idx]}({(int)SkillDataType.LastIdx+ 1 +idx}) 유효하지 않음");
                }
            }

            skill.attackType = attackType;
            
            Debug.Log($"Register Skill {skill}");
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
    private Dictionary<string, EnemyStatData> ParseEnemyData(DataTable sheet, string[] header, int colNum)
    {
        Dictionary<string, EnemyStatData> result = new Dictionary<string, EnemyStatData>();
        
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

            AttackType registerType = AttackType.None;
            AttackType weakType = AttackType.None;
            for (idx++ ; idx < header.Length; idx++)
            {
                string rawHeader = header[idx];
                if(rawHeader == String.Empty)
                    continue;
                AttackType currentType = AttackTypeHelper.GetFromKorean(rawHeader);
                if(currentType != AttackType.None)
                {
                    if(row[idx].Contains("R"))
                        registerType |= currentType;
                    if(row[idx].Contains("W"))
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

    private enum EnemySkillDataType
    {
        적이름,
        유형,
        스킬이름,
        물리데미지,
        속성데미지,
        가중치,
        LastIdx = 5,
    }
    private Dictionary<string, List<SkillData>> ParseEnemySkillData(DataTable sheet, string[] header, int colNum)
    {
        Dictionary<string, List<SkillData>> skills = new Dictionary<string, List<SkillData>> ();
        List<SkillData> skillList = new List<SkillData>();
        string postName = null;
        for (int i = 1; i < sheet.Rows.Count; i++)
        {
            SkillData skill = ScriptableObject.CreateInstance<SkillData>();
            var row = Array.ConvertAll(sheet.Rows[i].ItemArray,
                p => (p ?? String.Empty).ToString());
            if (row[(int)EnemySkillDataType.스킬이름] == "")
                continue;
            

            skill.enemyName = row[(int)EnemySkillDataType.적이름];
            skill.skillName = skill.name;

            skill.physicsDamage = Convert.ToSingle(row[(int)EnemySkillDataType.물리데미지] == string.Empty ? "0" : row[(int)EnemySkillDataType.물리데미지]);
            skill.propertyDamage = Convert.ToSingle(row[(int)EnemySkillDataType.속성데미지] == string.Empty ? "0" : row[(int)EnemySkillDataType.속성데미지]);
            skill.skillWeight = Convert.ToInt32(row[(int)EnemySkillDataType.가중치] == string.Empty ? "0" : row[(int)EnemySkillDataType.가중치]);

            var booleanArr = row.Skip((int)EnemySkillDataType.LastIdx + 1).Select((a) => a.ToUpper() == "TRUE").ToArray();
            int idx = 0;
            skill.isPassive = booleanArr[idx++];
            skill.isSelf = booleanArr[idx++];


            TargetType Ally = TargetType.None;
            if (booleanArr[idx++])
                Ally |= TargetType.Single;
            if (booleanArr[idx++])
                Ally |= TargetType.Front;
            if (booleanArr[idx++])
                Ally |= TargetType.Back;
            if (booleanArr[idx++])
                Ally |= TargetType.Area;

            TargetType Enemy = TargetType.None;
            if (booleanArr[idx++])
                Enemy |= TargetType.Single;
            if (booleanArr[idx++])
                Enemy |= TargetType.Front;
            if (booleanArr[idx++])
                Enemy |= TargetType.Back;
            if (booleanArr[idx++])
                Enemy |= TargetType.Area;

            skill.allyTargetType = Ally;
            skill.enemyTargetType = Enemy;

            skill.isBuff = booleanArr[idx++];
            skill.isDebuff = booleanArr[idx++];
            skill.isHealing = booleanArr[idx++];
            skill.isMelee = booleanArr[idx++];
            skill.isRanged = booleanArr[idx++];

            AttackType attackType = AttackType.None;
            for (; (int)SkillDataType.LastIdx + 1 + idx < header.Length; idx++)
            {
                string rawHeader = header[(int)SkillDataType.LastIdx + idx];
                if (rawHeader == String.Empty)
                    continue;
                AttackType currentType = AttackTypeHelper.GetFromKorean(rawHeader);
                if (currentType != AttackType.None)
                {
                    if (booleanArr[idx])
                        attackType |= currentType;
                }
                else
                {
                    Debug.Log($"[DB::ParseEnemySkillData] {header[(int)SkillDataType.LastIdx + 1 + idx]}({(int)SkillDataType.LastIdx + 1 + idx}) 유효하지 않음");
                }
            }

            skill.attackType = attackType;
            
            if(!skills.ContainsKey(skill.enemyName))
                skills.Add(skill.enemyName,new List<SkillData>());
            skills[skill.enemyName].Add(skill);
            
            
            
            // if(i == 1)
            // {
            //     skillList.Add(skill);
            // }
            // else if (i != sheet.Rows.Count - 1)
            // {
            //     skills.Add(postName, skillList);
            //     // 키가 없으면
            // }
            // else
            // {
            //     if (postName == skill.enemyName)
            //     {
            //         skillList.Add(skill);
            //     }
            //     else
            //     {
            //         skills.Add(postName, skillList);
            //         skillList = new List<SkillData>();
            //     }
            // }
            // postName = skill.enemyName;
            Debug.Log($"Register EnemySkill {skill}");
        }

        return skills;
    }
}
