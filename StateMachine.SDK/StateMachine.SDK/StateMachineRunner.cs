
namespace StateMachine.SDK
{
    public class StateMachineRunner
    {

        public StateMachineRunner(State defaultState, List<State> states)
        {
            CurrentState = defaultState;
            DefaultState = defaultState;
            States = states;
        }

        public State CurrentState { get; private set; }

        public State DefaultState { get; set; }

        public List<State> States { get; set; }

        public void Reset()
        {
            CurrentState = DefaultState;
        }

        public State TakeAction()
        {
            var newState = CurrentState.CheckMove();
            if(newState != null)
            {
                CurrentState = newState;
            }

            return newState;
        }
    }
}
