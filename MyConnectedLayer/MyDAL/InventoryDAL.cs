using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// using namespaces for and DB and ADO.NET API
using System.Data;
using System.Data.SqlClient;


namespace MyDAL
{
    /// <summary>
    /// 3 tiers architecture:
    /// 1. PL (Presentation layer)
    /// 2. BLL (Business Logic Layer)
    /// 3. DAL (Data Access Layer)
    /// </summary>
    public class InventoryDAL
    {
        #region Connect and Disconnect

        // create db connection 
        private SqlConnection sqlCon =
            new SqlConnection();

        // open connection to DB using open()
        public void OpenConnection(string connectionString)
        {
            sqlCon.ConnectionString = connectionString;
            try
            {
                sqlCon.Open();
                Console.WriteLine("Connection establlished!");
            }
            catch (SqlException e)
            {
                Console.WriteLine("{0}", e.Message);
            }
            //finally
            //{
            //    sqlCon.Close();
            //}
        }
        // Close connection:
        // There are 2 options to manage the connection object
        // 1. we can use Close() method - in this case the instance will remain availble 
        // at the heap
        // 2. using Dispose pattern - in this case the object will be terminated!
        // close connection 
        public void CloseConnection()
        {
            sqlCon.Close();
        }

        #endregion

        #region Insert Logic
        // Logic insert - will be based on parameterized variables
        // for security connection between the client and the DB 
        public void InsertAuto(string make, string color, string carName)
        {
            // writing the SQL query 
            // we must use parameters as placeholders in order to avoid 
            // first world attack - Injection 
            string sql =
                string.Format
                ("Insert into tblInventory " +
                " (Make, Color, CarName) " +
                " Values " +
                " (@Make, @Color, @CarName)");

            // Bad usage 
            //string badSql =
            //    string.Format
            //    (" insert into tblInventory " + 
            //    " (Make, color, carName ) " +
            //    " Values " + 
            //    " ('{0}', '{1}', '{2}' ) ", make, color, carName);

            // using dispose pattern
            // recommend the GC to work 
            using (SqlCommand cmd = new SqlCommand(sql, this.sqlCon))
            {
                // create the parameter references 
                SqlParameter param = new SqlParameter();
                param.ParameterName = "@Make";
                param.Value = make;
                param.SqlDbType = SqlDbType.NVarChar;
                param.Size = 20;
                cmd.Parameters.Add(param);

                param = new SqlParameter();
                param.ParameterName = "@Color";
                param.Value = color;
                param.SqlDbType = SqlDbType.NVarChar;
                param.Size = 15;
                cmd.Parameters.Add(param);

                param = new SqlParameter();
                param.ParameterName = "@CarName";
                param.Value = carName;
                param.SqlDbType = SqlDbType.NVarChar;
                param.Size = 20;
                cmd.Parameters.Add(param);

                try
                {
                    // to run the SqlParameter we will use ExecuteNonQuery()
                    cmd.ExecuteNonQuery();
                }
                catch (SqlException e)
                {
                    Console.WriteLine("{0}", e.Message);
                }

            }

        }

        #endregion 

        #region Delete Logic
        public void DeleteCar(int id)
        {
            string sql =
                string.Format
                ("Delete from tblInventory " +
                " where CarID = @CarID");

            using (SqlCommand cmd = new SqlCommand(sql, this.sqlCon))
            {
                SqlParameter param = new SqlParameter();
                param.ParameterName = "@CarID";
                param.Value = id;
                param.SqlDbType = SqlDbType.Int;
                cmd.Parameters.Add(param);
                try
                {
                    // to run the SqlParameter we will use ExecuteNonQuery()
                    cmd.ExecuteNonQuery();
                }
                catch (SqlException e)
                {
                    Exception error = new Exception("No car was deleted!", e);
                    // throw error;
                }

            }

        }

        #endregion 

        #region Update Logic

        public void UpdateCarName(int id, string newCarName)
        {
            string sql = string.Format("Update tblInventory " +
                                        " SET CarName = @CarName " +
                                        " Where CarID = @CarID");
            using (SqlCommand cmd = new SqlCommand(sql, this.sqlCon))
            {
                SqlParameter param = new SqlParameter();
                param.ParameterName = "@CarName";
                param.Value = newCarName;
                param.SqlDbType = SqlDbType.NVarChar;
                param.Size = 20;
                cmd.Parameters.Add(param);

                param = new SqlParameter();
                param.ParameterName = "@CarID";
                param.Value = id;
                param.SqlDbType = SqlDbType.Int;
                cmd.Parameters.Add(param);

                try
                {
                    // to run the SqlParameter we will use ExecuteNonQuery()
                    cmd.ExecuteNonQuery();
                }
                catch (SqlException e)
                {
                    Exception error = new Exception("No car was updated!", e);
                    // throw error;
                }


            }
        }

        #endregion 

        #region Select Logic

        public DataTable GetAllInventory()
        {
            // DataTable - is actually one table from the db
            // DataSet - number of data tables 
            DataTable inv = new DataTable();
            string sql =
                string.Format("select * from tblInventory");
            // fetching the data base 
            // we need to use SqlDataReader that enabling us to read data
            // directly from the DN using ExecuteReader() method.
            // The class SqlDataReader MUST be explicitly closed.
            using (SqlCommand cmd = new SqlCommand(sql, this.sqlCon))
            {
                SqlDataReader dr = cmd.ExecuteReader();

                try
                {
                    inv.Load(dr);
                }
                catch (SqlException ex)
                {
                    Exception error = new Exception("Error reading data!", ex);
                }
                finally
                {
                    dr.Close();
                }

            }
            return inv;
        }

        #endregion

        #region Stored procedure Logic

        public string SearchForCarName(int carId)
        {
            string carName = string.Empty;

            using (SqlCommand cmd = new SqlCommand("GetCarName", this.sqlCon))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                // Get input parameter
                SqlParameter param = new SqlParameter();
                param.ParameterName = "@CarID";
                param.Value = carId;
                param.SqlDbType = SqlDbType.Int;
                param.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(param);

                // Get output parameter
                param = new SqlParameter();
                param.ParameterName = "@CarName";
                param.SqlDbType = SqlDbType.NVarChar;
                param.Size = 10;
                param.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(param);

                // Execute SP
                cmd.ExecuteNonQuery();
                // Logic for getting the carName value
                carName = ((string)cmd.Parameters["@CarName"].Value);
             }
           
            return carName;
           
        }
        #endregion
    }
}
