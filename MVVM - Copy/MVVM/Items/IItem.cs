using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM.Items
{
    public interface IItem
    {
        string Name { get; set; }
        DateTime TOC { get; set; }
    }
}
