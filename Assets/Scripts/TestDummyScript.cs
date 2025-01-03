using System;
using System.Collections;
using System.Collections.Generic;
using Scripts.Entity;
using Scripts.Manager;
using UnityEngine;

namespace Scripts
{
    public class TestDummyScript : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            var gm = Manager.GameManager.Instance;
            var sm = Manager.SoundManager.Instance;

            var ex = new ExcelReader();
            ex.Read();
            
            // StartCoroutine(SaveLoadTestCo());
        }

        // Update is called once per frame
        void Update()
        {

        }


        IEnumerator SaveLoadTestCo()
        {
            yield return new WaitForSeconds(1.5f);
            Debug.Log("[Dummy::SaveLoadTestCo] Start");
            SaveTest();
            yield return new WaitForSeconds(1.5f);
            LoadTest();
            Debug.Log("[Dummy::SaveLoadTestCo] Finished");
        }

        private string testSaveName = "TestSave01";

        void SaveTest()
        {
            SaveData data = SaveData.Make();
            
            /*List<Character> party = new List<Character>();
            Character tc = GetComponent<Character>();
            party.Add(tc);
            data.Party = party;*/
            
            data.startTime = DateTime.Today.Subtract(new TimeSpan(1, 0, 0, 0)).ToBinary();
            data.saveName = testSaveName;
            SaveLoadManager.Instance.Save(data);
            Debug.Log($"Saved! : {data}");
        }

        void LoadTest()
        {
            Debug.Log("Saves : " + SaveLoadManager.Instance.UpdateAll().Count);
            SaveData data = SaveLoadManager.Instance.GetData(testSaveName);
            Debug.Log($"Loaded! : {data}");
        }
    }
}
