using Scripts.Data;
using Scripts.Entity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlePlayerUnit : BattleUnit
{
    
    public override void Initialize(BattleStat stat)
    {
        base.Initialize(stat);
        isEnemy = false;
    }

    public override void Die()
    {
        base.Die();
    }

    public override void Guard()
    {

    }


}
