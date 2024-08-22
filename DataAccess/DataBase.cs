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
        public List<Dictionary<string, object>> GetAllData(string tableName) {
            var queryString = $"SELECT * FROM {tableName}";
            return Read(queryString);
        }
        public List<Dictionary<string, object>> GetBy(string tableName,string columnName,object valueToSearch) {
            var queryString = $"SELECT * FROM {tableName} WHERE {tableName}.{columnName} = '{valueToSearch}'";
            return Read(queryString);
        }
        public Dictionary<string,object> GetById(string tableName, object id)
        {
            var queryString = $"SELECT * FROM {tableName} WHERE {tableName}.Id = '{id}'";
            return Read(queryString).FirstOrDefault()??new Dictionary<string, object>();
        }

        private List<Dictionary<string, object>> Read(string queryString)
        {
            var res = new List<Dictionary<string, object>>();

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
        public bool WriteOne(Dictionary<string, object> input, string tableName)
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
        public void WriteAll(List<Dictionary<string,object>> input,string tableName)
        {
            if (input.Count() > 0)
            {
                string queryString = $"INSERT INTO {tableName} (";
                int i = 0;
                foreach (var item in input.First())
                {
                    if (i == 0) { queryString += item.Key; i++; }
                    else queryString += "," + item.Key;
                }
                queryString += ") VALUES ";
                var ii = 0;
                foreach (var item in input) {
                    if (ii == 0) 
                    {
                        i = 0;
                        queryString += "(";

                        foreach (var innerItem in item)
                        {
                            if (i == 0) { queryString += $"'{innerItem.Value}'"; i++; }
                            else queryString += $",'{innerItem.Value}'";
                        }
                        queryString += ")";
                        ii++;
                    }
                    else
                    {
                        queryString += ",";
                        queryString += "(";

                        i = 0;
                        foreach (var innerItem in item)
                        {
                            if (i == 0) { queryString += $"'{innerItem.Value}'"; i++; }
                            else queryString += $",'{innerItem.Value}'";
                        }
                        queryString += ")";
                    }

                  
                }
                using (SqlConnection connection =new(_connectionString))
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


            }
        }



    }
}
