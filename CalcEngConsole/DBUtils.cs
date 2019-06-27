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
        NpgsqlConnection MyCnxWindowsServer = null;
        NpgsqlConnection MyCnxJEE = null;
        public DAO()
        {
            MyCnxWindowsServer = new NpgsqlConnection(BDDWindowsServer);
            MyCnxWindowsServer.Open();
            MyCnxJEE = new NpgsqlConnection(BDDJEE);
            MyCnxJEE.Open();
        }
        /*
        public void InsertDevices(string name)
        {
            MyCnx = new NpgsqlConnection(BDDJEE);
            string insert = "INSERT INTO \"devices\"(id,name) values(DEFAULT,:name)";
            MyCnx.Open();
            MyCmd = new NpgsqlCommand(insert, MyCnx);
            MyCmd.Parameters.Add(new NpgsqlParameter("@name", NpgsqlDbType.Varchar)).Value = @name;
            MyCmd.ExecuteNonQuery(); 
            MyCnx.Close();
        }

        public void InsertMacAddress(string type, string macAddress)
        {
            MyCnx = new NpgsqlConnection(BDDJEE);
            string insert = "INSERT INTO devices (\"type\",\"macAddress\") values(:type,:macAddress)";
            MyCnx.Open();
            MyCmd = new NpgsqlCommand(insert, MyCnx);
            MyCmd.Parameters.Add(new NpgsqlParameter("type", NpgsqlDbType.Text)).Value = @type;
            MyCmd.Parameters.Add(new NpgsqlParameter("macAddress", NpgsqlDbType.Text)).Value = @macAddress;
            MyCmd.ExecuteNonQuery(); 
            MyCnx.Close();
        }
        */
        public void InsertMoyen(NpgsqlTimeStamp date, string macadress, Double value)
        {
           
            string insert = "INSERT INTO \"average\"(date,macaddress,value) values(:date,:macaddress,:value)";
            
            MyCmd = new NpgsqlCommand(insert, MyCnxWindowsServer);
            MyCmd.Parameters.Add(new NpgsqlParameter("date", NpgsqlDbType.Timestamp)).Value = date;
            MyCmd.Parameters.Add(new NpgsqlParameter("macaddress", NpgsqlDbType.Varchar)).Value = macadress;
            MyCmd.Parameters.Add(new NpgsqlParameter("value", NpgsqlDbType.Double)).Value = value;
            MyCmd.ExecuteNonQuery(); 
           
        }

        public List<Metric> SelectAllMetricsQuarter()
        {
            var MyMetrics = new List<Metric>();
            
            NpgsqlTimeStamp date = DateTime.Now.AddMinutes(-15).ToLocalTime();
            string select = "SELECT * FROM \"metrics\" WHERE (date > timestamp \'" + date + "\')";
            MyCmd = new NpgsqlCommand(select, MyCnxJEE);
            var reader = MyCmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    MyMetrics.Add(new Metric(reader.GetTimeStamp(1), reader.GetInt32(2), reader.GetString(3)));
                }
            }
            else
            {
                Console.WriteLine("No rows found.");
                MyMetrics = null;
            }
            reader.Close();
            
            return MyMetrics;
        }

        public List<listmac> Checkmacaddress()
        {
            var list = new List<listmac>();
          
            NpgsqlTimeStamp date = DateTime.Now.AddMinutes(-15).ToLocalTime();
            string select = "SELECT \"macAddress\" FROM \"metrics\" WHERE (date > timestamp \'" + date + "\')";
            MyCmd = new NpgsqlCommand(select, MyCnxJEE);
            var reader = MyCmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                   list.Add(new listmac(reader.GetString(0)));
                }
            }
            else
            {
                Console.WriteLine("No rows found.");
                list = null;
            }
            reader.Close();
            return list;
        }

        // public List<string> Checkmacdevice()
        //{
        //    var list = new List<string>();
           
        //    NpgsqlTimeStamp date = DateTime.Now.AddMinutes(-15).ToLocalTime();
        //    string select = "SELECT \"macAddress\" FROM \"devices\"";
        //    MyCmd = new NpgsqlCommand(select, MyCnxJEE);
        //    var reader = MyCmd.ExecuteReader();
        //    if (reader.HasRows)
        //    {
        //        while (reader.Read())
        //        {
        //            list.Add(reader.GetString(0));
        //        }
        //    }
        //    else
        //    {
        //        Console.WriteLine("No rows found.");
        //        list = null;
        //    }
        //    reader.Close();
        //    return list;
        //}
    }
}


//TODO suivant le type de mes devices faire telle ou tel calcul ? min max moyenne, capteur de presence 