using System;
using System.Collections.Generic;
using CalcEngConsole;
using Npgsql;
using NpgsqlTypes;


namespace DBUtilisation
{
    public class DAO
    {
        string BDDJEE = "Server=10.151.129.35;Port=5432;Database=postgres;User Id=admin;Password=admin;"; //exia:10.151.129.35:5432 chez guigui:192.168.1.77
        string BDDWindowsServer = "Server=10.151.129.35;Port=5432;Database=calc;User Id=admin;Password=admin;"; // les identifiant de connexions a la base windows server
        NpgsqlCommand MyCmd = null;
        NpgsqlConnection MyCnx = null;

        public void InsertDevices(string name)
        {
            MyCnx = new NpgsqlConnection(BDDJEE);
            //La valeur DEFAULT parce que la propriété id est auto incrémenté "INSERT INTO \"Test\"(id,name) values(DEFAULT,:name)"
            string insert = "INSERT INTO \"devices\"(id,name) values(DEFAULT,:name)";   
            MyCnx.Open();
            MyCmd = new NpgsqlCommand(insert, MyCnx);
            MyCmd.Parameters.Add(new NpgsqlParameter("@name", NpgsqlDbType.Varchar)).Value = @name;
            MyCmd.ExecuteNonQuery(); //Exécution
            MyCnx.Close();
        }

        public void InsertMoyen(string macadress, Double value, NpgsqlTimeStamp date)
        {
            MyCnx = new NpgsqlConnection(BDDWindowsServer);
            string insert = "INSERT INTO \"average\"(id_device,type,value,date,macadress) values(:id_device,:type,:value,:date,:macadress)";
            MyCnx.Open();
            MyCmd = new NpgsqlCommand(insert, MyCnx);
            MyCmd.Parameters.Add(new NpgsqlParameter("id_device", NpgsqlDbType.Integer)).Value = 1;
            MyCmd.Parameters.Add(new NpgsqlParameter("type", NpgsqlDbType.Varchar)).Value = "";
            MyCmd.Parameters.Add(new NpgsqlParameter("value", NpgsqlDbType.Double)).Value = value;
            MyCmd.Parameters.Add(new NpgsqlParameter("date", NpgsqlDbType.Timestamp)).Value = date;
            MyCmd.Parameters.Add(new NpgsqlParameter("macadress", NpgsqlDbType.Varchar)).Value = macadress;
            MyCmd.ExecuteNonQuery(); //Exécution
            MyCnx.Close();
        }

        public List<Metric> SelectAllMetrics()
        {
            var MyMetrics = new List<Metric>();
            MyCnx = new NpgsqlConnection(BDDJEE);
            MyCnx.Open();
            // string select = "SELECT * FROM \"metrics\""; //TODO juste les donnée depuis 15 min
            DateTime randge =   ;
            DateTime date = DateTime.Now - DateTimeOffset(1);
            string select = "SELECT * FROM \"metrics\" + WHERE date = @date ";

            //https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/ado-net-code-examples#sqlclient
            //https://docs.microsoft.com/fr-fr/dotnet/standard/datetime/choosing-between-datetime


            MyCmd = new NpgsqlCommand(select, MyCnx);
            var reader = MyCmd.ExecuteReader();
            if (reader.HasRows)
            {
               while (reader.Read())   //créer la liste des metrics
                {
                    //Console.WriteLine("{0}\t{1}", reader.GetInt32(0), reader.GetString(1));
                    // s = String.Format("{0}\t{1}\t{2}\t{3}", reader.GetInt32(0), reader.GetInt32(1), reader.GetTimeStamp(2), reader.GetInt32(3));
                    //s = String.Format("{0}\t{1}\t{2}", reader.GetInt32(1), reader.GetTimeStamp(2), reader.GetInt32(3));
                    MyMetrics.Add(new Metric(reader.GetTimeStamp(2), reader.GetInt32(3), reader.GetString(4)));
                    // Console.WriteLine(<Metric>);
                }
                
            }
            else
            {
                Console.WriteLine("No rows found.");
                MyMetrics = null;
            }
            reader.Close();
            MyCnx.Close();
            return MyMetrics;
        }
     }
}//Fin

