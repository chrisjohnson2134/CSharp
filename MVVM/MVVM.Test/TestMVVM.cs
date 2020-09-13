using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MVVM.Test
{
    [TestClass]
    public class TestMVVM : MVVMTestContext
    {
        [TestMethod]
        public void AddItemTest()
        {
            var obj = new object();
            VM.AddItemName = "Test Item.";
            VM.AddCommand.Execute(obj);

            Assert.AreEqual("Test Item.", VM.ItemList[0].Name);

        }
    }
}
