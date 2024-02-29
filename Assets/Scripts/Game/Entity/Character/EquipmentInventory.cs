using Scripts.Entity.Item;
using Scripts.Game;

namespace Game.Entity.Character
{
    public enum EquipmentSlot
    {
        Default = 0,
        LeftHand = 1,
        RightHand = 2,
            
            
        Head = 5,
        Chest = 6,
        Leggings = 7,
        Boots = 8,
            
        Neck = 11,
        LeftWrists = 12,
        RightWirsts = 13,
            
        Ring01 = 16,
        Ring02 = 17,
        Ring03 = 18,


        Size
    }
    public class EquipmentInventory: Inventory
    {
        public static EquipmentInventory CreateInstance()
        {
            var instance = new EquipmentInventory();
            instance.SetMaxSlot((int)EquipmentSlot.Size);
            
            return instance;
        }


        public bool SetItem(EquipmentSlot slot, BaseItemData item) => base.SetItem((int)slot, item);
        protected new bool SetItem(int slot, BaseItemData item) => base.SetItem(slot, item); // 사용 막음
        public bool GetItem(EquipmentSlot slot) => base.GetItem((int)slot);
    }
}