using Scripts.Game.Dungeon.Unit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace MonsterOwnedStates
{
    public class Idle : State<MonsterUnit>
    {
        public override void Enter(MonsterUnit entity)
        {
            Debug.Log("몬스터 IDLE 상태 진입.");
        }

        public override void Execute(MonsterUnit entity)
        {
            if (Vector3.Distance(entity.transform.position, entity.Target.position) <= entity.DetectRadius)
            {
                entity.CanChaseOrFlee = true;
                if (entity.CanChaseOrFlee && entity.AttackType == MonsterAttackType.firstStrike)
                {
                    entity.ChangeState(MonsterStates.Chase);
                }
                else if(entity.CanChaseOrFlee && entity.AttackType==MonsterAttackType.fleeing)
                {
                    entity.ChangeState(MonsterStates.Flee);
                }
                else if(entity.AttackType==MonsterAttackType.mustCombat)
                {
                    entity.ChangeState(MonsterStates.Combat);
                }
            }

            if (entity.MoveType == MonsterMoveType.doPatrol)
            {
                entity.Patrol();
            }
        }

        public override void Exit(MonsterUnit entity)
        {
            Debug.Log("몬스터 IDLE 상태 탈출.");
        }

    }

    public class Chase : State<MonsterUnit>
    {
        public override void Enter(MonsterUnit entity)
        {
            Debug.Log("몬스터 Chase 상태 진입.");
            entity.IsChasing = true;
        }

        public override void Execute(MonsterUnit entity)
        {           
            if (!entity.CanChaseOrFlee) return;
            else
            {
                Debug.Log(Vector3.Distance(entity.transform.localPosition, entity.InitialPosition));
                if (Vector3.Distance(entity.transform.localPosition, entity.InitialPosition) >= entity.MoveRadius - 0.2f)
                {
                    entity.CanChaseOrFlee = false;
                    entity.ChangeState(MonsterStates.Return);
                }
                else if (Vector3.Distance(entity.Target.localPosition, entity.InitialPosition) < entity.MoveRadius)
                {
                    entity.IsChasing = true;
                    entity.Nav.SetDestination(entity.Target.localPosition);
                    entity.Nav.speed = entity.ChaseSpeed;
                    Debug.Log("추격 중!");

                    if (Vector3.Distance(entity.Target.localPosition, entity.transform.localPosition) <= entity.AttackRange)
                    {
                        Debug.Log("전투 돌입!");
                        entity.ChangeState(MonsterStates.Combat);
                    }
                }
            }
        }

        public override void Exit(MonsterUnit entity)
        {
            Debug.Log("몬스터 Chase 상태 탈출.");
        }

    }

    public class Flee : State<MonsterUnit>
    {
        public override void Enter(MonsterUnit entity)
        {
            throw new System.NotImplementedException();
        }

        public override void Execute(MonsterUnit entity)
        {
            throw new System.NotImplementedException();
        }

        public override void Exit(MonsterUnit entity)
        {
            throw new System.NotImplementedException();
        }

    }

    public class Return : State<MonsterUnit>
    {
        public override void Enter(MonsterUnit entity)
        {
            Debug.Log("몬스터 Return 상태 진입.");
        }

        public override void Execute(MonsterUnit entity)
        {
            if (entity.IsChasing && !entity.CanChaseOrFlee)
            {
                entity.Nav.SetDestination(entity.InitialPosition);
                entity.Nav.speed = entity.DefaultSpeed;
                Debug.Log("놓쳤다. 돌아가야지~");
            }

            
        }

        public override void Exit(MonsterUnit entity)
        {
            Debug.Log("몬스터 Return 상태 탈출.");
        }

    }

    public class Combat : State<MonsterUnit>
    {
        public override void Enter(MonsterUnit entity)
        {
            Debug.Log("몬스터 Combat 상태 진입.");
            //전투 돌입 코드
            //난입(랜덤 몬스터)
            //인카운터 방식(공격 / 피격)
        }

        public override void Execute(MonsterUnit entity)
        {

        }

        public override void Exit(MonsterUnit entity)
        {
            Debug.Log("몬스터 Combat 상태 탈출.");
        }

    }

    public class Dead : State<MonsterUnit>
    {
        public override void Enter(MonsterUnit entity)
        {
            throw new System.NotImplementedException();
        }

        public override void Execute(MonsterUnit entity)
        {
            throw new System.NotImplementedException();
        }

        public override void Exit(MonsterUnit entity)
        {
            throw new System.NotImplementedException();
        }

    }
}
