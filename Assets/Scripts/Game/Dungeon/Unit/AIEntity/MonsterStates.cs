using Scripts.Game.Dungeon.Unit;
using Scripts.Data;
using Scripts.FSM;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GeneralMonsterStates
{
    public class Idle : IState<GeneralUnit>
    {
        public void Enter(GeneralUnit entity)
        {
            Debug.Log("몬스터 IDLE 상태 진입.");
        }

        public void Execute(GeneralUnit entity)
        {
            if (Vector3.Distance(entity.transform.position, entity.target.position) <= entity.AIData.detectRadius)
            {
                entity.checker.canChaseOrFlee = true;
                if (entity.checker.canChaseOrFlee && entity.MonsterProperty.HasFlag(EnemyProperty.Hostile))
                {
                    entity.ChangeState(GMStates.Chase);
                }
                /*
                // 적 속성 두개로 감소됨 - machamy
                // else if (entity.checker.canChaseOrFlee && !entity.MonsterProperty.HasFlag(EnemyProperty.Chase))
                // {
                //     entity.ChangeState(GMStates.Flee);
                // }*/
            }

            //entity.Nav.SetDestination(entity.target.position);

            if (entity.MonsterProperty.HasFlag(EnemyProperty.Movement))
            {
                entity.Patrol();
            }           
        }

        public  void Exit(GeneralUnit entity)
        {
            Debug.Log("몬스터 IDLE 상태 탈출.");
        }

    }

    public class Chase : IState<GeneralUnit>
    {
        public  void Enter(GeneralUnit entity)
        {
            Debug.Log("몬스터 Chase 상태 진입.");
            entity.checker.isChasingOrFleeing = true;
        }

        public void Execute(GeneralUnit entity)
        {           
            if (!entity.checker.canChaseOrFlee) return;

            float enemyDistance = Vector3.Distance(entity.transform.localPosition, entity.InitialPosition);
            if (enemyDistance >= entity.AIData.moveRadius - 0.2f)
            {
                entity.checker.canChaseOrFlee= false;
                entity.ChangeState(GMStates.Return);
            }
            else if (Vector3.Distance(entity.target.localPosition, entity.InitialPosition) < entity.AIData.moveRadius)
            {
                entity.checker.isChasingOrFleeing = true;
                entity.Nav.speed = entity.AIData.chaseOrFleeSpeed;
                entity.Nav.SetDestination(entity.target.localPosition);
                Debug.Log(entity.Nav.destination);
            }
            
        }

        public void Exit(GeneralUnit entity)
        {
            Debug.Log("몬스터 Chase 상태 탈출.");
        }

    }

    public class Flee : IState<GeneralUnit>
    {
        private float fleeTimer;
        public  void Enter(GeneralUnit entity)
        {
            Debug.Log("몬스터 Flee 상태 진입.");
            entity.checker.isChasingOrFleeing = true;
            fleeTimer = 0f;
        }

        public  void Execute(GeneralUnit entity)
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

        public  void Exit(GeneralUnit entity)
        {
            Debug.Log("몬스터 Flee 상태 탈출.");
        }

    }

    public class Return : IState<GeneralUnit>
    {
        public  void Enter(GeneralUnit entity)
        {
            Debug.Log("몬스터 Return 상태 진입.");
        }

        public void Execute(GeneralUnit entity)
        {
            if (entity.checker.isChasingOrFleeing && !entity.checker.canChaseOrFlee)
            {
                entity.Nav.SetDestination(entity.InitialPosition);
                entity.Nav.speed = entity.AIData.patrolOrReturnSpeed;
                Debug.Log("돌아가야지~");

                Debug.Log(Vector3.Distance(entity.transform.localPosition, entity.InitialPosition));
                if (Vector3.Distance(entity.transform.localPosition, entity.InitialPosition) <= 0.5f)
                {
                    entity.ChangeState(GMStates.Idle);
                }

                //Return 중에 플레이어가 다시 감지 범위 내에 들어왔을 경우
                if (Vector3.Distance(entity.transform.position, entity.target.position) <= entity.AIData.detectRadius &&
                    Vector3.Distance(entity.target.position, entity.InitialPosition) <= entity.AIData.moveRadius - 0.2f)
                {
                    entity.checker.canChaseOrFlee = true;
                    if (entity.checker.canChaseOrFlee && entity.MonsterProperty.HasFlag(EnemyProperty.Hostile))
                    {
                        entity.ChangeState(GMStates.Chase);
                    }
                    /*//else if (entity.checker.canChaseOrFlee && !entity.MonsterProperty.HasFlag(EnemyProperty.Hostile))
                    //{
                    //    entity.ChangeState(GMStates.Flee);
                    //}*/
                }
            }

            
        }

        public  void Exit(GeneralUnit entity)
        {
            Debug.Log("몬스터 Return 상태 탈출.");
        }

    }

    public class Combat : IState<GeneralUnit>
    {
        int doRush;

        public  void Enter(GeneralUnit entity)
        {
            Debug.Log("몬스터 Combat 상태 진입.");
            //전투 돌입 코드

            //난입
            doRush = Random.Range(1, 4);
            //인카운터 방식(공격 / 피격)

            SceneManager.LoadScene("BattleScene");

        }

        public  void Execute(GeneralUnit entity)
        {

        }

        public  void Exit(GeneralUnit entity)
        {
            Debug.Log("몬스터 Combat 상태 탈출.");
        }

    }

    public class Dead : IState<GeneralUnit>
    {
        float RespawnTimer;
        public  void Enter(GeneralUnit entity)
        {
            Debug.Log("몬스터 Dead 상태 진입. 60초 후 리스폰합니다.");
            RespawnTimer = 0f;
            entity.meshRenderer.enabled = false;
            entity.mosnterCollider.enabled = false;
        }

        public  void Execute(GeneralUnit entity)
        {
            RespawnTimer += Time.deltaTime;
            if (RespawnTimer >= 60f)
            {
                entity.ChangeState(GMStates.Idle);
            }
        }

        public  void Exit(GeneralUnit entity)
        {
            Debug.Log("몬스터 Dead 상태 탈출. 리스폰합니다.");
            entity.meshRenderer.enabled = true;
            entity.mosnterCollider.enabled = true;

        }
    }
}

