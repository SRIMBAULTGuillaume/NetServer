using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using DBUtilisation;



namespace CalcEngConsole
{
    class Program
    {
        //check macadress quand tu recupere les metrics si inconnu ajoute dans la table device
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
            var MyMetrics = Bdd.SelectAllMetricsQuarter();
            
            Verifmacaddress(Bdd.Checkmacaddress());
            CalculInsertValues(MyMetrics);
        }

        public static void Verifmacaddress(List<listmac> listmac)
        {
            int i = 0;
            foreach (var obj in listmac)
            {
                //Console.WriteLine(obj.Key);
                // Pour chaque object dans la liste my listmac
                // regarde sa mac adress, 
                //chercher cette macadress dans la bdd device
                //si tu la trouve alors rien
                // si non ajoute la ligne trouver dans mylist  avec l'adress inconnu dans la base devices
                //2 requete compare liste metric et liste devices
                var newList = listmac.Where(x => x.macaddress == obj.macaddress).ToList();
                
                    if (newList[0] == listmac[i])
                    {
                    Console.WriteLine("I know");
                    i++;
                    }
                    else
                    {
                    //add la macaddress inconnu de newList dans la bdd devices
                    DAO Bdd = new DAO();
                    String type = "Unknow";
                    int userid = 0;
                    Console.WriteLine("I don't know, who is this Macaddress ?");
                    Bdd.InsertMacAddress(type, userid, newList[0].macaddress);
                    Console.WriteLine("Call your administrator, I insert this Macaddress in my BDD ");
                }

                try
                {
                    Console.WriteLine("Pas d'erreur");
                }
                catch (Exception e)
                {
                    Console.WriteLine("Erreur list null" + e);
                }
            }
        }


      

        public static void CalculInsertValues(List<Metric> MyMetrics)
        {
            if (MyMetrics == null)
            {
                return;
                Console.WriteLine("pas de metric enregistrer depuis moin de 15 min");
            }

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


