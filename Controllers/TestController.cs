using WebApplication2.Services;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.Models;
using System.Data.SqlClient;


namespace WebApplication2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        private readonly DatabaseHelper _dbHelper;

        public TestController(IConfiguration config)
        {
            string? connStr = config.GetConnectionString("WBHConnection");
            if (string.IsNullOrEmpty(connStr))
                throw new Exception("Connection string 'WBHConnection' is missing.");

            _dbHelper = new DatabaseHelper(connStr);
        }

        [HttpGet]
        public IActionResult Get()
        {
            var items = _dbHelper.GetTestItems();
            return Ok(items);
        }

        [HttpPost]
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

        [HttpDelete]
        public IActionResult Delete()
        {
            try
            {
                _dbHelper.deleteData();
                return Ok("Data deleted successfully.");
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPut]
        public IActionResult Update()
        {
            try
            {
                _dbHelper.updateData();
                return Ok("Data updated successfully.");
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}