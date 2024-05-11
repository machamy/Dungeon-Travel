using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State<T> where T : class
{
    public abstract void Enter(T entity);   //들어갈 때
    public abstract void Execute(T entity); //해당 상태에서의 실행
    public abstract void Exit(T entity);    //나갈때

    public virtual string StateName => GetType().Name;

}
