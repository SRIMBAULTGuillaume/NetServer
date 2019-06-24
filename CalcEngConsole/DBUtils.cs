using System;
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

        public void InsertMoyen(int id_device,string type,int value)
        {
            MyCnx = new NpgsqlConnection(BDDWindowsServer);
            string insert = "INSERT INTO \"average\"(id,id_device,type,value) values(DEFAULT,:id_device,:type,:value)";
            MyCnx.Open();
            MyCmd = new NpgsqlCommand(insert, MyCnx);
            MyCmd.Parameters.Add(new NpgsqlParameter("id_device", NpgsqlDbType.Integer)).Value = id_device;
            MyCmd.Parameters.Add(new NpgsqlParameter("type", NpgsqlDbType.Varchar)).Value = type;
            MyCmd.Parameters.Add(new NpgsqlParameter("value", NpgsqlDbType.Integer)).Value = value;

            MyCmd.ExecuteNonQuery(); //Exécution
            MyCnx.Close();
        }

        public void SelectAllMetrics()
        {
            MyCnx = new NpgsqlConnection(BDDJEE);
            MyCnx.Open();
            string select = "SELECT * FROM \"metrics\"";
            MyCmd = new NpgsqlCommand(select, MyCnx);
            var reader = MyCmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    //Console.WriteLine("{0}\t{1}", reader.GetInt32(0), reader.GetString(1));
                    string s = String.Format("{0}\t{1}\t{2}", reader.GetInt32(0), reader.GetTime(1), reader.GetInt32(2));
                    //Console.WriteLine("{0}\t{1}\t{2}\t{3}", reader.GetInt32(0), reader.GetString(1), reader.GetString(3), reader.GetString(4));
                    Console.WriteLine(s);
                }
            }
            else
            {
                Console.WriteLine("No rows found.");
            }
            reader.Close();
            MyCnx.Close();
        }
     }
}//Fin

