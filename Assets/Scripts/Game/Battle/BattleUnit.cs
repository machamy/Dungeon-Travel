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
    public int position;
    public bool isEnemy;
    public string Name;
    public SkillData[] skillData = new SkillData[4];
    public HUDmanager hudManager;

    protected float moveDistance = 100f; // 이동할 거리
    protected float shakeDuration = 3f; // 흔드는 시간
    protected float smoothSpeed = 5f; // 부드럽게 이동하는 속도

    private RectTransform rectTransform;

    protected SpriteRenderer spriteRenderer;
    protected BuffManager buffManager;
    /// <summary>
    /// 유닛 초기화
    /// </summary>
    /// <param name="character">Character 데이터</param>
    public virtual void Initialize(CharacterData data)
    {
        statData = new StatData();
        statData.hp = data.maxHP;
        statData.mp = data.maxMP;
        statData.atk = data.atk;
        statData.def = data.def;
        statData.mdef = data.mdef;
        statData.accuracy = data.acc;
        statData.critical = data.cri;
        statData.strWeight = data.strWeight;
        statData.magWeight = data.magWeight;
        statData.str = data.str;
        statData.vit = data.vit;
        statData.mag = data.mag;
        statData.agi = data.agi;
        statData.luk = data.luk;
        statData.statUp = data.statUp;

        Name = data.unitName;
        position = data.position;

        Debug.Log(statData);



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
    public virtual IEnumerator Attack( BattleUnit[] target = null, SkillData skillData = null)
    {
        yield return null;
    }

    public virtual void Guard()
    {

    }

    /// <summary>
    /// 데미지를 받을 때 호출
    /// </summary>
    public void TakeDamage(BattleUnit attackUnit, SkillData skillData = null)
    {
        float damage = attackUnit.statData.atk - statData.def;
        currentHP -= damage; // 복사된 데이터에서 HP 감소

        hudManager.UpdateHUD();
        if (currentHP <= 0)
        {
            currentHP = 0;
            Die();
            hudManager.Die();
        }
    }

    /// <summary>
    /// 유닛 사망 처리
    /// </summary>
    public virtual void Die()
    {
        
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
