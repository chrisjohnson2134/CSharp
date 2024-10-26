// See https://aka.ms/new-console-template for more information
using StateMachine.SDK;

State a = new State("a");
State b = new State("b");

bool userInput = false;


a.AddStateMove(() => userInput, b);
b.AddStateMove(() => userInput, a);

List<State> states = new List<State>();
states.Add(a);
states.Add(b);

StateMachineRunner runner = new StateMachineRunner(a,states);

Console.WriteLine($"Take Action {userInput} - Current State {runner.CurrentState}");

userInput = !userInput;
runner.TakeAction();
Console.WriteLine($"Take Action {userInput} - Current State {runner.CurrentState}");

userInput = !userInput;
runner.TakeAction();
Console.WriteLine($"Take Action {userInput} - Current State {runner.CurrentState}");

userInput = !userInput;
runner.TakeAction();
Console.WriteLine($"Take Action {userInput} - Current State {runner.CurrentState}");
