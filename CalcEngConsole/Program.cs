using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using DBUtilisation;

namespace CalcEngConsole
{
    class Program
    {
        static DAO Bdd = new DAO();
        static void Main(string[] args)
        {
            Console.WriteLine("Je démarre");
            Console.WriteLine("Je lance une save de la moyenne");
            //SelectAllMetricsbySinceYear();
            while (true)
            {
                SaveAllMetricsbyQuarter();
                //SelectAllMetricsbySinceYear();
                Console.WriteLine("J'ai fini d'insérer mes calculs");
                Thread.Sleep(15*60*1000);
               // Thread.Sleep(30000);
            }           
        }

        public static void SaveAllMetricsbyQuarter()
        {
           
            var MyMetrics = Bdd.SelectAllMetricsQuarter();            
           // Verifmacaddress(Bdd.Checkmacaddress());
            CalculInsertValues(MyMetrics);
        }

        public static void SelectAllMetricsbySinceYear()
        {
            DateTime date1 = DateTime.Now.AddMonths(-1).ToLocalTime(); ;
            DateTime date2 = date1.AddMinutes(15).ToLocalTime();

            while (date2 < DateTime.Now)
            {
                var MyMetrics = Bdd.SelectAllMetricsSinceYear(date1, date2);

                CalculInsertValues(MyMetrics);
                date1 = date2;
                date2 = date2.AddMinutes(15).ToLocalTime();
            }
            
        }


            public static void CalculInsertValues(List<Metric> MyMetrics)
        {
            if (MyMetrics == null)
            {
                Console.WriteLine("Pas de metric enregistrer depuis moins de 15 min");
                return;               
            }

           
            foreach (var obj in MyMetrics.GroupBy(x => x.macaddress).ToList())
            { 
                var newList = MyMetrics.Where(x => x.macaddress == obj.Key).ToList();
                try
                {
                    Metric moyenne = CalcMoyenne(newList);
                    Bdd.InsertMoyen(moyenne.date, moyenne.macaddress, moyenne.value );
                    Console.WriteLine("Value " + moyenne.value);                   
                }
                catch (Exception e)
                {
                    Console.WriteLine("Erreur list metric null" + e);
                }
            }
        }

        static Metric CalcMoyenne(List<Metric> MyMetrics)
        {      
            if (MyMetrics.Count() == 0)
                throw new Exception();
            Double somme = 0;
            var date = MyMetrics[0].date;
            foreach (Metric obj in MyMetrics)
            {
                somme = obj.value + somme;
                if (date > obj.date)
                {
                    date = obj.date;
                }    
            }
            somme = somme / MyMetrics.Count();
            return new Metric(date, somme, MyMetrics[0].macaddress);
        }
    }
}


