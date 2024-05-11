using System;

namespace Scripts.FSM
{
    public class StateBuilder<T> where T : class
    {
        private State<T> _state;
        public StateBuilder()
        {
            _state = new State<T>();
        }


        public StateBuilder<T> SetEnterBehavior(Action<T> action)
        {
            _state.enter = action;
            return this;
        }
        public StateBuilder<T> SetExecuteBehavior(Action<T> action)
        {
            _state.execute = action;
            return this;
        }
        public StateBuilder<T> SetExitBehavior(Action<T> action)
        {
            _state.exit = action;
            return this;
        }

        public State<T> Build()
        {
            return _state;
        }
    }
}