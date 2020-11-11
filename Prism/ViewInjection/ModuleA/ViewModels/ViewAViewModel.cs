using Framework.Commands;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModuleA.ViewModels
{
    class ViewAViewModel
    {
        public string ResponseText { get; set; }
        public IApplicationCommands _applicationCommands;
        public DelegateCommand UpdateCommand { get;private set; }

        public ViewAViewModel(IApplicationCommands applicationCommands)
        {
            ResponseText = "Bear ViewA";
            _applicationCommands = applicationCommands;
            _applicationCommands.PokeCommand.RegisterCommand(UpdateCommand);
        }
    }
}
