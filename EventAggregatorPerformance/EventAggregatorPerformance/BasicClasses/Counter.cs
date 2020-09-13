using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Events;

namespace EventAggregatorPerformance.BasicClasses
{
    public class Counter : PubSubEvent<int>
    {
        public event EventHandler ThresholdReached;

        public int threshold;
        public int Total;

        public Counter()
        {
            threshold = 0;
            Total = 0;
        }

        public void Add(int x)
        {
            Total += x;
            if (Total >= threshold)
            {
                OnThresholdReached(EventArgs.Empty);
            }
        }

        protected virtual void OnThresholdReached(EventArgs e)
        {
            EventHandler handler = ThresholdReached;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        
    }
}
