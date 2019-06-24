using System;
using Npgsql;
using NpgsqlTypes;

namespace DBUtilisation
{
    public class DAO
    {
        string Conx = "Server=192.168.1.77;Port=5432;Database=postgres;User Id=admin;Password=admin;";
        NpgsqlCommand MyCmd = null;
        NpgsqlConnection MyCnx = null;

        public void InsertPersons(string name)
        {
            MyCnx = new NpgsqlConnection(Conx);
            //La valeur DEFAULT parce que la propriété id est auto incrémenté "INSERT INTO \"Test\"(id,name) values(DEFAULT,:name)"
            string insert = "INSERT INTO \"Test\"(id,name) values(DEFAULT,:name)";   
            MyCnx.Open();
            MyCmd = new NpgsqlCommand(insert, MyCnx);
            MyCmd.Parameters.Add(new NpgsqlParameter("@name", NpgsqlDbType.Varchar)).Value = @name;
            MyCmd.ExecuteNonQuery(); //Exécution
            MyCnx.Close();
        }

        public void SelectAllDevices()
        {
            MyCnx = new NpgsqlConnection(Conx);
            MyCnx.Open();
            string select = "SELECT * FROM \"Test\"";
            MyCmd = new NpgsqlCommand(select, MyCnx);
            var reader = MyCmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Console.WriteLine("{0}\t{1}", reader.GetInt32(0), reader.GetString(1));
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

