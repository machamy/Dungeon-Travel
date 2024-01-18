using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Script.Global;
using Scripts.Entity;
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
        /// idx를 통해 세이브 파일 가져오기
        /// </summary>
        /// <param name="idx"></param>
        /// <returns>세이브데이터, idx가 무효하면 null</returns>
        public SaveData Load(int idx)
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
        public SaveData Load(string name)
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
        public List<SaveData> LoadAll()
        {
            List<SaveData> res = new List<SaveData>();
            DirectoryInfo di = new DirectoryInfo(path:basePath);
            foreach (var fi in di.GetFiles())
            {
                if (Path.GetExtension(fi.FullName) == extension)
                {
                    Debug.Log("[SVManager] Load " + fi.Name);
                    SaveData data;
                    data = JsonUtil.Instance.LoadJson<SaveData>(fi.FullName);
                    res.Add(data);
                    Debug.Log($"[SVManager] Load Success({res.Count})! {data}");
                }
            }
            
            if (res.Any())
                dataList = res;
            return dataList;
        }

    }

    [Serializable]
    public class SaveData
    {
        public string saveName;
        //TODO: 배열로 안바꿔도 저장이 되어야하는데 안됨. 일단 임시로 배열로 변환토록 함.
        public Character[] partyArr;
        public List<Character> Party
        {
            get => partyArr.ToList();
            set => partyArr = value.ToArray();
        }
        
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
                .Append("save time: ").Append(DateTime.FromBinary(saveTime)).Append("\n")
                .Append($"party({Party.Count}) : \n");

            foreach (var chr in Party)
            {
                sb.Append(chr.ToString()).Append("\n");
            }
            
            return sb.ToString();
        }
    }

    // public class CharacterData
    // {
    //     
    // }
    
}