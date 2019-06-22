using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcEngConsole
{
    class Metric
    {
        public int id;
        public DateTime date;
        public float value;

        public Metric(int id, DateTime date, float value)
        {
            this.id = id;
            this.date = date;
            this.value = value;
        }
    }

    class MetricMoy
    {
        public int id;
        public DateTime date;
        public float value;

        public MetricMoy(int id, DateTime date, float value)
        {
            this.id = id;
            this.date = date;
            this.value = value;
        }
    }
}


