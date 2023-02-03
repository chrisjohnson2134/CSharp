using ListDragToCanvas.ViewModels;
using ListDragToCanvas.ViewModels.AbstractClasses;
using Microsoft.VisualStudio.PlatformUI;
using MVVMEssentials.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;

namespace ListDragToCanvas.ViewModels
{
    public class TransactionsDragListViewModel : ViewModelBase
    {
        private readonly ObservableCollection<DraggableListItemViewModel> _dragableItemViewModels;
        private TransactionItemViewModel _transactionItemViewModel;
        public TransactionsDragListViewModel()
        {
            _dragableItemViewModels = new ObservableCollection<DraggableListItemViewModel>();
            _dragableItemViewModels.Add(new TransactionItemViewModel("new Item", "$5.00"));

            TransactionRemoveCommand = new DelegateCommand(TransactionRemoveCommandHandler);
        }

        public ICommand TransactionRemoveCommand { get; }

        public IEnumerable<DraggableListItemViewModel> DragableItemViewModels => _dragableItemViewModels;

        public TransactionItemViewModel RemovedTransactionItem 
        {
            get { return _transactionItemViewModel; }
            set
            {
                _transactionItemViewModel = value;
                OnPropertyChanged(nameof(RemovedTransactionItem));
            }
        }

        private void TransactionRemoveCommandHandler(object obj)
        {
            RemoveItem(RemovedTransactionItem);
        }

        public void RemoveItem(TransactionItemViewModel removedTransactionItem)
        {
            _dragableItemViewModels.Remove(removedTransactionItem);
        }

    }
}
