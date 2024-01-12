
using System.Collections.Generic;

using JetBrains.Annotations;

using UnityEngine.Events;


/// <summary>
/// 게임 DB가 담기는 싱글턴 클래스
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
    
    private Dictionary<string, string[]> data = new();
    public Dictionary<string, string[]> Data => data;
    

    public void SetData(string key, string[] value)
    {
        data[key]= (value);
    }

    public void SetData(Dictionary<string, string[]> datadict)
    {
        foreach (var pair in datadict)
        {
            SetData(pair.Key,pair.Value);
        }
    }
    
    public static bool IsReady()
    {
        return Contains("PlayerSpeed");
    }
    public static bool Contains(string key)
    {
        return Instance.data.ContainsKey(key);
    }

    /// <summary>
    /// key를 이용해 DB데이터(strting)를 가져옴. 없을경우 null반환
    /// </summary>
    /// <param name="key">가져올 데이터</param>
    /// <returns>데이터, 없으면 null</returns>
    [CanBeNull]
    public static string[] GetRaw(string key)
    {
        if(Contains(key)) return Instance.data[key];
        return null;
    }

    public static int GetSize(string key)
    {
        return Instance.data[key].Length;
    }
    
    public static string GetOne(string key, int idx = 0)
    {
        return Instance.data[key][idx];
    }
}
