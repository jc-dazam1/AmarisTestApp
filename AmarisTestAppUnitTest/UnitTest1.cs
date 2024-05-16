using NUnit.Framework;
using Moq;
using System.Threading.Tasks;
using AmarisTestApp.BusinessLogic;
using AmarisTestApp.Controllers;
using AmarisTestApp.Models;
using Microsoft.AspNetCore.Mvc;
using AmarisTestApp.DataAccess;
using System.Net;
using Xunit;
using AmarisTestApp.Utilities;

namespace AmarisTestAppUnitTest
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public async Task CalculateAnnualSalary_Returns_CorrectValue()
        {
            // Data
            int employeeId = 1;
            int expectedSalary = 60000; // Assuming the salary is $5000 per month

            // Mocking the IEmployeeController
            var employeeControllerMock = new Mock<IEmployeeController>();
            employeeControllerMock.Setup(controller => controller.GetEmployee(employeeId))
                .ReturnsAsync(new ActionResult<Employee>(new Employee { Id = employeeId, Employee_salary = 5000 }));

            var employeeBusinessLogic = new EmployeeBusinessLogic(employeeControllerMock.Object);

            // Act
            var result = await employeeBusinessLogic.CalculateAnnualSalary(employeeId);

            // Assert
            Assert.AreEqual(expectedSalary, result);
        }

        [Test]
        public async Task CalculateAnnualSalary_Returns_Zero_When_MonthlySalary_Is_Zero()
        {
            // Arrange
            int employeeId = 1;
            int expectedSalary = 0; // Expected annual salary when monthly salary is zero

            // Mocking the IEmployeeController
            var employeeControllerMock = new Mock<IEmployeeController>();
            employeeControllerMock.Setup(controller => controller.GetEmployee(employeeId))
                .ReturnsAsync(new ActionResult<Employee>(new Employee { Id = employeeId, Employee_salary = 0 }));

            var employeeBusinessLogic = new EmployeeBusinessLogic(employeeControllerMock.Object);

            // Act
            var result = await employeeBusinessLogic.CalculateAnnualSalary(employeeId);

            // Assert
            Assert.AreEqual(expectedSalary, result);
        }

        [Test]
        public async Task CalculateAnnualSalary_Returns_NonZeroValue_When_MonthlySalary_IsPositive()
        {
            // Data
            int employeeId = 1;
            int monthlySalary = 5000;

            // Expected annual salary when monthly salary is positive
            int expectedAnnualSalary = monthlySalary * 12;

            // Mocking the IEmployeeController
            var employeeControllerMock = new Mock<IEmployeeController>();
            employeeControllerMock.Setup(controller => controller.GetEmployee(employeeId))
                .ReturnsAsync(new ActionResult<Employee>(new Employee { Id = employeeId, Employee_salary = monthlySalary }));

            var employeeBusinessLogic = new EmployeeBusinessLogic(employeeControllerMock.Object);

 
            var result = await employeeBusinessLogic.CalculateAnnualSalary(employeeId);

            // Assert
            Assert.AreNotEqual(0, result); // Verifies that the calculated annual salary is not zero
            Assert.AreEqual(expectedAnnualSalary, result); // Verifies that the calculated annual salary is correct
        }

        [Test]
        public async Task GetEmployeeByIdAsync_Returns_Employee_When_ValidId()
        {
            // Arrange
            int employeeId = 1;
            var httpClientWrapperMock = new Mock<IHttpClientWrapper>();
            httpClientWrapperMock.Setup(client => client.GetAsync($"employee/{employeeId}"))
                                  .ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = new StringContent("{\"data\":{\"id\":1,\"employee_name\":\"Tiger Nixon\",\"employee_salary\":320800,\"employee_age\":61,\"profile_image\":\"\"}}") });

            var employeeApiClient = new EmployeeApiClient(httpClientWrapperMock.Object);

            // Act
            var result = await employeeApiClient.GetEmployeeByIdAsync(employeeId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
            Assert.AreEqual("Tiger Nixon", result.Employee_name);
            Assert.AreEqual(320800, result.Employee_salary);
            Assert.AreEqual(61, result.Employee_age);
            Assert.AreEqual("", result.Profile_image);
        }

        [Test]
        public void GetEmployeeByIdAsync_Throws_HttpRequestException_When_InvalidId()
        {
            // Arrange
            int invalidEmployeeId = -1;
            var httpClientWrapperMock = new Mock<IHttpClientWrapper>();
            httpClientWrapperMock.Setup(client => client.GetAsync($"employee/{invalidEmployeeId}"))
                                  .ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.NotFound });

            var employeeApiClient = new EmployeeApiClient(httpClientWrapperMock.Object);

            // Act & Assert
            Assert.ThrowsAsync<HttpRequestException>(async () => await employeeApiClient.GetEmployeeByIdAsync(invalidEmployeeId));
        }



        [Test]
        public async Task GetAllEmployeesAsync_Should_Return_Employees()
        {
            // Arrange
            var employeeList = new List<Employee>
            {
                new Employee { Id = 1, Employee_name = "John Doe", Employee_salary = 1000, Employee_age = 30, Profile_image = "" }
            };

            var employeeApiClientMock = new Mock<IEmployeeApiClient>();
            employeeApiClientMock
                .Setup(client => client.GetAllEmployeesAsync())
                .ReturnsAsync(employeeList);

            var employeeApiClient = employeeApiClientMock.Object;

            // Act
            var result = await employeeApiClient.GetAllEmployeesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(1, result[0].Id);
            Assert.AreEqual("John Doe", result[0].Employee_name);
        }


    }
}