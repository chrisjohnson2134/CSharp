using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM.Items
{
    public class Item : IItem
    {
        public Item(string addItemName)
        {
            Name = addItemName;
            TOC = DateTime.Now;
        }

        public string Name { get; set; }
        public DateTime TOC { get ; set ; }
    }
}
