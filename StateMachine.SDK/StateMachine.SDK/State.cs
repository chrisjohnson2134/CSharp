using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace StateMachine.SDK
{
    public class State
    {
        protected List<Tuple<Func<bool>, State>> _moveState;

        public Action StateAction { get; set; }

        public string StateName { get; }

        public int OnDelayTime { get; set; }

        public int OffDelayTime { get; set; }

        public State(string stateName)
        {
            StateName = stateName;
            _moveState = new List<Tuple<Func<bool>, State>>();
        }

        public void AddStateMove(Func<bool> predicate, State state)
        {
            _moveState.Add(new Tuple<Func<bool>, State>(predicate,state));
        }

        public void ActivateState()
        {
            StateAction?.Invoke();
        }

        public State CheckMove()
        {
            foreach (var item in _moveState)
            {
                if(item.Item1.Invoke())
                    { return item.Item2; }
            }

            return null;
        }

        public override string ToString()
        {
            return $"{StateName}";
        }
    }
}
