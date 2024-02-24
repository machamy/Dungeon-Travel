using Scripts.Entity.Item;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Scripts.Game
{
    public class Inventory
    {
        private List<BaseItemData> itemList;
        
        
        private int maxSlot;
        public int MaxSlot => maxSlot;

        public void SetMaxSlot(int n)
        {
            this.maxSlot = n;
        }

        public bool AddItem(BaseItemData item)
        {
            // 최대 개수 초과
            if (itemList.Count >= maxSlot)
            {
                return false;
            }

            itemList.Add(item);
            return true;
        }

        public bool SetItem(int slot, BaseItemData item)
        {
            if (itemList.Count > slot)
            {
                return false;
            }

            itemList[slot] = item;
            return true;
        }

        public bool RemoveItem(int slot)
        {
            return SetItem(slot, null);
        }

        public BaseItemData GetItem(int slot)
        {
            if (itemList.Count > slot)
            {
                return null;
            }

            return itemList[slot];
        }

        
        /// <summary>
        /// 아이템 목록을 복사해 가져온다
        /// </summary>
        /// <returns>아이템 목록</returns>
        public BaseItemData[] GetItems()
        {
            return itemList.ToArray();
        }
        
    }
}