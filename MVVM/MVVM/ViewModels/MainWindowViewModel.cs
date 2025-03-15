using MVVM.Commands;
using MVVM.Items;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Controls;

namespace MVVM.ViewModels
{
    public class MainWindowViewModel
    {
        public string AddItemName { get; set; }
        public BindingList<IItem> ItemList { get; set; }
        public List<string> Options { get; set; }
        public RelayCommand AddCommand { get; set; }

        public MainWindowViewModel()
        {
            AddCommand = new RelayCommand(AddCommandAction);

            AddItemName = String.Empty;
            ItemList = new BindingList<IItem>();

            for (int i = 0; i < 10000; i++)
            {
                ItemList.Add(new Item(i.ToString()));
                ItemList.Add(new Item("Ford"));
                ItemList.Add(new Item(i.ToString()));
            }
        }

        public void AddCommandAction()
        {
            ItemList.Add(new Item(AddItemName));
        }

    }
}
