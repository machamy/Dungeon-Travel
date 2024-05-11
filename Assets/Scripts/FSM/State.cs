﻿using System;

namespace Scripts.FSM
{
    public class State<T> : IState<T> where T : class
    {
        public Action<T> enter;
        public Action<T> execute;
        public Action<T> exit;
        public void Enter(T entity) => enter.Invoke(entity);   //들어갈 때
        public void Execute(T entity) => execute.Invoke(entity); //해당 상태에서의 실행
        public void Exit(T entity) => exit.Invoke(entity);    //나갈때
        public virtual string StateName => GetType().Name;
    }
}