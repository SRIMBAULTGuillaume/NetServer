using System;

namespace CalcEngConsole
{
   public class Metric
    {
       // public int id;
        public DateTime date;
        public Double value;
        public string macadress;

        public Metric (DateTime date, Double value, string macadress)
        {
            //this.id = id;
            this.date = date;
            this.value = value;
            this.macadress = macadress;
        }
    }
    
}


