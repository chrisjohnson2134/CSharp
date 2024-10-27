// See https://aka.ms/new-console-template for more information
using StateMachine.SDK;

State a = new State("a");
State b = new State("b");

bool key = false;

a.AddStateMove(() => key, b);
a.OffDelayTime = 2000;
a.OnDelayTime = 0;
a.StateAction = ActionAHandler;

b.AddStateMove(() => !key, a);
b.StateAction = ActionBHandler;

List<State> states = new List<State>();
states.Add(a);
states.Add(b);

StateMachineRunner runner = new StateMachineRunner(a,states);

char input = 'a';
while(input != 'q')
{
    input = Console.ReadKey().KeyChar;

    if (input == 'k')
    {
        key = true;
    }
    else if(input == 'u')
    {
        key = false;
    }

    runner.TakeAction();
}

void ActionAHandler()
{
    Console.WriteLine("a action");
}

void ActionBHandler()
{
    Console.WriteLine("b action");
}