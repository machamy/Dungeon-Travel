using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDmanager : MonoBehaviour
{
    public Slider HPslider, MPslider;
    public TMP_Text HPtext, MPtext;

    public TMP_Text playerNameText;

    private Unit unit;

    private float HP, MP;
    private float _maxHP, _maxMP;

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

        playerNameText.text = player.Name;

        this.gameObject.SetActive(true);
    }

    public void Asset()
    {
        
    }

    /// <summary>
    /// HP바와 MP바, HP텍스트, MP텍스트 업데이트
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
}
