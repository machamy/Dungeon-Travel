using Scripts.Game.Dungeon.Unit;
using Scripts.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using Unity.VisualScripting;

namespace GeneralMonsterStates
{
    public class Idle : State<GeneralUnit>
    {
        public override void Enter(GeneralUnit entity)
        {
            Debug.Log("몬스터 IDLE 상태 진입.");
        }

        public override void Execute(GeneralUnit entity)
        {
            if (Vector3.Distance(entity.transform.position, entity.target.position) <= entity.AIData.detectRadius)
            {
                entity.checker.canChaseOrFlee = true;
                if (entity.checker.canChaseOrFlee && entity.MonsterProperty.HasFlag(EnemyProperty.Hostile))
                {
                    entity.ChangeState(GMStates.Chase);
                }
                // 적 속성 두개로 감소됨 - machamy
                // else if (entity.checker.canChaseOrFlee && !entity.MonsterProperty.HasFlag(EnemyProperty.Chase))
                // {
                //     entity.ChangeState(GMStates.Flee);
                // }
            }

            if (entity.MonsterProperty.HasFlag(EnemyProperty.Movement))
            {
                entity.Patrol();
            }           
        }

        public override void Exit(GeneralUnit entity)
        {
            Debug.Log("몬스터 IDLE 상태 탈출.");
        }

    }

    public class Chase : State<GeneralUnit>
    {
        public override void Enter(GeneralUnit entity)
        {
            Debug.Log("몬스터 Chase 상태 진입.");
            entity.checker.isChasingOrFleeing = true;
        }

        public override void Execute(GeneralUnit entity)
        {           
            if (!entity.checker.canChaseOrFlee) return;

            //Debug.Log(Vector3.Distance(entity.transform.localPosition, entity.InitialPosition));
            if (Vector3.Distance(entity.transform.localPosition, entity.InitialPosition) >= entity.AIData.moveRadius - 0.2f)
            {
                entity.checker.canChaseOrFlee= false;
                entity.ChangeState(GMStates.Return);
            }
            else if (Vector3.Distance(entity.target.localPosition, entity.InitialPosition) < entity.AIData.moveRadius)
            {
                entity.checker.isChasingOrFleeing = true;
                entity.Nav.SetDestination(entity.target.localPosition);
                entity.Nav.speed = entity.AIData.chaseOrFleeSpeed;
                Debug.Log("추격 중!");

                Debug.Log(Vector3.Distance(entity.target.localPosition, entity.transform.localPosition));
                if (Vector3.Distance(entity.target.localPosition, entity.transform.localPosition) <= entity.AIData.attackRange)
                {
                    Debug.Log("전투 돌입!");
                    entity.ChangeState(GMStates.Combat);
                }
            }
            
        }

        public override void Exit(GeneralUnit entity)
        {
            Debug.Log("몬스터 Chase 상태 탈출.");
        }

    }

    public class Flee : State<GeneralUnit>
    {
        private float fleeTimer;
        public override void Enter(GeneralUnit entity)
        {
            Debug.Log("몬스터 Flee 상태 진입.");
            entity.checker.isChasingOrFleeing = true;
            fleeTimer = 0f;
        }

        public override void Execute(GeneralUnit entity)
        {
            if (!entity.checker.canChaseOrFlee) return;

            fleeTimer += Time.deltaTime;

            Vector3 fleeDir = (entity.transform.localPosition - entity.target.localPosition).normalized;
            Vector3 newPos = entity.transform.localPosition + fleeDir;
            entity.Nav.SetDestination(newPos);
            entity.Nav.speed = entity.AIData.chaseOrFleeSpeed;

            if (Vector3.Distance(entity.transform.localPosition, entity.target.localPosition) >= entity.AIData.detectRadius)
            {
                entity.checker.canChaseOrFlee = false;
                entity.ChangeState(GMStates.Return);
            }

            if (fleeTimer >= 5f)
            {
                entity.ChangeState(GMStates.Dead);
            }
            // [TODO] : 해당 state 유지 시간 5초 초과 시 Dead로 change state -> 60초 후 리스폰
        }

        public override void Exit(GeneralUnit entity)
        {
            Debug.Log("몬스터 Flee 상태 탈출.");
        }

    }

    public class Return : State<GeneralUnit>
    {
        public override void Enter(GeneralUnit entity)
        {
            Debug.Log("몬스터 Return 상태 진입.");
        }

