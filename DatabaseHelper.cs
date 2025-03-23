using Microsoft.ReportingServices.Diagnostics.Internal;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace MidProjectDb
{
    public class DatabaseHelper
    {
        
        private String serverName = "127.0.0.1";
        private String port = "3306";
        private String databaseName = "midprojectdb";
        private String databaseUser = "root";
        private String databasePassword = "1234567890-=";

        private static DatabaseHelper _instance;
        public static DatabaseHelper Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new DatabaseHelper();
                return _instance;
            }
        }
        private DatabaseHelper() { }

        public MySqlConnection GetConnection()
        {
            string connectionString = $"server={serverName};port={port};user={databaseUser};database={databaseName};password={databasePassword};SslMode=None;";
            return new MySqlConnection(connectionString);
        }


        public bool ExecuteParameterizedQuery(string query, object[] parameters)
        {
            try
            {
                using (var connection = GetConnection())
                {
                    connection.Open();
                    using (var command = new MySqlCommand(query, connection))
                    {
                        string[] paramNames = { "@year", "@theme", "@desc", "@id" };
                        for (int i = 0; i < parameters.Length; i++)
                        {
                            command.Parameters.AddWithValue(paramNames[i], parameters[i]);
                        }
                        command.ExecuteNonQuery();
                    }
                }
                return true; 
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error executing query: " + ex.Message);
                return false; 
            }
        }

        public DataTable GetDataTable(string query)
        {
            DataTable dt = new DataTable();

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
            }

            return dt;
        }

        public object ExecuteScalar(string query)
        {
            object result = null;
            try
            {
                using (var connection = GetConnection())
                {
                    connection.Open();
                    using (var command = new MySqlCommand(query, connection))
                    {
                        result = command.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error executing scalar query: " + ex.Message);
            }
            return result;
        }
       public DataTable ExecuteQuery(string query, MySqlParameter[] parameters = null)
       {
            DataTable dataTable = new DataTable();
            try
            {
                using (var connection = GetConnection())  
                {
                    connection.Open();
                    using (MySqlCommand cmd = new MySqlCommand(query, connection)) 
                    {
                        if (parameters != null)
                        {
                            cmd.Parameters.AddRange(parameters);
                        }

                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                        {
                            adapter.Fill(dataTable);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error executing query: " + ex.Message);
            }
            return dataTable;
       }

        public bool ExecuteNonQuery(string query, MySqlParameter[] parameters)
        {
            try
            {
                using (var connection = GetConnection())
                {
                    connection.Open();
                    using (var command = new MySqlCommand(query, connection))
                    {
                        if (parameters != null)
                        {
                            command.Parameters.AddRange(parameters);
                        }
                        command.ExecuteNonQuery();
                    }
                }
                return true; 
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error executing non-query: " + ex.Message);
                return false; 
            }
        }
        public DataTable GetData(string query)
        {
            DataTable dt = new DataTable();
            try
            {
                using (var connection = GetConnection())
                {
                    connection.Open();
                    using (var command = new MySqlCommand(query, connection))
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                    {
                        adapter.Fill(dt);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching data: " + ex.Message);
            }
            return dt;
        }
        public bool ExecuteQueryy(string query)
        {
            using (var connection = GetConnection()) 
            {
                 connection.Open();
                 using (var cmd = new MySqlCommand(query, connection)) 
                 {
                     return cmd.ExecuteNonQuery() > 0; 
                 }
            }
        }
        public bool ExecuteNonQueryy(string query, MySqlParameter[] parameters = null)
        {
            try
            {
                using (var connection = GetConnection())
                {
                    connection.Open();
                    using (var command = new MySqlCommand(query, connection))
                    {
                        if (parameters != null)
                        {
                            command.Parameters.AddRange(parameters);
                        }
                        command.ExecuteNonQuery();
                    }
                }
                return true; 
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error executing non-query: " + ex.Message);
                return false; 
            }
        }
        public void ClearFields(params Control[] controls)
        {
            foreach (var control in controls)
            {
                if (control is TextBox)
                    ((TextBox)control).Text = "";  

                else if (control is ComboBox)
                    ((ComboBox)control).SelectedIndex = -1; 

                else if (control is RichTextBox)
                    ((RichTextBox)control).Text = "";  
            }
        }
        public DataTable GetDataTablee(string query, MySqlParameter[] parameters)
        {
            DataTable dt = new DataTable();

            using (MySqlConnection conn = GetConnection())
            {
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
            }

            return dt;
        }
        public object ExecuteScalar(string query, MySqlParameter[] parameters = null)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand(query, conn))
                {
                    if (parameters != null)
                        cmd.Parameters.AddRange(parameters);

                    return cmd.ExecuteScalar();
                }
            }
        }

    }
}