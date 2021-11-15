using ListDragToCanvas.ViewModels.AbstractClasses;
using System;
using System.Collections.Generic;
using System.Text;

namespace ListDragToCanvas.ViewModels
{
    public class TransactionItemViewModel : DraggableListItemViewModel
    {
        private string _description;
        private string _amount;


        public TransactionItemViewModel(string description,string amount)
        {
            Description = description;
            Amount = amount;
        }

        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        public string Amount
        {
            get
            {
                return _amount;
            }
            set
            {
                _amount = value;
                OnPropertyChanged(nameof(Amount));
            }
        }

    }
}
