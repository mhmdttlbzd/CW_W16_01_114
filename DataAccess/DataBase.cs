using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;

namespace DataAccess
{
    internal class DataBase
    {
        private string _connectionString;

        public DataBase(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Dictionary<string, object>> Read(string tableName)
        {
            var res = new List<Dictionary<string, object>>();
            string queryString = $"SELECT * FROM {tableName} ";

            using (SqlConnection connection =
                new SqlConnection(_connectionString))
            {
                // Create the Command and Parameter objects.
                SqlCommand command = new SqlCommand(queryString, connection);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var i = 0;
                        var row = new Dictionary<string, object>();

                        for (i = 0; i < reader.FieldCount; i++)
                        {
                            row.Add(reader.GetName(i), reader[i]);

                        }
                        res.Add(row);
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }


            }
            return res;
        }
        public bool Write(Dictionary<string, object> input, string tableName)
        {

            string queryString = $"INSERT INTO {tableName} (";
            int i = 0;
            foreach (var item in input)
            {
                if (i == 0) { queryString += item.Key; i++; }
                else queryString += "," + item.Key;

            }
            queryString += ") VALUES (";
            i = 0;
            foreach (var item in input)
            {
                if (i == 0) { queryString += $"'{item.Value}'"; i++; }
                else queryString += $",'{item.Value}'";
            }
            queryString += ")";

            using (SqlConnection connection =
                new(_connectionString))
            {
                // Create the Command and Parameter objects.
                SqlCommand command = new(queryString, connection);

                try
                {
                    connection.Open();
                    var reader = command.ExecuteNonQuery();
                    connection.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }


            return true;
        }
    }
}
