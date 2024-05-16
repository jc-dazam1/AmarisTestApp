using AmarisTestApp.Controllers;
using AmarisTestApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace AmarisTestApp.BusinessLogic
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeBusinessLogic : ControllerBase
    {
        private readonly EmployeeController _employeeController;

        public EmployeeBusinessLogic(EmployeeController employeeController)
        {
            _employeeController = employeeController;
        }

        /// <summary>
        /// Get the Salary of an Employee 
        /// </summary>
        /// <param name="idEmployee"></param>
        /// <returns></returns>
        [HttpGet("{id}/annual-salary")]
        public async Task<int> CalculateAnnualSalary(int id)
        {
            var employeeResponse = await _employeeController.GetEmployee(id);

            if (employeeResponse.Value != null)
            {
                var employee = employeeResponse.Value as Employee;
                return employee.Employee_salary * 12;
            }
            else
            {
                // Handle error response
                throw new Exception($"Failed to retrieve employee {id}. Status code: {employeeResponse.Result}");
            }
        }
    }
}
