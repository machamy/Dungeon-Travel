using Scripts.Entity.Item;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Scripts.UserData
{
    /// <summary>
    /// 플레이어 인벤토리 관리 클래스;
    /// 캐릭터별 장비 인벤토리는 캐릭터 객체 안에 있다.
    /// </summary>
    public class Inventory
    {
        private List<BaseItemData> itemList;

        public int MaxSlot { get; private set; }
        public int Count => itemList.Count;
        public bool IsFull => MaxSlot >= Count;

        protected Inventory() { }

        public static Inventory CreateInstance(int maxSlot)
        {
            Inventory inv = new Inventory();
            inv.itemList = new List<BaseItemData>();
            inv.SetMaxSlot(maxSlot);
            
            return inv;
        }

        public void SetMaxSlot(int n)
        {
            MaxSlot = n;
        }

        public bool AddItem(BaseItemData item)
        {
            // 최대 개수 초과
            if (itemList.Count >= MaxSlot)
            {
                return false;
            }

            itemList.Add(item);
            return true;
        }

        public void RemoveItem(BaseItemData item)
        {
            itemList.Remove(item);
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