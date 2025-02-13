using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Script.Global;
using Scripts.Entity;
using Scripts.Manager;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Scripts.Manager
{
    /// <summary>
    /// 게임매니저, 싱글턴
    /// </summary>
    public class SaveLoadManager
    {
        public const string NAME = "@SaveLoader";
        public string basePath;
        public string extension = ".savedata";
         
        private static SaveLoadManager instance;
        private SaveData currentSave;

        public SaveData CurrentSave
        {
            get => currentSave;
            private set => currentSave = value;
        }
        public static SaveLoadManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SaveLoadManager();
                    instance.init();
                }

                return instance;
            }
        }

        private List<SaveData> dataList= new List<SaveData>();

        public void init()
        {
            GameObject root = GameObject.Find(NAME);
            if (root == null)
            {
                root = new GameObject { name = NAME };
                Object.DontDestroyOnLoad(root);
            }
            basePath = Path.Combine(Application.dataPath,"Saves");
        }


        /// <summary>
        /// SaveDate를 기본 경로에 "이름.savedata" 로 저장함.
        /// </summary>
        /// <param name="data">저장할 데이터</param>
        public void Save(SaveData data)
        {
            string path = Path.Combine(this.basePath, data.saveName+extension);
            string json = JsonUtil.Instance.Data2JSON(data);
            JsonUtil.Instance.SaveJSON(json, path);
        }

        /// <summary>
        /// SaveData를 게임으로 로드한다.
        /// </summary>
        /// <param name="data"></param>
        public void Load(SaveData data)
        {
            GameManager gm = GameManager.Instance;
            CurrentSave = data;
        }

        /// <summary>
        /// idx를 통해 세이브 파일 가져오기
        /// </summary>
        /// <param name="idx"></param>
        /// <returns>세이브데이터, idx가 무효하면 null</returns>
        public SaveData GetData(int idx)
        {
            if (idx > dataList.Count || idx < 0)
                return null;
            return dataList[idx];
        }
        /// <summary>
        /// 이름을 통해 세이브 파일 가져오기
        /// </summary>
        /// <param name="idx"></param>
        /// <returns>세이브데이터, 해당이름이 없으면 null</returns>
        public SaveData GetData(string name)
        {
            foreach (var data in dataList)
            {
                if (data.saveName == name)
                {
                    return data;
                }
            }

            return null;
        }
        
        
        /// <summary>
        /// 모든 세이브 데이터를 가져와 매니저에 등록한다.
        /// 로드전에 한번은실행되어야 한다.
        /// </summary>
        /// <returns>가져온 세이브 데이터들</returns>
        public List<SaveData> UpdateAll()
        {
            List<SaveData> res = new List<SaveData>();
            DirectoryInfo di = new DirectoryInfo(path:basePath);
            foreach (var fi in di.GetFiles())
            {
                if (Path.GetExtension(fi.FullName) == extension)
                {
                    Debug.Log("[SVManager] Update " + fi.Name);
                    SaveData data;
                    data = JsonUtil.Instance.LoadJson<SaveData>(fi.FullName);
                    res.Add(data);
                    Debug.Log($"[SVManager] Update Success({res.Count})! {data}");
                }
            }
            // 마지막 저장 순으로 정렬
            res.Sort(((a, b) => a.saveTime.CompareTo(b.saveTime)));
            
            if (res.Any())
                dataList = res;
            return dataList;
        }

    }

    [Serializable]
    public class SaveData
    {
        public string saveName;
        
        public UserDataManager userData;
        
        public long startTime;
        public long saveTime;

        /// <summary>
        /// SaveData 생성후 저장시간을 저장한다.
        /// </summary>
        /// <returns>시간이 저장된 SaveData</returns>
        public static SaveData Make()
        {
            SaveData data = new SaveData();
            data.saveTime = DateTime.Now.ToBinary();
            return data;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"[SaveData] {saveName}\n").
                Append("start time: ").Append(DateTime.FromBinary(startTime)).Append("\n")
                .Append("save time: ").Append(DateTime.FromBinary(saveTime)).Append("\n");
                //userData ToString
            Debug.Log(sb);
            
            return sb.ToString();
        }

        public string ToStringData()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"{saveName}\n").Append("start time: ")
                .Append(DateTime.FromBinary(startTime)).Append("\n")
                .Append("save time: ").Append(DateTime.FromBinary(saveTime)).Append("\n");
                //userData ToString
            Debug.Log(sb);

            return sb.ToString();
        }
    }

    
}