        public override void Execute(GeneralUnit entity)
        {
            if (entity.checker.isChasingOrFleeing && !entity.checker.canChaseOrFlee)
            {
                entity.Nav.SetDestination(entity.InitialPosition);
                entity.Nav.speed = entity.AIData.patrolOrReturnSpeed;
                Debug.Log("돌아가야지~");

                Debug.Log(Vector3.Distance(entity.transform.localPosition, entity.InitialPosition));
                if (Vector3.Distance(entity.transform.localPosition, entity.InitialPosition) <= 1f)
                {
                    entity.ChangeState(GMStates.Idle);
                }

                //Return 중에 플레이어가 다시 감지 범위 내에 들어왔을 경우
                if (Vector3.Distance(entity.transform.position, entity.target.position) <= entity.AIData.detectRadius)
                {
                    entity.checker.canChaseOrFlee = true;
                    if (entity.checker.canChaseOrFlee && entity.MonsterProperty.HasFlag(EnemyProperty.Hostile))
                    {
                        entity.ChangeState(GMStates.Chase);
                    }
                    else if (entity.checker.canChaseOrFlee && !entity.MonsterProperty.HasFlag(EnemyProperty.Hostile))
                    {
                        entity.ChangeState(GMStates.Flee);
                    }
                }
            }

            
        }

        public override void Exit(GeneralUnit entity)
        {
            Debug.Log("몬스터 Return 상태 탈출.");
        }

    }

    public class Combat : State<GeneralUnit>
    {
        int doRush;

        public override void Enter(GeneralUnit entity)
        {
            Debug.Log("몬스터 Combat 상태 진입.");
            //전투 돌입 코드

            //난입
            doRush = Random.Range(1, 4);
            //인카운터 방식(공격 / 피격)

        }

        public override void Execute(GeneralUnit entity)
        {

        }

        public override void Exit(GeneralUnit entity)
        {
            Debug.Log("몬스터 Combat 상태 탈출.");
        }

    }

    public class Dead : State<GeneralUnit>
    {
        float RespawnTimer;
        public override void Enter(GeneralUnit entity)
        {
            Debug.Log("몬스터 Dead 상태 진입. 60초 후 리스폰합니다.");
            RespawnTimer = 0f;
            entity.meshRenderer.enabled = false;
            entity.mosnterCollider.enabled = false;
        }

        public override void Execute(GeneralUnit entity)
        {
            RespawnTimer += Time.deltaTime;
            if (RespawnTimer >= 60f)
            {
                entity.ChangeState(GMStates.Idle);
            }
        }

        public override void Exit(GeneralUnit entity)
        {
            Debug.Log("몬스터 Dead 상태 탈출. 리스폰합니다.");
            entity.meshRenderer.enabled = true;
            entity.mosnterCollider.enabled = true;

        }
    }
}

namespace EliteMonsterStates
{
    public class Idle : State<EliteUnit>
    {
        public override void Enter(EliteUnit entity)
        {
            Debug.Log("몬스터 IDLE 상태 진입.");
        }

        public override void Execute(EliteUnit entity)
        {
            if(Vector3.Distance(entity.transform.localPosition, entity.target.localPosition) < entity.AIData.attackRange)
            {
                entity.ChangeState(EMStates.Combat);
            }
        }

        public override void Exit(EliteUnit entity)
        {
            Debug.Log("몬스터 IDLE 상태 탈출.");
        }
    }

    public class Combat : State<EliteUnit>
    {
        public override void Enter(EliteUnit entity)
        {
            Debug.Log("몬스터 COMBAT 상태 진입.");
        }

        public override void Execute(EliteUnit entity)
        {

        }

        public override void Exit(EliteUnit entity)
        {
            Debug.Log("몬스터 COMBAT 상태 탈출.");
        }
    }

    public class Dead : State<EliteUnit>
    {
        float RespawnTimer;
        public override void Enter(EliteUnit entity)
        {
            Debug.Log("몬스터 Dead 상태 진입. 60초 후 리스폰합니다.");
            RespawnTimer = 0f;
            entity.meshRenderer.enabled = false;
            entity.mosnterCollider.enabled = false;
        }

        public override void Execute(EliteUnit entity)
        {
            RespawnTimer += Time.deltaTime;
            if(RespawnTimer >= 60f)
            {
                entity.ChangeState(EMStates.Idle);
            }
        }

        public override void Exit(EliteUnit entity)
        {
            Debug.Log("몬스터 Dead 상태 탈출. 리스폰합니다.");
            entity.meshRenderer.enabled = true;
            entity.mosnterCollider.enabled = true;

        }
    }
}