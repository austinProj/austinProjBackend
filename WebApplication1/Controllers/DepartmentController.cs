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
        public readonly IConfiguration _config;

        //This is the contructor that gets the sql server connection string in the appsettings.json
        public DepartmentController(IConfiguration config)
        {
            _config = config;
        }

        //Get all departments
        [HttpGet]
        public async Task<ActionResult<List<Department>>> GetAllDepartments()
        {
            //Opens a new sql connection
            using var connection = new SqlConnection(_config.GetConnectionString("EmployeeAppCon"));


            var departments = await connection.QueryAsync<Department>("select * from Department");

            return Ok(departments);
        }

        //Get a specific department based off id
        [HttpGet("{department_Id}")]
        public async Task<ActionResult<Department>> GetDepartment(int department_Id)
        {
            //Opens a new sql connection
            using var connection = new SqlConnection(_config.GetConnectionString("EmployeeAppCon"));


            var departments = await connection.QueryAsync<Department>("select * from Department where DepartmentId = @DepartmentId", 
                new { DepartmentId = department_Id });

            return Ok(departments);
        }


    }
}
