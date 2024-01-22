using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDmanager : MonoBehaviour
{
    public Slider HPslider, MPslider;
    public TMP_Text HPtext,MPtext;

    private Unit unit;

    private float maxHP, maxMP;
    private float currentHP, currentMP;

    /// <summary>
    /// SetupBattle ���� ����
    /// </summary>
    /// <param name="getunit"></param>
    public void Setup(Unit getunit)  //��Ʋ�ý��� SetupBattle ���� ����
    {
        unit = getunit;

        maxHP = unit.stat.hp;    
        maxMP = unit.stat.mp;
        currentHP = maxHP;     
        currentMP = maxMP;

        UpdateHUD();
    }

    public void UpdateHUD()
    {
        HPslider.value = currentHP / maxHP;
        MPslider.value = currentHP / maxHP;

        HPtext.text = currentHP + "/" + maxHP;
        MPtext.text = currentMP + "/" + maxMP;
    }

    public void TakeDamage(float damage)
    {
        currentHP -= damage;    
    }

    public void TakeMana(float mana)
    {
        currentMP -= mana;
    }
}
