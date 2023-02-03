using DragAndDrop.ViewModels;
using MVVMEssentials.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace DragAndDrop.Commands
{
    public class SaveRectangleCommand : CommandBase
    {

        private readonly DragDropCanvasViewModel _dragDropCanvasViewModel;

        public SaveRectangleCommand(DragDropCanvasViewModel dragDropCanvasViewModel)
        {
            _dragDropCanvasViewModel = dragDropCanvasViewModel;
        }

        public override void Execute(object parameter)
        {
            MessageBox.Show($"Successfully Saved VM. X : {_dragDropCanvasViewModel.X} , Y : {_dragDropCanvasViewModel.Y}");
        }
    }
}
