using AmarisTestApp.Controllers;
using AmarisTestApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace AmarisTestApp.BusinessLogic
{
    public class EmployeeBusinessLogic
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
        public async Task<decimal> CalculateAnnualSalary(int idEmployee)
        {
            var employeeResponse = await _employeeController.GetEmployee(idEmployee);

            if (employeeResponse.Result is OkObjectResult)
            {
                var employee = (employeeResponse.Result as OkObjectResult).Value as Employee;
                return CalculateAnnualSalary(employee.EmployeeSalary);
            }
            else
            {
                // Handle error response
                throw new Exception($"Failed to retrieve employee {idEmployee}. Status code: {employeeResponse.Result}");
            }
        }

        /// <summary>
        /// Calculate the annual Salary
        /// </summary>
        /// <param name="monthlySalary"></param>
        /// <returns></returns>
        public decimal CalculateAnnualSalary(decimal monthlySalary)
        {
            // Calcula el salario anual multiplicando el salario mensual por 12
            return monthlySalary * 12;
        }
    }
}
