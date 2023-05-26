using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using WebApplication1.Models;
using Microsoft.Extensions.Configuration;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly ILogger<DepartmentController> _logger;
        public readonly IConfiguration _config;

        //This is the contructor that gets the sql server connection string in the appsettings.json
        public DepartmentController(ILogger<DepartmentController> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        //Get all departments
        [HttpGet]
        public async Task<ActionResult<List<Department>>> GetAllDepartments()
        {
            _logger.LogInformation("Received GET request");

            try
            {
                //Opens a new sql connection
                using var connection = new SqlConnection(_config.GetConnectionString("EmployeeAppCon"));

                var departments = await connection.QueryAsync<Department>("select * from Department");

                return Ok(departments);
            }
            catch (Exception err)
            {
                _logger.LogError(err.Message);
                return StatusCode(500, err.Message);
            }
        }

        //Get a specific department based off id
        [HttpGet("{department_Id}")]
        public async Task<ActionResult<Department>> GetDepartment(int department_Id)
        {
            _logger.LogInformation("Received GET request");

            try
            {
                //Opens a new sql connection
                using var connection = new SqlConnection(_config.GetConnectionString("EmployeeAppCon"));

                var departments = await connection.QueryAsync<Department>("select * from Department where DepartmentId = @DepartmentId", 
                    new { DepartmentId = department_Id });

                return Ok(departments);
            }
            catch (Exception err)
            {
                _logger.LogError(err.Message);
                return StatusCode(500, err.Message);
            }

            
        }

        //Create a department
        [HttpPost]
        public async Task<ActionResult> CreateDepartment(Department department)
        {
            _logger.LogInformation("Received POST request");

            try
            {
                //Opens a new sql connection
                using var connection = new SqlConnection(_config.GetConnectionString("EmployeeAppCon"));

                await connection.ExecuteAsync("insert into Department (DepartmentName) values (@DepartmentName)", department);
            }
            catch (Exception err)
            {
                _logger.LogError(err.Message);
                return StatusCode(500, err.Message);
            }
            

            return Ok("Created Successfully!");
        }

        //Update a department
        [HttpPut]
        public async Task<ActionResult> UpdateDepartment(Department department)
        {
            _logger.LogInformation("Received PUT request");

            try
            {
                //Opens a new sql connection
                using var connection = new SqlConnection(_config.GetConnectionString("EmployeeAppCon"));

                await connection.ExecuteAsync("update Department set DepartmentName = @DepartmentName where DepartmentId = @DepartmentId", department);
            }
            catch (Exception err)
            {
                _logger.LogError(err.Message);
                return StatusCode(500, err.Message);
            }

            return Ok("Updated Successfully!");
        }

        //Delete a department
        [HttpDelete("{department_Id}")]
        public async Task<ActionResult> DeleteDepartment(int department_Id)
        {
            _logger.LogInformation("Received PUT request");

            try
            {
                //Opens a new sql connection
                using var connection = new SqlConnection(_config.GetConnectionString("EmployeeAppCon"));

                await connection.ExecuteAsync("delete from Department where DepartmentId = @DepartmentId", new {DepartmentId = department_Id });
            }
            catch(Exception err)
            {
                _logger.LogError(err.Message);
                return StatusCode(500, err.Message);
            }
            

            return Ok("Deleted Successfully!");
        }


    }
}
