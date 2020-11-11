using Prism.Commands;

namespace Framework.Commands
{
    public interface IApplicationCommands
    {
        CompositeCommand PokeCommand { get; }
    }
}
