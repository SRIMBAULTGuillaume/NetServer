using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tutorial.SqlConn;
using System.Data.SqlClient;
using System.Data.Common;
using System.Collections;

namespace CalcEngConsole
{
    class Program
    {
        static void Main(string[] args)
        {  
            Console.WriteLine("Se connecte a la Bdd");
            SreenDevices();
            Console.Read();    
        }

        

        public static void PrintValues(List<Metric> myList)
        {
            foreach (var obj in myList.GroupBy(x => x.id).ToList())
            {
                //Console.WriteLine(obj.Key);
                //Je selectionne parmis myList les elements dont l'id egale à 1
                var newList = myList.Where(x => x.id == obj.Key).ToList();
                try
                {
                    MetricMoy moyenne = CalcMoyenne(newList);
                    Console.WriteLine(moyenne.value);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Erreur list null");
                }

            }





        }

      public static void SreenDevices()
        {
            var MyDevices = new List<Metric>();
            MyDevices.Add(new Metric(42, DateTime.Now, 20));
            MyDevices.Add(new Metric(42, DateTime.Now, 18));
            MyDevices.Add(new Metric(47, DateTime.Now, 18));

            var rand = new Random();
            for (int i = 0; i<100; i++)
            {
                var a = rand.Next(50);
                MyDevices.Add(new Metric(48, DateTime.Now, a));
            }


            // Displays the properties and values of the ArrayList.
            Console.WriteLine("My devices");
            //Console.WriteLine("    Numbers of Devices:    {0}", MyDevices.Count);
            //Console.WriteLine("    Total Devices: {0}", MyDevices.Capacity);
            
            PrintValues(MyDevices);
        }


        static MetricMoy CalcMoyenne(List<Metric> list)
        {
            if (list.Count() == 0)
               throw new Exception();
            
            var somme = 0f;
            var date = list[0].date;
            foreach (Metric obj in list)
            {
                somme = obj.value + somme;
                if (date > obj.date)
                {
                    date = obj.date;
                }

            }
            somme = somme/list.Count();
            return new MetricMoy(list[0].id, date, somme);


        }

       


    }





    // ID device/ date d'insertion / valeur
    //https://o7planning.org/fr/10515/utilisation-d-une-base-de-donnees-sql-server-en-utilisant-csharp
}


