using Microsoft.VisualBasic;
using System.Data.SqlClient;
using System.Text.Encodings.Web;
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
                var command = new SqlCommand("SELECT name, quantity, last_update FROM test", connection);
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    items.Add(new Test
                    {
                        Name = reader["name"].ToString(),
                        Quantity = reader["quantity"] == DBNull.Value ? 0 : Convert.ToInt32(reader["quantity"]),
                        last_update = reader["last_update"] == DBNull.Value
    ? DateTime.MinValue
    : Convert.ToDateTime(reader["last_update"])
                    });
                }
            }

            return items;
        }

        //public void GetTestItemsa()
        //{
        //    try
        //    {
        //        using (var connection = new SqlConnection(_connectionString))
        //        {
        //            connection.Open();
        //            var command = new SqlCommand("select * from test", connection);
        //            SqlDataAdapter adapter = new SqlDataAdapter(command);
        //            DataTable dataTable = new DataTable(adapter);
        //            adapter.Fill(dataTable);
        //            command.ExecuteNonQuery();
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        public void insertData()
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    var command = new SqlCommand("insert into test (name,quantity,last_update) values('manually',7,'" + DateTime.Now + "')", connection);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void deleteData()
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    var command = new SqlCommand("DELETE FROM test WHERE id = (SELECT MAX(id) FROM test);", connection);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void updateData()
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    var command = new SqlCommand("update test set name = 'updated',last_update='" + DateTime.Now + "' where id =(SELECT MAX(id) FROM test);", connection);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
