using AmarisTestApp.Models;
using System.Threading.Tasks;

namespace AmarisTestApp.DataAccess
{
    public interface IEmployeeApiClient
    {
        Task<List<Employee>> GetAllEmployeesAsync();
        Task<Employee> GetEmployeeByIdAsync(int id);
    }
}
