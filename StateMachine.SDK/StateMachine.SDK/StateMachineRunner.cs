using Timer = System.Timers.Timer;

namespace StateMachine.SDK
{
    public class StateMachineRunner
    {
        bool _movingState = false;

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
                MoveToState(newState);
            }

            return newState;
        }

        private async void MoveToState(State newState)
        {
            if(!_movingState)
            {
                _movingState = true;

                Console.Out.WriteLine("Move State");
                if (CurrentState.OffDelayTime != 0)
                    await Task.Delay(CurrentState.OffDelayTime);

                CurrentState = newState;

                if (CurrentState.OnDelayTime != 0)
                    await Task.Delay(CurrentState.OnDelayTime);

                CurrentState.ActivateState();
                Console.Out.WriteLine("Move State Finished");

                _movingState = false;
            }
        }
    }
}
