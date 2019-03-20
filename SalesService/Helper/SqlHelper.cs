using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Microsoft.Win32;
using SalesService.Model;
using SalesService.Model.DomainModel;

namespace SalesService.Helper
{
    public class SqlHelper
    {
        private static string SERVER_TAG = "SERVER";
        private static string REG_PROVIDER_NAME = ";PROVIDER=";
        private static string REG_PROVIDER_VALUE = "SqlXmlProvider";
        private static string REG_SERVER_NAME = ";SERVER=";
        private static string REG_SERVER_VALUE = "Server";
        private static string REG_USERNAME_NAME = ";UID=";
        private static string REG_USERNAME_VALUE = "UserName";
        private static string REG_PASSWORD_NAME = ";PWD=";
        private static string REG_PASSWORD_VALUE = "Password";
        private static string REG_DATABASE_NAME = ";Database=";
        private static readonly Hashtable ConnectionStrings = new Hashtable();

        static SqlHelper()
        {
            RegistryKey parentKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\BHHC\\DB");
            if (parentKey == null)
                return;
            foreach (string subKeyName in parentKey.GetSubKeyNames())
            {
                string connectionString = SqlHelper.CreateConnectionString(subKeyName, parentKey);
                if (connectionString.Length > 0)
                {
                    int startIndex = connectionString.IndexOf(SqlHelper.SERVER_TAG);
                    string str = connectionString;
                    if (startIndex > 0)
                        str = connectionString.Substring(startIndex);
                    SqlHelper.ConnectionStrings.Add((object)subKeyName, (object)str);
                }
            }
        }

        //Create the Connection String
        private static string CreateConnectionString(string dbName, RegistryKey parentKey)
        {
            RegistryKey registryKey = parentKey.OpenSubKey(dbName);
            if (registryKey == null)
                return string.Empty;
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(SqlHelper.REG_PROVIDER_NAME);
            stringBuilder.Append(registryKey.GetValue(SqlHelper.REG_PROVIDER_VALUE));
            stringBuilder.Append(SqlHelper.REG_SERVER_NAME);
            stringBuilder.Append(registryKey.GetValue(SqlHelper.REG_SERVER_VALUE));
            stringBuilder.Append(SqlHelper.REG_USERNAME_NAME);
            stringBuilder.Append(registryKey.GetValue(SqlHelper.REG_USERNAME_VALUE));
            stringBuilder.Append(SqlHelper.REG_PASSWORD_NAME);
            stringBuilder.Append(registryKey.GetValue(SqlHelper.REG_PASSWORD_VALUE));
            stringBuilder.Append(SqlHelper.REG_DATABASE_NAME);
            stringBuilder.Append(dbName.ToString());
            return stringBuilder.ToString();
        }

        //Get Sql Connection
        public static SqlConnection GetConnection(string dbName)
        {
            if (dbName == null || dbName.Length == 0)
                throw new Exception("dbName parameter cannot be NULL or empty.");
            string connectionString = string.Empty;
            try
            {
                if (SqlHelper.ConnectionStrings.Contains((object)dbName))
                {
                    connectionString = SqlHelper.ConnectionStrings[(object)dbName] as string;
                }               
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Could not open registry", ex);
            }
            var sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
            return sqlConnection;
        }
        //Get all the boats.
        public static Boats GetAllBoats(string dbName, string spName)
        {
            var sqlConnection=SqlHelper.GetConnection(dbName);
           
            try
            {
                SqlCommand cmd = new SqlCommand(spName, sqlConnection);
                cmd.CommandType = CommandType.StoredProcedure;
                var reader = cmd.ExecuteReader();
                //Add each boat to the boatsList
                var boatsList = new Boats();
                while (reader.Read())
                {
                    var boat = new Boat();
                    boat.Identifier = (int)reader.GetValue(reader.GetOrdinal("Boat_ID"));
                    boat.ModelYear = reader.GetValue(reader.GetOrdinal("ModelYear")).ToString();
                    boat.BuilderName = reader.GetValue(reader.GetOrdinal("BuilderName")).ToString();
                    boat.Model = reader.GetValue(reader.GetOrdinal("Model")).ToString();
                    boat.WatercraftType = reader.GetValue(reader.GetOrdinal("WatercraftType")).ToString();
                    boat.EngineType = reader.GetValue(reader.GetOrdinal("EngineType")).ToString();
                    boat.IsCustomized = (bool)reader.GetValue(reader.GetOrdinal("IsCustomized"));
                    boatsList.BoatCollection.Add(boat);
                }
                return boatsList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

                if (sqlConnection != null)
                {
                    sqlConnection.Close();
                }
            }
        }

