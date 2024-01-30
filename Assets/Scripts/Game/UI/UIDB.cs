using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIDB : MonoBehaviour
{
    public static Dictionary<string, string> buttonDescription = new()
    {
        {"Weapon", "Buy Weapons" },
        {"Armor", "Buy Armors" },
        {"Upgrade", "Upgrade Weapons" },
        {"Sell", "Sell Items" },
        {"Talk", "Talk with Owner" },
        {"Exit", "Go back to Shops" },
    };

    public static List<string> talkList = new()
    {
        "textA",
        "textB",
        "textC",
        "textD",
        "textE",
        "textF",
    };


    public struct ItemInfo
    {
        public string description;
        public int sellCost;

        public ItemInfo(string description, int sellCost)
        {
            this.description = description;
            this.sellCost = sellCost;
        }
    }

    public static Dictionary<string, ItemInfo> allItemList = new()
    {
        {"Potion", new ItemInfo("Recover small amount of HP", 30) },
        {"HighPotion", new ItemInfo("Recover large amount of HP", 50) },
        {"Phoenix's Tail", new ItemInfo("A tail from Phoenix", 100) },
        {"Elixer", new ItemInfo("Recover small amount of MP", 30) },
    };

    public static List<string> inventoryItemList = new()
    {
        "Potion",
        "HighPotion",
        "Phoenix's Tail",
        "Elixer",
        "Potion",
        "Potion",
        "Potion",
        "Potion",
        "Potion",
        "Potion",
        "Potion",
    };


}
