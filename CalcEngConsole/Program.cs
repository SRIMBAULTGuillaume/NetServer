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
            DAO Bdd = new DAO();    //Instaciation
                                    //TODO faire la boucle au propre
                                    //OK SreenDevices();
                                    //OK Bdd.InsertPersons("guigui42");  
           // var MyMetrics = new List<Metric>();

            Console.WriteLine("La liste des metrics :"); 
            var MyMetrics = Bdd.SelectAllMetrics();
            CalculInsertValues(MyMetrics);
            //Console.WriteLine(MyMetrics);
            // SreenMetrics();
           //Console.WriteLine( CalcMoyenne(MyMetrics));

           //OK Bdd.InsertMoyen(3,"temperature",18);
           // Console.WriteLine("J'ai fini d'insérer mes calcul");

            OnPause();
            Console.WriteLine("J'ai fini");
            Console.ReadLine();

        }

        //TODO regroupe les metric par unité de temps

        public static void OnPause()
        {
           Thread.Sleep(1000);
        }

        //public static void PrintValues(List<Metric> myList)
        //{
        //    foreach (var obj in myList.GroupBy(x => x.macadress).ToList())
        //    {
        //        //Console.WriteLine(obj.Key);
        //        var newList = myList.Where(x => x.macadress == obj.Key).ToList();
        //        try
        //        {
        //            Metric moyenne = CalcMoyenne(newList);
        //            Console.WriteLine(moyenne.value);
        //        }
        //        catch (Exception e)
        //        {
        //            Console.WriteLine("Erreur list null" + e);
        //        }
        //    }
        //}

        public static void CalculInsertValues(List<Metric> MyMetrics)
        {
            DAO Bdd = new DAO();
            foreach (var obj in MyMetrics.GroupBy(x => x.macadress).ToList())
            {
                //Console.WriteLine(obj.Key);
                var newList = MyMetrics.Where(x => x.macadress == obj.Key).ToList();
                try
                {
                    Metric moyenne = CalcMoyenne(newList);
                    Bdd.InsertMoyen(moyenne.macadress, moyenne.value, moyenne.date);
                    Console.WriteLine("Value " + moyenne.value);
                    //TODO envoyé cette liste a la fonction insert DAO
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
            return new Metric(date, somme, MyMetrics[0].macadress);
        }

        //static Metric CalcMoyenne(List<Metric> list)
        //{
        //    if (list.Count() == 0)
        //       throw new Exception();
            
        //    var somme = 0f;
        //    var date = list[0].date;
        //    foreach (Metric obj in list)
        //    {
        //        somme = obj.value + somme;
        //        if (date > obj.date)
        //        {
        //            date = obj.date;
        //        }

        //    }
        //    somme = somme/list.Count();
        //    return new Metric(date, somme, MyMetrics[0].macadress);
        //}

    }
}


