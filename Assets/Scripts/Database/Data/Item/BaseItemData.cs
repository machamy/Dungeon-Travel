using Scripts.Data;
using UnityEngine;

namespace Scripts.Entity.Item
{
    public class BaseItemData : ScriptableObject
    {

        public string itemName;
        public Sprite itemSprite;

        public ItemType type;
        public string infomation;
    }
}