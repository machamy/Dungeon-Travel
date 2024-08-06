using Scripts.FSM;
using Scripts.Game.Dungeon.Unit;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Scripts.Game.Dungeon.Unit
{
    public enum GMStates
    {
        Idle = 0,
        Chase,
        Flee,
        Return,
        Combat,
        Dead
    }

    public struct GM_StateCheck
    {
        public bool canChaseOrFlee;
        public bool isChasingOrFleeing;
        public bool isAttacking;
        public bool isDead;
        public bool isPatroling;
    }

    [Serializable]
    public struct GM_AIData
    {
        public float patrolOrReturnSpeed; 
        public float chaseOrFleeSpeed;
        public float detectRadius;
        public float moveRadius;
        public float attackRange;
    }

    public class GeneralUnit : MonsterUnit
    {
        #region INITIALIZATION GENERALUNIT
        public GM_AIData AIData;
        public GM_StateCheck checker;
        
        private IState<GeneralUnit>[] states;
        private StateMachine<GeneralUnit> stateMachine;
       
        #endregion

        public override void Setup()
        {
            base.Setup();

            #region STATEMACHINE BUILDING
            states = new IState<GeneralUnit>[6];
            states[(int)GMStates.Idle] = new GeneralMonsterStates.Idle();
            states[(int)GMStates.Chase] = new GeneralMonsterStates.Chase();
            states[(int)GMStates.Flee] = new GeneralMonsterStates.Flee();
            states[(int)GMStates.Return] = new GeneralMonsterStates.Return();
            states[(int)GMStates.Combat] = new GeneralMonsterStates.Combat();
            states[(int)GMStates.Dead] = new GeneralMonsterStates.Dead();

            stateMachine = new StateMachine<GeneralUnit>();
            stateMachine.Setup(this, states[(int)GMStates.Idle]);
            #endregion
        }

        public override void Updated()
        {
            stateMachine.Execute();
        }

        public void ChangeState(GMStates newState)
        {
            stateMachine.ChangeState(states[(int)newState]);
        }

        public void Patrol()
        {
            if (!checker.isPatroling)
            {
                float patrolRadius = AIData.moveRadius - AIData.detectRadius;
                float patrolAngle = UnityEngine.Random.Range(0, 360f);
                Vector3 nextPatrolPoint = InitialPosition + new Vector3(patrolRadius * UnityEngine.Random.Range(0f, 1f) * Mathf.Cos(Mathf.Deg2Rad * patrolAngle), 0,
                                                                        patrolRadius * UnityEngine.Random.Range(0f, 1f) * Mathf.Sin(Mathf.Deg2Rad * patrolAngle));
                // 이동 가능한지 검사
                if (NavMesh.SamplePosition(nextPatrolPoint, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
                {
                    Nav.SetDestination(nextPatrolPoint);
                    checker.isPatroling = true;
                }
            }
            else
            {
                if (!Nav.pathPending && Nav.remainingDistance < 0.01f)
                {
                    checker.isPatroling = false;
                }
            }
        }

        private void OnDrawGizmos()
        {
            // 기즈모 1 - 검출 반경 (녹색)
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, AIData.detectRadius);

            // 기즈모 2 - 이동 반경 (빨간색)
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(InitialPosition, AIData.moveRadius);

            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, AIData.attackRange);
        }
    }

}
