using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Scripts.Entity;

public class StatusUpdater : MonoBehaviour
{
    public GameObject backPanel;
    public Image characterImage;
    public TextMeshProUGUI nameText, hpText, mpText;
    public Slider hpSlider, mpSlider;

    UnitHolder unit;
    float maxHp, maxMp;

    Coroutine updateCoroutine;
    public void Initialize(UnitHolder unit)
    {  
        this.unit = unit;

        maxHp = unit.character.rawBaseStat.hp;
        maxMp = unit.character.rawBaseStat.mp;
        // characterImage

        nameText.text = unit.isFriendly ? $"{unit.name} / {unit.character._class.name}" : $"{unit.name}";

        hpText.text = $"{unit.hp}/{maxHp}";
        mpText.text = $"{unit.mp}/{maxMp}";

        hpSlider.value = unit.hp / maxHp;
        mpSlider.value = unit.mp / maxMp;

        backPanel.SetActive(true);
        updateCoroutine = StartCoroutine(UpdateStatus());
    }


    IEnumerator UpdateStatus()
    {
        while (true)
        {
            hpText.text = $"{unit.hp}/{maxHp}";
            mpText.text = $"{unit.mp}/{maxMp}";

            hpSlider.value = unit.hp / maxHp;
            mpSlider.value = unit.mp / maxMp;

            yield return new WaitForSeconds(0.1f);
        }
        
    }

    public void Destroy()
    {
        if(updateCoroutine != null) StopCoroutine(updateCoroutine);
        unit = null;
        nameText.text = "Name/Class";
        backPanel.SetActive(false);
    }
}
