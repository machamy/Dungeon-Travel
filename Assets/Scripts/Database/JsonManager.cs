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

        public void SaveJSON<U, V>(Dictionary<U, V> dict) => SaveJSON(Dict2JSON(dict));

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