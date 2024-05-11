using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class HUDmanager : MonoBehaviour
{
    public Slider HPslider, MPslider;
    public TMP_Text HPtext, MPtext;

    public TMP_Text playerNameText;

    private Unit unit;

    private float HP, MP;
    private float _maxHP, _maxMP;

    /// <summary>
    /// 유닛 소환될 때
    /// </summary>
    /// <param name="player"></param>
    public void SetupHUD(Unit player)
    {
        _maxHP = player.maxHP;
        _maxMP = player.maxMP;

        HP = player.maxHP;
        MP = player.maxMP;

        HPslider.value = HP / _maxHP;
        MPslider.value = MP / _maxMP;

        HPtext.text = HP + "/" + _maxHP;
        MPtext.text = MP + "/" + _maxMP;

        playerNameText.text = player.unitName;

        this.gameObject.SetActive(true);
    }

    public void Asset()
    {
        
    }

    /// <summary>
    /// HP바,MP바, HP텍스트, MP텍스트 업데이트
    /// </summary>
    /// <param name="currentHP"></param>
    /// <param name="currentMP"></param>
    public void UpdateHUD(float currentHP, float currentMP)
    {
        HPslider.value = currentHP / _maxHP;
        MPslider.value = currentMP / _maxMP;

        HPtext.text = currentHP + "/" + _maxHP;
        MPtext.text = currentMP + "/" + _maxMP;
    }

    public void DeadColor()
    {
        Image deadpanel = GetComponent<Image>();
        deadpanel.color = new Color(0.5f,0.5f,0.5f,0.4f);
    }
}
