namespace AmarisTestApp.DataAccess
{
    using AmarisTestApp.Models;
    using AmarisTestApp.Utilities;
    using Newtonsoft.Json.Linq;
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Text.Json;
    using System.Threading.Tasks;

    public class EmployeeApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpClientWrapper _httpClientWrapper;

        public EmployeeApiClient()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("http://dummy.restapiexample.com/api/v1/");
        }

        public EmployeeApiClient(IHttpClientWrapper httpClientWrapper)
        {
            _httpClientWrapper = httpClientWrapper ?? throw new ArgumentNullException(nameof(httpClientWrapper));
        }

        /// <summary>
        /// Method to get all employees 
        /// </summary>
        /// <returns></returns>
        public async Task<List<Employee>> GetAllEmployeesAsync()
        {

            HttpResponseMessage response = await _httpClient.GetAsync("employees");
            try
            {
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    // Parse JSON
                    JObject jsonObject = JObject.Parse(content);

                    // Accede a la propiedad 'data'
                    JArray data = (JArray)jsonObject["data"];
                    List<Employee> resultList = new List<Employee>();
                    foreach (JObject employeeObject in data)
                    {
                        Employee employee = new Employee();
                        employee.Id = (int)employeeObject["id"];
                        employee.Employee_name = (string)employeeObject["employee_name"];
                        employee.Employee_salary = (int)employeeObject["employee_salary"];
                        employee.Employee_age = (int)employeeObject["employee_age"];
                        employee.Profile_image = (string)employeeObject["profile_image"];
                        resultList.Add(employee);

                    }

                    return resultList;
                }
                else if (response.StatusCode == HttpStatusCode.TooManyRequests)
                {
                    // Manejar el error de demasiadas solicitudes aquí
                    await Task.Delay(5000); // Esperar 5 segundos antes de volver a intentar
                    return await GetAllEmployeesAsync(); // Volver a intentar la solicitud
                }
                else
                {
                    // Handle error response
                    throw new HttpRequestException($"Failed to retrieve employees. Status code: {response.StatusCode}");
                }
            }
            finally
            {
                response.Dispose(); // Cierra la conexión HTTP
            }
            
        }

        /// <summary>
        /// Method to get employee by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Employee>? GetEmployeeByIdAsync(int id)
        {
            HttpResponseMessage response;
            if (_httpClientWrapper != null)
            {
                response = await _httpClientWrapper.GetAsync($"employee/{id}");
            }
            else
            {
                response = await _httpClient.GetAsync($"employee/{id}");
            }
            Employee? employee = new Employee(); // Creas una nueva instancia de Employee
            try
            {
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    // Parse JSON
                    JObject jsonObject = JObject.Parse(content);
                    JToken data = jsonObject["data"];
                    if (data != null)
                    {
                        // Accedes directamente al objeto "employee" dentro de "data"
                        string employeeName = (string)data["employee_name"];
                        int employeeSalary = (int)data["employee_salary"];
                        int employeeAge = (int)data["employee_age"];
                        string profileImage = (string)data["profile_image"];

                        // Utilizas los valores para inicializar tu objeto 'employee'
                        employee.Id = id;
                        employee.Employee_name = employeeName;
                        employee.Employee_salary = employeeSalary;
                        employee.Employee_age = employeeAge;
                        employee.Profile_image = profileImage;
                    }
                    return employee;
                }
                else if (response.StatusCode == HttpStatusCode.TooManyRequests)
                {
                    // Manejar el error de demasiadas solicitudes aquí
                    await Task.Delay(5000); // Esperar 5 segundos antes de volver a intentar
                    return await GetEmployeeByIdAsync(id); // Volver a intentar la solicitud
                }
                else
                {
                    // Handle error response
                    throw new HttpRequestException($"Failed to retrieve employee {id}. Status code: {response.StatusCode}");
                }
            }
            finally
            {
                response.Dispose(); // Cierra la conexión HTTP
            }
            
        }
    }


}
