using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rabbit : Enemy_Base
{
    public override void EnemyAttack(AttackType type)
    {
        switch (type)
        {
            case AttackType.Damage:
                Damaged();
                break;
            case AttackType.Penetrate:
                Penetrate();
                break;
            case AttackType.Slash:
                Slash();
                break;
        }
    }
}
