using Scripts.Data;
using Scripts.Entity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts;
using System;

public class BattleEnemyUnit : BattleUnit
{
    public List<SkillData> skillDatas = new List<SkillData>();
    Enemy_Skill skill;
    private List<Action<SkillData>> skillLists;
    public int index = -1;
    public override void Initialize(int floor, string name)
    {
        statData = DB.GetEnemyData(floor, name);
        skillDatas = DB.GetEnemySkillData(floor, name);
        skill = new Enemy_Skill(this);
        skillLists = skill.GetSkillList(floor, name);

        currentHP = statData.hp;
        currentMP = statData.mp;

        Debug.Log($"Unit Initialized: {statData.name} - HP: {currentHP}, MP: {currentMP}, ATK: {statData.atk}");
        spriteRenderer = GetComponent<SpriteRenderer>();
        buffManager = GetComponent<BuffManager>();
    }

    public override IEnumerator Attack(BattleUnit[] target = null, SkillData skillData = null)
    {
        yield return StartCoroutine(AttackCoroutine());
    }

    private IEnumerator AttackCoroutine()
    {
        Debug.Log("StartCoroutine");
        RectTransform enemyRect = GetComponent<RectTransform>();
        Vector3 originalPosition = enemyRect.anchoredPosition;

        // 앞으로 이동
        Vector3 targetPosition = originalPosition + new Vector3(moveDistance-80, 0);
        yield return MoveToPosition(targetPosition, shakeDuration - 2.0f);

        AttackExcute();

        // 뒤로 이동
        targetPosition = originalPosition - new Vector3(moveDistance, 0);
        yield return MoveToPosition(targetPosition, shakeDuration);

        // 원래 위치로 복귀
        yield return MoveToPosition(originalPosition, shakeDuration);
    }

    private void AttackExcute()
    {
        Debug.Log($"{statData.name} attack");
        if (buffManager.debuffDic.ContainsKey(DebuffType.Stun)) // 기절이라면 공격 함수 실행 x
            return;
        int[] weightArr = new int[5];
        int i = 0;
        foreach (SkillData skilldata in skillDatas)
        {
            weightArr[i++] = skilldata.skillWeight;
        }
        if (index == -1) // 미리 지정되있는 스킬이 없을때
        {
            index = Utility.WeightedRandom(weightArr);
            if (buffManager.debuffDic.ContainsKey(DebuffType.Silence)) // 침묵이라면  skillLists[0]에 저장되어 있는 기본공격만 하도록
                index = 0;
        }
        //Debug.Log($"index = {index}");
        skillLists[index].Invoke(skillDatas[index]);
        index = -1;
    }

    private IEnumerator MoveToPosition(Vector2 targetPosition, float shakeDuration)
    {
        RectTransform enemyRect = GetComponent<RectTransform>();
        float elapsedTime = 0f;
        Vector2 startPosition = enemyRect.anchoredPosition;

        while (elapsedTime < shakeDuration / 2)
        {
            enemyRect.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, (elapsedTime / (shakeDuration / 2)));
            elapsedTime += Time.deltaTime * smoothSpeed;
            yield return null;
        }

        enemyRect.anchoredPosition = targetPosition; // 정확한 위치 설정
    }
}
