using Prism.Commands;

namespace Framework.Commands
{
    class ApplicationCommands : IApplicationCommands
    {
        public CompositeCommand PokeCommand { get; } = new CompositeCommand();
    }
}
