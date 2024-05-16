using AmarisTestApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace AmarisTestApp.Controllers
{
    public interface IEmployeeController
    {
        Task<ActionResult<List<Employee>>> GetEmployees();
        Task<ActionResult<Employee>> GetEmployee(int id);

    }
}
