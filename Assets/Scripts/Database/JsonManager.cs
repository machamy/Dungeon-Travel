using System;
using System.Collections.Generic;
using System.IO;
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
    
    public class JsonManager : MonoBehaviour
    {
        private string path;

        private void Awake()
        {
            path = Path.Combine(Application.persistentDataPath, "database");
        }

        /// <summary>
        /// 경로 설정. path 설정 안할시 Appdata에 저장됨
        /// </summary>
        /// <param name="name"></param>
        public void SetPath(string name) => SetPath(name, Application.persistentDataPath);

        public void SetPath(string name, string path)
        {
            this.path = Path.Combine(path, name);
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
        /// JSON을 저장된 path로 저장함.
        /// </summary>
        /// <remarks>SetPath로 경로 지정 필수!</remarks>
        /// <param name="dict">저장할 딕셔너리</param>
        public void SaveJSON<U, V>(Dictionary<U, V> dict) => SaveJSON(Dict2JSON(dict));
        
        /// <summary>
        /// JSON을 저장된 path로 저장함.
        /// </summary>
        /// <remarks>SetPath로 경로 지정 필수!</remarks>
        /// <param name="dict">저장할 딕셔너리</param>
        public void SaveJSON(string json)
        {
            File.WriteAllText(path,json);
        }

        public Dictionary<U,V> LoadJson<U,V>()
        {
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