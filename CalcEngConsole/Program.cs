using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using DBUtilisation;



namespace CalcEngConsole
{
    class Program
    {

        static void Main(string[] args)
        {
            Console.WriteLine("Je démarre");
            Console.WriteLine("Je lance une save de la moyenne");
            
            while (true)
            {
                SaveAllMetrics();
                Console.WriteLine("J'ai fini d'insérer mes calcul");
                Thread.Sleep(15*60*1000);  
            }
            
        }

        public static void SaveAllMetrics()
        {
            DAO Bdd = new DAO();
            var MyMetrics = Bdd.SelectAllMetrics();
            CalculInsertValues(MyMetrics);
        }

        public static void CalculInsertValues(List<Metric> MyMetrics)
        {
            DAO Bdd = new DAO();
            foreach (var obj in MyMetrics.GroupBy(x => x.macaddress).ToList())
            {
                //Console.WriteLine(obj.Key);
                var newList = MyMetrics.Where(x => x.macaddress == obj.Key).ToList();
                try
                {
                    Metric moyenne = CalcMoyenne(newList);
                    Bdd.InsertMoyen(moyenne.date, moyenne.macaddress, moyenne.value );
                    Console.WriteLine("Value " + moyenne.value);
                    
                }
                catch (Exception e)
                {
                    Console.WriteLine("Erreur list null" + e);
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


