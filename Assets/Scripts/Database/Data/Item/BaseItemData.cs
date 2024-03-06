using Scripts.Data;
using Unity.VisualScripting;
using UnityEngine;

namespace Scripts.Entity.Item
{
    public class BaseItemData : ScriptableObject
    {

        public string itemName;
        public Sprite itemSprite;

        public ItemType type;

        public bool isUsable;
    }
}