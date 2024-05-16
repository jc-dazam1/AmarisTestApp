namespace AmarisTestApp.Models
{
    public class EmployeeApiResponse
    {
        public string Status { get; set; }
        public List<Employee> Data { get; set; } // Cambiamos el tipo de Data a List<Employee>
        public string Message { get; set; }
    }
}
