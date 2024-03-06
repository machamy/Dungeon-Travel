using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using ExcelDataReader;
using UnityEngine;


/// <summary>
/// 개선필요
/// </summary>
public class ExcelReader
{
    private string path;
    
    public ExcelReader(string name = "Dungeon_Travel_stats.xlsx")
    {
       path = Path.Combine(Application.dataPath,name );
    }

    /// <summary>
    /// 각 시트별로 가져온다.
    /// </summary>
    /// <returns></returns>
    public IEnumerable<DataTable> Read()
    {
        using (var stream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read))
        {
            using (var reader = ExcelReaderFactory.CreateReader(stream))
            {
                var result = reader.AsDataSet();

                for (int sheet = 0; sheet < result.Tables.Count; sheet++)
                {
                    var Sheet = result.Tables[sheet];
                    yield return Sheet;
                }
            }
        }
    }
}
