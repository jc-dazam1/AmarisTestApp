namespace AmarisTestApp.DataAccess
{
    using AmarisTestApp.Models;
    using System;
    using System.Net.Http;
    using System.Text.Json;
    using System.Threading.Tasks;

    public class EmployeeApiClient
    {
        private readonly HttpClient _httpClient;

        public EmployeeApiClient()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("http://dummy.restapiexample.com/api/v1/");
        }

        /// <summary>
        /// Method to get all employees 
        /// </summary>
        /// <returns></returns>
        public async Task<List<Employee>> GetAllEmployeesAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("employees");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<EmployeeApiResponse>(content);
                List<Employee> resultList = [.. result?.Data];
                return resultList;
            }
            else
            {
                // Handle error response
                throw new HttpRequestException($"Failed to retrieve employees. Status code: {response.StatusCode}");
            }
        }

        /// <summary>
        /// Method to get employee by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Employee>? GetEmployeeByIdAsync(int id)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"employee/{id}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<EmployeeApiResponse>(content);

                Employee foundEmployee = result.Data.FirstOrDefault(e => e.Id == id);
                return foundEmployee;
            }
            else
            {
                // Handle error response
                throw new HttpRequestException($"Failed to retrieve employee {id}. Status code: {response.StatusCode}");
            }
        }
    }


}
