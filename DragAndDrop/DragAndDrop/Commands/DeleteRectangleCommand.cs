using DragAndDrop.ViewModels;
using MVVMEssentials.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace DragAndDrop.Commands
{
    public class DeleteRectangleCommand : CommandBase
    {
        private readonly DragDropCanvasViewModel _canvasViewModel;

        public DeleteRectangleCommand(DragDropCanvasViewModel canvasViewModel)
        {
            _canvasViewModel = canvasViewModel;
        }

        public override void Execute(object parameter)
        {
            //MessageBox.Show(_canvasViewModel.RemoveRectangleName);//drag drop don't get along sometimes
        }
    }
}