namespace EliteMonsterStates
{
    public class Idle : IState<EliteUnit>
    {
        public void Enter(EliteUnit entity)
        {
            Debug.Log("몬스터 IDLE 상태 진입.");
        }

        public void Execute(EliteUnit entity)
        {
            if(Vector3.Distance(entity.transform.localPosition, entity.target.localPosition) < entity.AIData.attackRange)
            {
                entity.ChangeState(EMStates.Combat);
            }
        }

        public void Exit(EliteUnit entity)
        {
            Debug.Log("몬스터 IDLE 상태 탈출.");
        }
    }

    public class Combat : IState<EliteUnit>
    {
        public void Enter(EliteUnit entity)
        {
            Debug.Log("몬스터 COMBAT 상태 진입.");
        }

        public void Execute(EliteUnit entity)
        {
            
        }

        public void Exit(EliteUnit entity)
        {
            Debug.Log("몬스터 COMBAT 상태 탈출.");
        }
    }

    public class Dead : IState<EliteUnit>
    {
        float RespawnTimer;
        public void Enter(EliteUnit entity)
        {
            Debug.Log("몬스터 Dead 상태 진입. 60초 후 리스폰합니다.");
            RespawnTimer = 0f;
            entity.meshRenderer.enabled = false;
            entity.mosnterCollider.enabled = false;
        }

        public void Execute(EliteUnit entity)
        {
            RespawnTimer += Time.deltaTime;
            if(RespawnTimer >= 60f)
            {
                entity.ChangeState(EMStates.Idle);
            }
        }

        public void Exit(EliteUnit entity)
        {
            Debug.Log("몬스터 Dead 상태 탈출. 리스폰합니다.");
            entity.meshRenderer.enabled = true;
            entity.mosnterCollider.enabled = true;

        }
    }
}