using ListDragToCanvas.ViewModels;
using MVVMEssentials.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace ListDragToCanvas.Commands
{
    public class TransactionRemovedCommand : CommandBase
    {
        private readonly TransactionsDragListViewModel _transactionDragListViewModel;

        public TransactionRemovedCommand(TransactionsDragListViewModel transactionItemViewModel)
        {
            _transactionDragListViewModel = transactionItemViewModel;

        }

        public override void Execute(object parameter)
        {
            _transactionDragListViewModel.RemoveItem(_transactionDragListViewModel.RemovedTransactionItem);
        }

    }
}
