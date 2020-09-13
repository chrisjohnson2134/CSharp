using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EventAggregatorPerformance.BasicClasses;
using System.Diagnostics;
using Prism.Events;

namespace BasicTimeTest
{


    [TestClass]
    public class UnitTest1
    {
        protected event EventHandler incrementEvent;

        protected static int cc;

        protected int numberOfEvents = 1000000;

        protected Stopwatch watch = new Stopwatch();

        [TestMethod]
        public void RegularEventTest()
        {
            cc = 0;

            Counter c = new Counter();

            watch.Start();
            for (int a = 0; a < numberOfEvents; a++)
                c.ThresholdReached += c_ThresholdReached;
            watch.Stop();
            Console.WriteLine(watch.ElapsedMilliseconds);
            watch.Reset();

            watch.Start();
            c.Add(numberOfEvents);

            while(cc <= (numberOfEvents-1))
            {}
            watch.Stop();

            if (cc == numberOfEvents)
                Assert.IsTrue(true);
            else
                Assert.Fail();

            Console.WriteLine(watch.ElapsedMilliseconds);

        }

        IEventAggregator eventAGG;

        [TestMethod]
        public void EventAggregatorTest()
        {
            eventAGG = new EventAggregator();

            Counter c = new Counter();

            watch.Start();
            for (int a = 0; a < numberOfEvents; a++)
            {
                eventAGG.GetEvent<Counter>().Subscribe(cc_Inc);
            }

            
            watch.Stop();
            Console.WriteLine(watch.ElapsedMilliseconds);
            watch.Reset();


            watch.Start();
            eventAGG.GetEvent<Counter>().Publish(0);
            c.Add(numberOfEvents);

            while (cc <= (numberOfEvents - 1))
            { }
            watch.Stop();
            Console.WriteLine(watch.ElapsedMilliseconds);

            if (cc == numberOfEvents)
                Assert.IsTrue(true);
            else
                Assert.Fail();

        }


        static void c_ThresholdReached(object sender, EventArgs e)
        {
            cc++;
        }

        private void cc_Inc(int i)
        {
            cc++;
        }
    }
}
