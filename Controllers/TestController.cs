using Microsoft.AspNetCore.Mvc;
using WebApplication2.Models;
using WebApplication2.Services;
using Newtonsoft.Json;
using System.Data.SqlClient;

namespace WebApplication2.Controllers
{
	public class TestController : Controller
	{
		private readonly DatabaseHelper _dbHelper;
		private readonly IConfiguration _config;

		public TestController(IConfiguration config)
		{
			_config = config;
			string? connStr = _config.GetConnectionString("WBHConnection");
			if (string.IsNullOrEmpty(connStr))
				throw new Exception("Connection string 'WBHConnection' is missing.");

			_dbHelper = new DatabaseHelper(connStr);
		}

		// 🖥️ Render Razor HTML Table from SQL Records
		public async Task<IActionResult> TableView()
		{
			var items = await GetTestDataAsync();
			return View(items);
		}

		private async Task<List<Test>> GetTestDataAsync()
		{
			var items = new List<Test>();
			string connStr = _config.GetConnectionString("WBHConnection");

			using (var conn = new SqlConnection(connStr))
			{
				string query = "SELECT Name, Quantity, last_update, audit_user FROM test";
				using (var cmd = new SqlCommand(query, conn))
				{
					await conn.OpenAsync();
					using (var reader = await cmd.ExecuteReaderAsync())
					{
						while (await reader.ReadAsync())
						{
							items.Add(new Test
							{
								Name = reader["Name"]?.ToString(),
								Quantity = reader["Quantity"] as int?,
								last_update = reader["last_update"] as DateTime?,
								audit_user = reader["audit_user"]?.ToString()
							});
						}
					}
				}
			}

			return items;
		}

		// 🔁 Redirect to TableView
		public IActionResult Index()
		{
			return RedirectToAction("TableView");
		}

		// 📦 API: Get Items
		[HttpGet("api/get-items")]
		public IActionResult GetItems()
		{
			var items = _dbHelper.GetTestItems();
			return Ok(items);
		}

		// 🧾 API: Insert
		[HttpPost("api/insert-item")]
		public IActionResult Insert()
		{
			try
			{
				_dbHelper.insertData();
				return Ok("Data inserted successfully.");
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex.Message}");
			}
		}

		// 🗑️ API: Delete
		[HttpDelete("api/delete-item")]
		public IActionResult Delete()
		{
			try
			{
				_dbHelper.deleteData();
				return Ok("Data deleted successfully.");
			}
			catch
			{
				return StatusCode(500, "Failed to delete data.");
			}
		}

		// 🔄 API: Update
		[HttpPut("api/update-item")]
		public IActionResult Update()
		{
			try
			{
				_dbHelper.updateData();
				return Ok("Data updated successfully.");
			}
			catch
			{
				return StatusCode(500, "Failed to update data.");
			}
		}
	}
}