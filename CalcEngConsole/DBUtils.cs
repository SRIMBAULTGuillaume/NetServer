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

        public void InsertMoyen(NpgsqlTimeStamp date, string macadress, Double value)
        {
            MyCnx = new NpgsqlConnection(BDDWindowsServer);
            string insert = "INSERT INTO \"average\"(date,macaddress,value) values(:date,:macaddress,:value)";
            MyCnx.Open();
            MyCmd = new NpgsqlCommand(insert, MyCnx);

            MyCmd.Parameters.Add(new NpgsqlParameter("date", NpgsqlDbType.Timestamp)).Value = date;
            MyCmd.Parameters.Add(new NpgsqlParameter("macaddress", NpgsqlDbType.Varchar)).Value = macadress;
            MyCmd.Parameters.Add(new NpgsqlParameter("value", NpgsqlDbType.Double)).Value = value;
            
            MyCmd.ExecuteNonQuery(); //Exécution
            MyCnx.Close();
        }

        public List<Metric> SelectAllMetrics()
        {
            var MyMetrics = new List<Metric>();
            MyCnx = new NpgsqlConnection(BDDJEE);
            MyCnx.Open();

            //long date = (DateTime.UtcNow.Ticks - DateTime.Parse("01/01/1970 00:00:00").Ticks) / 10000000 - 900 ;  //15*60*1000 = 900000
            // long unixDate = 1297380023295;
            long unixDate = DateTime.UtcNow.Ticks - DateTime.Parse("31/12/1969 23:45:00").Ticks;
            DateTime start = new DateTime(2019, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            DateTime elhg = new DateTime(unixDate);

            NpgsqlTimeStamp date = DateTime.Now.AddMinutes(-15).ToLocalTime();
            
            //TODO Va chercher la date de now moin 15 min;

            string select = "SELECT * FROM \"metrics\" WHERE (date > timestamp \'" + date + "\')";           

           


            MyCmd = new NpgsqlCommand(select, MyCnx);
            var reader = MyCmd.ExecuteReader();
            if (reader.HasRows)
            {
               while (reader.Read())   //créer la liste des metrics
                {
                    // s = String.Format("{0}\t{1}\t{2}\t{3}", reader.GetInt32(0), reader.GetInt32(1), reader.GetTimeStamp(2), reader.GetInt32(3));
                    MyMetrics.Add(new Metric(reader.GetTimeStamp(1), reader.GetInt32(2), reader.GetString(3)));
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

