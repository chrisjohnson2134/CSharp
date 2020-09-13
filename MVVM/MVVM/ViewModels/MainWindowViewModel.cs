using MVVM.Commands;
using MVVM.Items;
using System;
using System.ComponentModel;

namespace MVVM.ViewModels
{
    public class MainWindowViewModel
    {
        public string AddItemName {get;set;}
        public BindingList<IItem> ItemList { get; set; }

        public RelayCommand AddCommand { get; set; }

        public MainWindowViewModel()
        {
            AddCommand = new RelayCommand(AddCommandAction);

            AddItemName = String.Empty;
            ItemList = new BindingList<IItem>();
        }

        public void AddCommandAction()
        {
            ItemList.Add(new Item(AddItemName));
        }
    }
}
