using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rabbit : Enemy_Base
{
    EnemyStatData enemyStatData = DB.GetEnemyData("토끼");
    public override void EnemyAttack(AttackProperty property, AttackType type)
    {
        switch (type)
        {
            case AttackType.Damage:
                Slash(enemyStatData.atk);
                break;
            case AttackType.Penetrate:
                Penetrate();
                break;
            case AttackType.Slash:
                Smash();
                break;
        }
    }
}
