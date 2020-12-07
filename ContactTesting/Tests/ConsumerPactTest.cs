using ContactTesting.Consumer;
using ContactTesting.Mock;
using PactNet.Mocks.MockHttpService;
using PactNet.Mocks.MockHttpService.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ContactTesting.Tests
{
    public class ConsumerPactTest : IClassFixture<ConsumerPact>
    {

        private IMockProviderService _mockProviderService;
        private string _mockProviderServiceBaseUri;


        public ConsumerPactTest(ConsumerPact data)
        {
            _mockProviderService = data.MockProviderService;
            _mockProviderService.ClearInteractions();
            _mockProviderServiceBaseUri = data.MockProviderServiceBaseUri;
        }


        [Fact]
        public void GetEmployeesDetails_VerifyIfItReturns()
        {

            //Arrange
            _mockProviderService
                .Given("Employee Details for Id '2'")
                .UponReceiving("A GET request to retrieve the employee details")
                .With(new ProviderServiceRequest
                {

                    Method = HttpVerb.Get,
                    Path = "/employee/2",
                    Headers = new Dictionary<string, object>
                    {
                        { "Accept", "application/json" }
                    }


                })
                .WillRespondWith(new ProviderServiceResponse
                {
                    Status = 200,
                    Headers = new Dictionary<string, object>
                    {
                        { "Content-Type", "application/json; charset=utf-8" }
                    },
                    Body = new //NOTE: Note the case sensitivity here, the body will be serialised as per the casing defined
                    {
                        id = 2,
                        employeeName = "Prashanth",
                        email = "prashanth@gmail.com",
                        city = "chennai"
                    }
                });

            //Act 
            var consumer = new APIClient(_mockProviderServiceBaseUri);
            var result = consumer.GetEmployeeDetails("2");

            //Assert
            Assert.Equal("Prashanth", result.EmployeeName);

            _mockProviderService.VerifyInteractions();
        }

    }
}