        //Get a boat based on the boat id provided.
        public static Boat GetBoatbyId(string dbName, string spName, int sqlParam)
        {
            var sqlConnection = SqlHelper.GetConnection(dbName);

            try
            {
                SqlCommand cmd = new SqlCommand(spName, sqlConnection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@boatId", sqlParam));
                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    var boat = new Boat();
                    while (reader.Read())
                    {
                        boat.Identifier = (int)reader.GetValue(reader.GetOrdinal("Boat_ID"));
                        boat.ModelYear = reader.GetValue(reader.GetOrdinal("ModelYear")).ToString();
                        boat.BuilderName = reader.GetValue(reader.GetOrdinal("BuilderName")).ToString();
                        boat.Model = reader.GetValue(reader.GetOrdinal("Model")).ToString();
                        boat.WatercraftType = reader.GetValue(reader.GetOrdinal("WatercraftType")).ToString();
                        boat.EngineType = reader.GetValue(reader.GetOrdinal("EngineType")).ToString();
                        boat.IsCustomized = (bool) reader.GetValue(reader.GetOrdinal("IsCustomized"));
                    }

                    return boat;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

                if (sqlConnection != null)
                {
                    sqlConnection.Close();
                }
            }
        }

        //Create a boat based on the Boat object provided.
        public static Boat CreateBoat(string dbName, string spName, Boat boat)
        {
            var sqlConnection = SqlHelper.GetConnection(dbName);

            try
            {
                SqlCommand cmd = new SqlCommand(spName, sqlConnection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@ModelYear", boat.ModelYear));
                cmd.Parameters.Add(new SqlParameter("@BuilderName", boat.BuilderName));
                cmd.Parameters.Add(new SqlParameter("@Model", boat.Model));
                cmd.Parameters.Add(new SqlParameter("@WatercraftType", boat.WatercraftType));
                cmd.Parameters.Add(new SqlParameter("@EngineType", boat.EngineType));
                cmd.Parameters.Add(new SqlParameter("@IsCustomized", boat.IsCustomized));
                //Get the identifier of the boat added.
                boat.Identifier = (int)cmd.ExecuteScalar();
                return boat;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

                if (sqlConnection != null)
                {
                    sqlConnection.Close();
                }
            }
        }
     
        //Update a boat based on the Boat object provided.
        public static Boat UpdateBoat(string dbName, string spName, Boat boat)
        {
            var sqlConnection = SqlHelper.GetConnection(dbName);

            try
            {
                SqlCommand cmd = new SqlCommand(spName, sqlConnection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@Boat_ID", boat.Identifier));
                cmd.Parameters.Add(new SqlParameter("@ModelYear", boat.ModelYear));
                cmd.Parameters.Add(new SqlParameter("@BuilderName", boat.BuilderName));
                cmd.Parameters.Add(new SqlParameter("@Model", boat.Model));
                cmd.Parameters.Add(new SqlParameter("@WatercraftType", boat.WatercraftType));
                cmd.Parameters.Add(new SqlParameter("@EngineType", boat.EngineType));
                cmd.Parameters.Add(new SqlParameter("@IsCustomized", boat.IsCustomized));
                cmd.ExecuteReader();       
                return boat;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

                if (sqlConnection != null)
                {
                    sqlConnection.Close();
                }
            }
        }

        //Delete a boat based on the id provided
        public static bool DeleteBoatbyId(string dbName, string spName, int sqlParam)
        {
            var sqlConnection = SqlHelper.GetConnection(dbName);

            try
            {
                SqlCommand cmd = new SqlCommand(spName, sqlConnection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@boatId", sqlParam));
                cmd.ExecuteReader();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

                if (sqlConnection != null)
                {
                    sqlConnection.Close();
                }
            }
        }
        
    }
}
