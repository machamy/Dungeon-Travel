using Scripts.Data;
using System;
using UnityEngine;

namespace Scripts.Entity.Item
{
    [CreateAssetMenu]
    [Serializable]
    public class BaseItemData : ScriptableObject
    {

        public string itemName;
        public Sprite itemSprite;

        public ItemType type;

        public bool isUsable;
        public string infomation;
    }
}