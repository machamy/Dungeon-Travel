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

    Character character;
    float maxHp, maxMp;
    public void Initialize(Character character)
    {
        this.character = character;

        maxHp = character.rawBaseStat.hp;
        maxMp = character.rawBaseStat.mp;
        // characterImage

        nameText.text = character.isFriendly ? $"{character.Name} / {character._class.name}" : $"{character.Name}";

        hpText.text = $"{character.hp}/{maxHp}";
        mpText.text = $"{character.mp}/{maxMp}";

        hpSlider.value = character.hp / maxHp;
        mpSlider.value = character.mp / maxMp;

        backPanel.SetActive(true);
        StartCoroutine(UpdateStatus());
    }


    IEnumerator UpdateStatus()
    {
        hpText.text = $"{character.hp}/{maxHp}";
        mpText.text = $"{character.mp}/{maxMp}";

        hpSlider.value = character.hp / maxHp;
        mpSlider.value = character.mp / maxMp;
        yield return new WaitForSeconds(0.1f);
    }
}
