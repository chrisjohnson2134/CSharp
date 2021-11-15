using MVVMEssentials.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ListDragToCanvas.ViewModels
{
    public class BudgetCategoriesViewModel : ViewModelBase
    {
        public ObservableCollection<BudgetItemViewModel> BudgetItemsList { get; }

        public BudgetCategoriesViewModel()
        {

        }

    }
}
