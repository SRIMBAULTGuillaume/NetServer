using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Npgsql;
using NpgsqlTypes;


namespace NetServer.Models
{
    public class DAO
    {
        string Conx = "Server=10.151.129.35;Port=5432;Database=calc;User Id=admin;Password=admin;";
        NpgsqlCommand MyCmd = null;
        NpgsqlConnection MyCnx = null;

        

        public ArrayList RetreiveValues(string device, int size)
        {
            ArrayList list = new ArrayList();
            MyCnx = new NpgsqlConnection(Conx);
            MyCnx.Open();
            string select = "SELECT value, date FROM \"average\" WHERE Id_Device = " + device + "ORDER BY date LIMIT " + size;
            MyCmd = new NpgsqlCommand(select, MyCnx);
            var reader = MyCmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Measure measure = new Measure();
                    measure.value = reader.GetInt32(0);
                    measure.date = reader.GetDateTime(1);
                    list.Add(measure);
                }
            }
            
            reader.Close();
            MyCnx.Close();

            return list;
        }
    }
}
