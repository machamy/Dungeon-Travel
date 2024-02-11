using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIDB : MonoBehaviour
{

    public enum State
    {
        Forge_Menu,
            Forge_Weapon,
                Forge_AskBuy,
            Forge_Talk,
            Forge_AskExit,

        Main_Place,
            Main_Menu,
                Main_Item,
                    Main_ItemUseDelete,
                        Main_ItemParty,
                Main_Skill,
                Main_Equip,
                Main_Status,
                Main_Party,
                Main_Config,
                    Main_ConfigType,
                        Main_ConfigVideo,

    }


    public static Dictionary<string, string> buttonDescription = new()
    {
        {"Weapon", "Buy Weapons" },
        {"Armor", "Buy Armors" },
        {"Upgrade", "Upgrade Weapons" },
        {"Sell", "Sell Items" },
        {"Talk", "Talk with Owner" },
        {"Exit", "Go Back to Shops" },

        {"Item", "Use or Delete Items" },
        {"Skill", "Use Skills" },
        {"Equip", "Change Equipments" },
        {"Status", "See Status of Characters" },
        {"Party", "Change Party Organization" },
        {"Config", "Change Configuration" },
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
        "HighPotion",
        "Phoenix's Tail",
        "Elixer",
        "Potion",
        "HighPotion",
        "Phoenix's Tail",
        "Elixer",
        "Potion",
        "HighPotion",
        "Phoenix's Tail",
        "Elixer",
    };


}
