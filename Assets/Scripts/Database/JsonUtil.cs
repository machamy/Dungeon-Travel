using System;
using System.Collections.Generic;
using System.IO;
using Scripts.Entity;
using UnityEngine;

namespace Script.Global
{
    [Serializable]
    public class DictionaryPair<U, V>
    {
        public U key;
        public V value;
    }
    
    [Serializable]
    public class DictionaryPairList<U,V>
    {
        public List<DictionaryPair<U,V>> data;
    }
    

    [Serializable]
    public class PlayerData
    {
        
        public Stat _stat;
        
        public int lv;
        //public List<Item> itemList;

    }
    
    public class JsonUtil
    {
        private static JsonUtil instance;

        public static JsonUtil Instance
        {
            get
            {
                if (instance != null)
                    return instance;
                instance = new JsonUtil();
                return instance;
            }
        }
        
        private string defaultPath;

        private void Awake()
        {
            defaultPath = Path.Combine(Application.persistentDataPath, "database");
        }

        /// <summary>
        /// 기본경로 설정. path 설정 안할시 Appdata에 저장됨
        /// </summary>
        /// <param name="name"></param>
        public void SetDefaultPath(string name) => SetDefaultPath(name, Application.persistentDataPath);

        public void SetDefaultPath(string name, string path)
        {
            this.defaultPath = Path.Combine(path, name);
        }

        /// <summary>
        /// 딕셔너리를 JSON으로 변환하는 함수
        /// </summary>
        /// <param name="dict">변환할 딕셔너리</param>
        /// <returns>JSON 문자열</returns>
        public string Dict2JSON<U,V>(Dictionary<U,V> dict)
        {
            string json;
            DictionaryPairList<U,V> saveData = new DictionaryPairList<U, V>();
            saveData.data = new List<DictionaryPair<U,V>>();
            foreach (var keyValuePair in dict)
            {
                var pair = new DictionaryPair<U,V>();
                pair.key = keyValuePair.Key;
                pair.value = keyValuePair.Value;
                saveData.data.Add(pair);
            }
            json = JsonUtility.ToJson(saveData, true);
            return json;
        }

        /// <summary>
        /// 데이터를 JSON으로 변환하는 함수
        /// </summary>
        /// <param name="data"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public string Data2JSON(object data)
        {
            string json;
            json = JsonUtility.ToJson(data, true);
            return json;
        }

        /// <summary>
        /// JSON을 저장된 path로 저장함.
        /// </summary>
        /// <remarks>SetPath로 경로 지정 필수!</remarks>
        /// <param name="dict">저장할 딕셔너리</param>
        public void SaveJSON<U, V>(Dictionary<U, V> dict, string path = null) => SaveJSON(Dict2JSON(dict), path);
        
        /// <summary>
        /// JSON을 저장된 path로 저장함.
        /// </summary>
        /// <remarks>SetPath로 경로 지정 필수!</remarks>
        /// <param name="dict">저장할 딕셔너리</param>
        public void SaveJSON(string json, string path = null)
        {
            if (path == null)
                path = defaultPath;
            path = path.Replace("\\", "/");
            //폴더가 없으면 생성
            var paths = path.Split("/");
            string currentPath = paths[0];
            for (int i = 1; i < paths.Length-1; i++)
            {
                currentPath = Path.Combine(currentPath, paths[i]);
                // Debug.Log(Directory.Exists(currentPath));
                // Debug.Log(currentPath);
                if (!Directory.Exists(currentPath))
                    Directory.CreateDirectory(currentPath);
            }
            
            File.WriteAllText(path,json);
        }

        public T LoadJson<T>(string path = null)
        {
            if (path == null)
                path = defaultPath;
            if (!File.Exists(path))
            {
                Debug.Log($"No File :{path}");
                return default(T);
            }
            string rawJson = File.ReadAllText(path);
            var result = JsonUtility.FromJson<T>(rawJson);
            return result;
        }

        public Dictionary<U,V> LoadJson<U,V>(string path = null)
        {
            if (path == null)
                path = defaultPath;
            DictionaryPairList<U,V> saveData = new DictionaryPairList<U,V>();
            Dictionary<U, V> result = new Dictionary<U, V>();
            if (!File.Exists(path))
            {
                Debug.Log($"No File :{path}");
                return result;
            }
            string rawJson = File.ReadAllText(path);
            saveData = JsonUtility.FromJson<DictionaryPairList<U,V>>(rawJson);
            foreach (var pair in saveData.data)
            {
                result.Add(pair.key,pair.value);
            }

            return result;
        }
    }
}