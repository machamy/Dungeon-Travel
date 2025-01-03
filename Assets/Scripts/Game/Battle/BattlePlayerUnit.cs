using Scripts.Data;
using Scripts.Entity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlePlayerUnit : BattleUnit
{
    
    public override void Initialize(CharacterData data)
    {
        base.Initialize(data);
        isEnemy = false;
    }

    public override void Die()
    {
        base.Die();
    }

    public override IEnumerator Attack(List<BattleUnit> target = null, SkillData skillData = null)
    {
        //base.StartCoroutine(Attack(target, skillData));
        if (target == null) 
            yield return null;

        for(int i = 0; i < target.Count; i++)
        {
            if (skillData != null) target[i].TakeDamage(this, skillData);
            else target[i].TakeDamage(this);
            yield return null;
        }

    }
    public override void Guard()
    {

    }


}
