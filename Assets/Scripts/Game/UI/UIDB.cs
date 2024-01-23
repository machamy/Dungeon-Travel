using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIDB : MonoBehaviour
{
    public static Dictionary<string, string> buttonDescription = new()
    {
        {"buyWeapon", "you can buyWeapon" },
        {"buyArmor", "you can buyArmor" },
        {"upgradeWeapon", "you can upgradeWeapon" },
        {"sellItem", "you can sellItem" },
        {"talk", "you can talk" },
        {"exit", "you can exit" },
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

    
}
