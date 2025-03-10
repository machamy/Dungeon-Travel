using Scripts.Data;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using static UnityEngine.UI.CanvasScaler;

public class HUDmanager : MonoBehaviour
{
    public Slider HPslider, MPslider;
    public TMP_Text HPtext, MPtext;

    public TMP_Text NameText;

    private float HP, MP;
    private float maxHP, maxMP;

    private BattleUnit unit;
    private StatData stat;

    public void Initialize(BattleUnit unit)
    {
        this.unit = unit;
        unit.hudManager = this;
        stat = unit.statData;
        maxHP = stat.hp;
        maxMP = stat.mp;

        NameText.text = unit.Name;

        HP = unit.currentHP;
        MP = unit.currentMP;

        HPslider.value = HP / maxHP;

        if (maxMP == 0) MPslider.value = 0;
        else MPslider.value = MP / maxMP;

        HPtext.text = HP + "/" + maxHP;
        MPtext.text = MP + "/" + maxMP;

        gameObject.SetActive(true);

        Image livepanel = GetComponent<Image>();
        livepanel.color = new Color(1, 1, 1, 0.4f);
    }


    public void Die()
    {
        HP = 0;
        HPslider.value = 0;
        MPslider.value = 0;

        //HUD 색상 변경
        Image deadpanel = GetComponent<Image>();
        deadpanel.color = new Color(0.5f, 0.5f, 0.5f, 0.4f);
    }

    /// <summary>
    /// HP바,MP바, HP텍스트, MP텍스트 업데이트
    /// </summary>
    /// <param name="currentHP"></param>
    /// <param name="currentMP"></param>
    public void UpdateHUD()
    {
        HPslider.value = unit.currentHP / maxHP;

        if (maxMP == 0) MPslider.value = 0;
        else MPslider.value = unit.currentMP / maxMP;

        HPtext.text = unit.currentHP + "/" + maxHP;
        MPtext.text = unit.currentMP + "/" + maxMP;
    }

    public void Destroy()
    {
        gameObject.SetActive(false);
    }
}
