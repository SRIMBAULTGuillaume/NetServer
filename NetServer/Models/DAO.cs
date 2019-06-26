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

        

        public ArrayList RetreiveValues(string table, string device, int size)
        {
            ArrayList list = new ArrayList();
            try
            {
                MyCnx = new NpgsqlConnection(Conx);
                MyCnx.Open();
                string select = "SELECT value, date FROM \"" + table + "\"WHERE id_device = " + device + "ORDER BY date DESC LIMIT " + size;
                MyCmd = new NpgsqlCommand(select, MyCnx);
                var reader = MyCmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Measure measure = new Measure();
                        measure.value = reader.GetDouble(0);
                        measure.date = reader.GetDateTime(1);
                        list.Add(measure);
                    }
                }

                reader.Close();
                MyCnx.Close();

                return list;
            }
            catch(Npgsql.PostgresException)
            {
                list.Add("Parameter Error");
                return list;
            }
        }

        public ArrayList RetreiveValues(string table, string device, int size, int frequence)
        {
            ArrayList list = new ArrayList();
            try
            {
                int count = 0;
                double sum = 0;
                MyCnx = new NpgsqlConnection(Conx);
                MyCnx.Open();
                string select = "SELECT value, date FROM " + table + " WHERE id_device = " + device + "ORDER BY date DESC LIMIT " + size;
                MyCmd = new NpgsqlCommand(select, MyCnx);
                var reader = MyCmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        count++;
                        sum = sum + reader.GetDouble(0);
                        if (count == frequence)
                        {

                            Measure measure = new Measure();
                            measure.value = sum/frequence;
                            measure.date = reader.GetDateTime(1);
                            list.Add(measure);
                            count = 0;
                            sum = 0;
                        }
                    }
                }

                reader.Close();
                MyCnx.Close();

                return list;
            }
            catch(Npgsql.PostgresException)
            {
                list.Add("Parameter Error");
                return list;
            }
        }
    }
}
