using MVVMEssentials.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ListDragToCanvas.ViewModels
{
    public class DragSortViewModel : ViewModelBase
    {

        public TransactionsDragListViewModel TransactionList { get; }

        public DragSortViewModel(TransactionsDragListViewModel transactionsDragListViewModel)
        {
            TransactionList = transactionsDragListViewModel;
        }

    }
}
