using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDmanager : MonoBehaviour
{
    public GameObject HUD;

    public Slider HPslider, MPslider;
    public TMP_Text HPtext,MPtext;

    public TMP_Text playerNameText;
    public TMP_Text playerClassText;

    private Unit unit;

    private float HP,MP;
    private float _maxHP, _maxMP;


    public void SetupHUD(float maxHP, float maxMP, string PlayerName, string ClassName)
    {
        _maxHP = maxHP;
        _maxMP = maxMP;

        HP = maxHP;     
        MP = maxMP;

        HPslider.value = HP / _maxHP;
        MPslider.value = MP / _maxMP;

        HPtext.text = HP + "/" + _maxHP;
        MPtext.text = MP + "/" + _maxMP;

        playerNameText.text = PlayerName;
        playerClassText.text = ClassName;

        HUD.SetActive(true);
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
