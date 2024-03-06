using System;
using Script.Data;
using Script.Global;
using UnityEngine;

namespace Scripts.Data
{
    
    /// <summary>
    /// ItemData로 합쳐도 괜찮을거같음 - machamy
    /// </summary>
    [Serializable]
    public class EquipmentData:ScriptableObject, IDBdata
    {
        public string name;
    }
}