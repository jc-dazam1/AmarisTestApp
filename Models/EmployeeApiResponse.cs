namespace AmarisTestApp.Models
{
    public class EmployeeApiResponse
    {
        public string Status { get; set; }
        public List<Employee> Data { get; set; }
        public string Message { get; set; }
    }
}
