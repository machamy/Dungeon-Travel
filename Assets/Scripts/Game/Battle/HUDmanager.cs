using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class HUDmanager : MonoBehaviour
{
    public Slider HPslider, MPslider;
    public TMP_Text HPtext, MPtext;

    public TMP_Text playerNameText;

    private float HP, MP;
    private float maxHP, maxMP;

    /// <summary>
    /// 유닛 소환될 때
    /// </summary>
    /// <param name="unit"></param>
    public void SetupHUD(Unit unit)
    {
        float[] status = unit.GetStatus();
        maxHP = status[0];
        maxMP = status[1];

        HP = status[2];
        MP = status[3];

        if (maxHP == 0) { HPslider.value = 0; }
        else { HPslider.value = HP / maxHP; }

        if (maxMP == 0) { MPslider.value = 0; }
        else { MPslider.value = MP / maxMP; }

        HPtext.text = HP + "/" + maxHP;
        MPtext.text = MP + "/" + maxMP;

        playerNameText.text = unit.unitName;

        this.gameObject.SetActive(true);

        Image livepanel = GetComponent<Image>();
        livepanel.color = new Color(1,1,1,0.4f);
    }

    public void Dead()
    {
        HP = 0;
        UpdateHUD(0, maxHP);

        //HUD 색상 변경
        Image deadpanel = GetComponent<Image>();
        deadpanel.color = new Color(0.5f, 0.5f, 0.5f, 0.4f);
    }

    /// <summary>
    /// HP바,MP바, HP텍스트, MP텍스트 업데이트
    /// </summary>
    /// <param name="currentHP"></param>
    /// <param name="currentMP"></param>
    public void UpdateHUD(float currentHP, float currentMP)
    {
        if (maxHP == 0) { HPslider.value = 0; }
        else { HPslider.value = currentHP / maxHP; }

        if (maxMP == 0) { MPslider.value = 0; }
        else { MPslider.value = currentMP / maxMP; }

        HPtext.text = currentHP + "/" + maxHP;
        MPtext.text = currentMP + "/" + maxMP;
    }
}
