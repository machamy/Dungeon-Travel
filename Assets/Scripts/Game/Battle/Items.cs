using Scripts.Entity.Item;
using UnityEngine;
using Scripts.Data;

public class Items : MonoBehaviour
{
    public BaseItemData[] items;

    public void Awake()
    {
        items[0] = new BaseItemData() { itemName = "Heal Potion", type = ItemType.Item ,infomation = "player can heal"};
        items[1] = new BaseItemData() { itemName = "Cleans"};
        items[2] = new BaseItemData() { itemName = "Invisibility Potion"};
    }
}
