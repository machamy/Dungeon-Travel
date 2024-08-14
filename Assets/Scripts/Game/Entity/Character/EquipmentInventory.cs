using Scripts.Entity.Item;
using Scripts.Game;
using System;
using System.Collections.Generic;

namespace Game.Entity.Character
{
    public enum EquipmentType
    {
        Weapon = 0,
        Helmet = 1,
        Armor = 2,
        Shoes = 3,
        Gloves = 4,
        Accessory1 = 5,
        Accessory2 = 6,

        Size
    }
    public class EquipmentInventory
    {
        private BaseItemData[] EquipmentSlot;

        public static EquipmentInventory CreateInstance()
        {
            var instance = new EquipmentInventory();
            
            return instance;
        }

        private EquipmentInventory()
        {
            EquipmentSlot = new BaseItemData[(int)EquipmentType.Size];
        }


        public BaseItemData SetItem(EquipmentType slot, BaseItemData item)
        {
            BaseItemData BeforeItem = EquipmentSlot[(int)slot];
            EquipmentSlot[(int)slot] = item;

            return BeforeItem;
        }

        public BaseItemData RemoveItem(EquipmentType slot)
        {
            return SetItem(slot, null);
        }

        public BaseItemData GetItem(EquipmentType slot)
        {
            return EquipmentSlot[(int)slot];
        }

        /// <summary>
        /// 아이템 목록을 복사해 가져온다
        /// </summary>
        /// <returns>아이템 목록</returns>
        public BaseItemData[] GetItems()
        {
            return EquipmentSlot;
        }

    }
}