using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine<T> where T : class
{
    private T ownerEntity;
    private State<T> currentState;

    public void Setup(T owner, State<T> entryState)
    {
        ownerEntity = owner;
        currentState = entryState;       
    }

    /// <summary>
    /// Update에서 매 프레임 해당 상태에서의 행동을 호출하기 위한 함수.
    /// </summary>
    public void Execute()
    {
        if (currentState != null)
        {
            currentState.Execute(ownerEntity);
        }
    }

    /// <summary>
    /// State를 newState로 교체하는 함수.
    /// </summary>
    /// <param name="newState"></param>
    public void ChangeState(State<T> newState)
    {
        if (newState == null)
        {
            return;
        }

        if (currentState != null)
        {
            currentState.Exit(ownerEntity);
        }

        currentState = newState;
        currentState.Enter(ownerEntity);
    }
}