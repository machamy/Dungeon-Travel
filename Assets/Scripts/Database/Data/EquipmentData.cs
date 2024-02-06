using System;
using Script.Data;
using Script.Global;
using UnityEngine;

namespace Scripts.Data
{
    [Serializable]
    public class EquipmentData:ScriptableObject, IDBdata
    {
        public string name;
    }
}