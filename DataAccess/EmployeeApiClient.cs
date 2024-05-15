namespace AmarisTestApp.DataAccess
{
    using System;
    using System.Net.Http;
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
        public async Task<string> GetAllEmployeesAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("employees");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                return null; // Handle error
            }
        }

        /// <summary>
        /// Method to get employee by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<string> GetEmployeeByIdAsync(int id)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"employee/{id}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                return null; // Handle error
            }
        }
    }


}
