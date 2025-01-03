using Scripts.Entity;
using Scripts.Manager;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterStatUI_Small : MonoBehaviour
{
    public Image classImage;
    public RectTransform hpRect;
    public RectTransform mpRect;
    private const float width = 130f;

    public void LoadUI(int idx = 0)
    {
        Character character = UserDataManager.Instance.party.GetCharacter(idx);
        classImage.sprite = Resources.Load<Sprite>("UI/Class/" + character.Name);

        hpRect.sizeDelta = new Vector2(width * (character.hp / character.FinalStat.hp), hpRect.sizeDelta.y);
        mpRect.sizeDelta = new Vector2(width * (character.mp / character.FinalStat.mp), mpRect.sizeDelta.y);
    }

}
