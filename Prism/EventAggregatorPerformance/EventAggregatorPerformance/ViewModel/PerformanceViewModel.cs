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

        private static int staticCounterVariable = 0;
        private int nonStaticCounterVariable = 0;

        Counter standardCounter = new Counter();



        IEventAggregator eventAGG;



        public RelayCommand RunCommand { get; set; }
        public RelayCommand SubscribeEventsCommand { get; set; }

        string _aggregatorTime;
        public string AggregatorTime 
        {
            get 
            {
                return _aggregatorTime;
            }
            set 
            {
                _aggregatorTime = value;
                OnPropertyRaised("AggregatorTime");
            }
        }

        string _standardTime;
        public string StandardTime
        {
            get
            {
                return _standardTime;
            }
            set
            {
                _standardTime = value;
                OnPropertyRaised("StandardTime");
            }
        }

        public int _numberOfEvents = 100000;
        public string NumberOfEvents
        {
            get
            {
                return Convert.ToString(_numberOfEvents);
            }
            set
            {
                if (!Int32.TryParse(value, out _numberOfEvents))
                    _numberOfEvents = 0;
                OnPropertyRaised("NumberOfEvents");
            }
        }


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
            RunCommand = new RelayCommand(RunCommandMethod);
            SubscribeEventsCommand = new RelayCommand(SubscribeEvents);

            _aggregatorTime = "Aggregator Time Microseconds : ";
            _standardTime = "Standard Time Microseconds : ";
            _numberOfEvents = 100000;
        }

        public void RunCommandMethod()
        {
            RegularEventTest();
            EventAggregatorTest();
        }

        public void SubscribeEvents()
        {
            AggregatorTime = "Aggregator Time Microseconds : ";
            StandardTime = "Standard Time Microseconds : ";
            nonStaticCounterVariable = 0;

            standardCounter.threshold = _numberOfEvents;

            eventAGG = new EventAggregator();

            for (int a = 0; a < _numberOfEvents; a++)
            {
                standardCounter.ThresholdReached += c_ThresholdReached;
                eventAGG.GetEvent<Counter>().Subscribe(cc_Inc);
            }
        }

        public void RegularEventTest()
        {
            nonStaticCounterVariable = 0;
            Stopwatch watch = new Stopwatch();

            watch.Start();
            standardCounter.Total = 0;
            standardCounter.Add(_numberOfEvents);

            while (staticCounterVariable <= (_numberOfEvents - 1))
            { }
            watch.Stop();

            _standardTime = "Standard Time Microseconds : ";
            StandardTime += watch.ElapsedMilliseconds;

            Console.WriteLine(watch.ElapsedMilliseconds);

        }


        public void EventAggregatorTest()
        {
            Stopwatch watch = new Stopwatch();
            Counter c = new Counter();

            watch.Start();
            eventAGG.GetEvent<Counter>().Publish(0);
            c.Add(_numberOfEvents);

            while (nonStaticCounterVariable <= (_numberOfEvents - 1))
            { }
            watch.Stop();

            AggregatorTime = "Aggregator Time Microseconds : ";
            AggregatorTime += watch.ElapsedMilliseconds;
            
            Console.WriteLine("finished");
        }


        static void c_ThresholdReached(object sender, EventArgs e)
        {
            staticCounterVariable++;
        }

        private void cc_Inc(int i)
        {
            nonStaticCounterVariable++;
        }
    }
}

