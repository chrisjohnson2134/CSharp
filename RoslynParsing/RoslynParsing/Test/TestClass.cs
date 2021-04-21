using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynParsing.Test
{
    public class TestClass : TestClassL2
    {
        public int fieldInTest;

        public TestClass()
        {
            Level1String = string.Empty;
        }

        public TestClass(string l1String)
        {
            Level1String = l1String;
        }

        public string Level1String { get; set; }

        public string returnStringExample()
        {
            return string.Empty;
        }
    }
}
