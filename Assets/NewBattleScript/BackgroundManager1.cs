using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BackgroundManager1 : MonoBehaviour
{
    public GameObject endPanel;

    public Image backgroundImage;
    public TextMeshProUGUI turnText;
    int turnCount;

    public void Initailize()
    {
        turnCount = 0;
        backgroundImage.enabled = true;
        turnText.enabled = true;
    }

    public void addTurn()
    {
        turnCount++;
        turnText.text = $"Turn: {turnCount}";
    }

    public IEnumerator End()
    {
        backgroundImage.enabled = false;
        turnText.enabled = false;
        endPanel.SetActive(true);
        yield return null;
    }
}