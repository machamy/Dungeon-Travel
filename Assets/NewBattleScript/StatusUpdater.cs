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

    Coroutine updateCoroutine;
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
        updateCoroutine = StartCoroutine(UpdateStatus());
    }


    IEnumerator UpdateStatus()
    {
        while (!character.isDead)
        {
            hpText.text = $"{character.hp}/{maxHp}";
            mpText.text = $"{character.mp}/{maxMp}";

            hpSlider.value = character.hp / maxHp;
            mpSlider.value = character.mp / maxMp;
            yield return new WaitForSeconds(0.1f);
        }
        Image panel = backPanel.GetComponent<Image>();
        panel.color = new Color(0, 0, 0, 0.19f);
    }

    public void Destroy()
    {
        if(updateCoroutine != null) StopCoroutine(updateCoroutine);
        character = null;
        nameText.text = "Name/Class";
        backPanel.SetActive(false);
    }
}
