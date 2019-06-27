using System;

namespace CalcEngConsole
{
   public class Metric
    {
     
        public DateTime date;
        public Double value;
        public string macaddress;

        public Metric (DateTime date, Double value, string macadress)
        {
          
            this.date = date;
            this.value = value;
            this.macaddress = macadress;
        }     

    }

    public class listmac
    {
        public string macaddress;

        public listmac(string macadress)
        {
            this.macaddress = macadress;
        }
    }

  

}


