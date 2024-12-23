using Scripts.Data;
using Scripts.Entity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class BattleUnit : MonoBehaviour
{
    public StatData statData;
    public CharacterData data; // 복사된 스탯 데이터
    public bool isDie;
    public float currentHP;   // 현재 HP
    public float currentMP;   // 현재 MP
    protected Character originalCharacter; // 원본 Character 데이터
    public bool isEnemy;

    private SpriteRenderer spriteRenderer;
    /// <summary>
    /// 유닛 초기화
    /// </summary>
    /// <param name="character">Character 데이터</param>
    public virtual void Initialize(CharacterData data)
    {
        // Character 데이터 복사
        // originalCharacter = character;
        // statData = (StatData)character.FinalStat.Clone();

        // 임시 데이터 복사
        this.data = data;

        // 체력과 마나 초기화
        isDie = data.isDie;
        currentHP = this.data.currentHP;
        currentMP = this.data.currentMP;

        //Debug.Log($"Unit Initialized: {character.Name} - HP: {currentHP}, MP: {currentMP}, ATK: {statData.atk}");

        //아웃라인 관련
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public virtual void Initialize(int floor, string name) { }

    /// <summary>
    /// 유닛 공격 처리
    /// </summary>
    /// <param name="skillData">사용 스킬, 기본값 = 기본공격</param>
    public virtual void Attack( BattleUnit target = null,BattleSkill skillData = null)
    {

    }

    public virtual void Guard()
    {

    }

    /// <summary>
    /// 데미지를 받을 때 호출
    /// </summary>
    public void TakeDamage(float damage)
    {
        currentHP -= damage; // 복사된 데이터에서 HP 감소
        Debug.Log($"Unit {originalCharacter.Name} took {damage} damage. Remaining HP: {currentHP}");

        if (currentHP <= 0)
        {
            currentHP = 0;
            Die();
        }
    }

    /// <summary>
    /// 유닛 사망 처리
    /// </summary>
    public virtual void Die()
    {
        Debug.Log($"Unit {originalCharacter.Name} has died.");
        // 필요 시 추가적인 처리
    }

    /// <summary>
    /// 유닛 데이터를 원본 Character에 반환
    /// </summary>
    public void ReturnDataToCharacter()
    {
        if (originalCharacter != null)
        {
            // 최종 데이터를 원본 Character에 반환
            originalCharacter.hp = currentHP;
            originalCharacter.mp = currentMP;
            originalCharacter.rawBaseStat = (StatData)statData.Clone();

            Debug.Log($"Unit data returned to Character: {originalCharacter.Name}");
        }
    }

    /// <summary>
    /// 유닛 정보 출력
    /// </summary>
    public void PrintUnitInfo()
    {
        Debug.Log($"[Unit] {originalCharacter.Name} \n" +
                  $"Class: {originalCharacter._class.name}\n" +
                  $"Level: {originalCharacter.LV}\n" +
                  $"HP: {currentHP}, MP: {currentMP}\n" +
                  $"ATK: {statData.atk}, DEF: {statData.def}");
    }


    #region 아웃라인관련
    private Color color = Color.red;
    private int outlineSize = 2;

    public void UpdateOutline(bool outline)
    {
        MaterialPropertyBlock mpb = new MaterialPropertyBlock();
        spriteRenderer.GetPropertyBlock(mpb);
        mpb.SetFloat("_Outline", outline ? 1f : 0);
        mpb.SetColor("_OutlineColor", color);
        mpb.SetFloat("_OutlineSize", outlineSize);
        spriteRenderer.SetPropertyBlock(mpb);
    }
    #endregion
}
