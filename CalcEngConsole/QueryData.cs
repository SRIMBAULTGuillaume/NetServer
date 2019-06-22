//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Tutorial.SqlConn;
//using System.Data.SqlClient;
//using System.Data.Common;

//namespace CsSQLServerTutorial
//{
//    class QueryDataExample
//    {
//        static void Main_Bdd(string[] args)
//        {
//            // Obtenez l'objet Connection pour se connecter à DB.
//            SqlConnection conn = DBUtils.GetDBConnection();
//            conn.Open();
//            try
//            {
//                QueryEmployee(conn);
//            }
//            catch (Exception e)
//            {
//                Console.WriteLine("Error: " + e);
//                Console.WriteLine(e.StackTrace);
//            }
//            finally
//            {
//                // Closez la connexion.
//                conn.Close();
//                // Éliminez l'objet, libérant les ressources.
//                conn.Dispose();
//            }
//            Console.Read();
//        }

//        private static void QueryEmployee(SqlConnection conn)
//        {
//            string sql = "Select Emp_Id, Emp_No, Emp_Name, Mng_Id from Employee";

//            // Créez un objet Command.
//            SqlCommand cmd = new SqlCommand();

//            // Combinez l'objet Command avec Connection.
//            cmd.Connection = conn;
//            cmd.CommandText = sql;


//            using (DbDataReader reader = cmd.ExecuteReader())
//            {
//                if (reader.HasRows)
//                {

//                    while (reader.Read())
//                    {
//                        // Récupérez l'index de Column Emp_ID dans l'instruction de requête SQL.
//                        int empIdIndex = reader.GetOrdinal("Emp_Id"); // 0


//                        long empId = Convert.ToInt64(reader.GetValue(0));

//                        // L'index de colonne Emp_No = 1.
//                        string empNo = reader.GetString(1);
//                        int empNameIndex = reader.GetOrdinal("Emp_Name");// 2
//                        string empName = reader.GetString(empNameIndex);

//                        // L'index de colonne Mng_Id trong dans l'instruction de requête SQL.
//                        int mngIdIndex = reader.GetOrdinal("Mng_Id");

//                        long? mngId = null;


//                        if (!reader.IsDBNull(mngIdIndex))
//                        {
//                            mngId = Convert.ToInt64(reader.GetValue(mngIdIndex));
//                        }
//                        Console.WriteLine("--------------------");
//                        Console.WriteLine("empIdIndex:" + empIdIndex);
//                        Console.WriteLine("EmpId:" + empId);
//                        Console.WriteLine("EmpNo:" + empNo);
//                        Console.WriteLine("EmpName:" + empName);
//                        Console.WriteLine("MngId:" + mngId);
//                    }
//                }
//            }

//        }
//    }

//}