using Scripts.FSM;
using Scripts.Game.Dungeon.Unit;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Scripts.Game.Dungeon.Unit
{
    public enum EMStates
    {
        Idle = 0,
        Combat,
        Dead
    }

    public struct EM_StateCheck
    {
        public bool canAttack;
    }

    [Serializable]
    public struct EM_AIData
    {
        public float attackRange;
    }

    public class EliteUnit : MonsterUnit
    {
        #region INITIALIZATION ELITEUNIT
        public EM_AIData AIData;
        public EM_StateCheck checker;

        private IState<EliteUnit>[] states;
        private StateMachine<EliteUnit> stateMachine;

        #endregion

        public override void Setup()
        {
            base.Setup();


            states = new IState<EliteUnit>[6];
            states[(int)EMStates.Idle] = new EliteMonsterStates.Idle();
            states[(int)EMStates.Combat] = new EliteMonsterStates.Combat();
            states[(int)EMStates.Dead] = new EliteMonsterStates.Dead();

            stateMachine = new StateMachine<EliteUnit>();
            stateMachine.Setup(this, states[(int)EMStates.Idle]);
        }

        public override void Updated()
        {
            stateMachine.Execute();
        }

        public void ChangeState(EMStates newState)
        {
            stateMachine.ChangeState(states[(int)newState]);
        }


        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, AIData.attackRange);
        }
    }

}
