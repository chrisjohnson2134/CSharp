using MVVM.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM.Test
{
    public class MVVMTestContext
    {
        protected MainWindowViewModel VM;

        public MVVMTestContext()
        {
            VM = new MainWindowViewModel();
        }
    }
}
