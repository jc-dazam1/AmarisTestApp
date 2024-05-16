using AmarisTestApp.Models;

namespace AmarisTestApp.BusinessLogic
{
    public class EmployeeBusinessLogic
    {
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

        /// <summary>
        /// Get the Salary of an Employee 
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        public decimal CalculateAnnualSalary(Employee employee)
        {
            // Pendiente por modificar
            return CalculateAnnualSalary(employee.EmployeeSalary);
        }
    }
}
