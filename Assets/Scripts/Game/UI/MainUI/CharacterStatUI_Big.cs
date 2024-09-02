using Scripts.Entity;
using Scripts.Manager;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterStatUI_Big : MonoBehaviour
{
    public Image classImage;
    public TMP_Text classText;
    public TMP_Text hpText;
    public TMP_Text mpText;
    public RectTransform hpRect;
    public RectTransform mpRect;
    private const float width = 300f;

    public void LoadUI(int idx = 0)
    {
        Character character = GameManager.Instance.userData.party.GetCharacter(idx);
        classImage.sprite = Resources.Load<Sprite>("UI/Class/" + character.Name);
        classText.text = character.Name;

        hpText.text = character.hp.ToString() + " / " + character.FinalStat.hp.ToString();
        mpText.text = character.mp.ToString() + " / " + character.FinalStat.mp.ToString();
        hpRect.sizeDelta = new Vector2(width * (character.hp / character.FinalStat.hp), hpRect.sizeDelta.y);
        mpRect.sizeDelta = new Vector2(width * (character.mp / character.FinalStat.mp), mpRect.sizeDelta.y);
    }

}
