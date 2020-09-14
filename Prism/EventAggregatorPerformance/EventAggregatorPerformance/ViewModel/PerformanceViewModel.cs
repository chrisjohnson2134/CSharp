using EventAggregatorPerformance.BasicClasses;
using MVVM.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventAggregatorPerformance.ViewModel
{
    class PerformanceViewModel : INotifyPropertyChanged
    {
        public event EventHandler incrementEvent;

        static int ccc;
        public int cc { 
            get { return ccc; }
            set
            {
                ccc = value;
                OnPropertyRaised("cc");
            } 
        }

        public int numberOfEvents = 1000000;

        public Stopwatch watch = new Stopwatch();

        IEventAggregator eventAGG;

        public RelayCommand RunCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyRaised(string propertyname)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
            }
        }

        public PerformanceViewModel()
        {
            RunCommand = new RelayCommand(EventAggregatorTest);
        }

        //public void RegularEventTest()
        //{
        //    cc = 0;

        //    Counter c = new Counter();

        //    watch.Start();
        //    for (int a = 0; a < numberOfEvents; a++)
        //        c.ThresholdReached += c_ThresholdReached;
        //    watch.Stop();
        //    Console.WriteLine(watch.ElapsedMilliseconds);
        //    watch.Reset();

        //    watch.Start();
        //    c.Add(numberOfEvents);

        //    while (cc <= (numberOfEvents - 1))
        //    { }
        //    watch.Stop();

        //    //if (cc == numberOfEvents)
        //    //    Assert.IsTrue(true);
        //    //else
        //    //    Assert.Fail();

        //    Console.WriteLine(watch.ElapsedMilliseconds);

        //}

        

        public void EventAggregatorTest()
        {
            cc = 0;
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

            //if (cc == numberOfEvents)
            //    Assert.IsTrue(true);
            //else
            //    Assert.Fail();

        }


        //static void c_ThresholdReached(object sender, EventArgs e)
        //{
        //    cc++;
        //}

        private void cc_Inc(int i)
        {
            cc++;
        }
    }
}

