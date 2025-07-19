using System.Data.SqlClient;
using WebApplication2.Models;

namespace WebApplication2.Services
{
    public class DatabaseHelper
    {
        private readonly string _connectionString;

        public DatabaseHelper(string connectionString)
        {
            _connectionString = connectionString;
        }
        public List<Test> GetTestItems()
        {
            var items = new List<Test>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT name, quantity FROM test", connection);
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    items.Add(new Test
                    {
                        Name = reader["name"].ToString(),
                        Quantity = reader["quantity"] == DBNull.Value ? 0 : Convert.ToInt32(reader["quantity"])
                    });
                }
            }

            return items;
        }
    }
}
