using UnityEngine;
using UnityEngine.UI;

public class HPSlider : MonoBehaviour
{
    [SerializeField]
    private Slider HPbar;
    public int maxHP, currentHP;

    private void Awake()
    {
        HPbar = GetComponentInChildren<Slider>();
    }

    public void UpdateHPbar(int currentHP, int maxHP)
    {
        HPbar.value = currentHP / maxHP;
    }

    public void TakeDamage(int damageAmount)
    {
        currentHP -= damageAmount;
        UpdateHPbar(currentHP, maxHP);
    }
}
