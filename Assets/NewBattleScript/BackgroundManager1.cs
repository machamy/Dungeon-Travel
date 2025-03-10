using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BackgroundManager1 : MonoBehaviour
{
    public Image backgroundImage;
    public TextMeshProUGUI turnText;

    public void Initailize()
    {
        backgroundImage.enabled = true;
        turnText.enabled = true;
    }

    public void End()
    {
        backgroundImage.enabled = false;
    }
}