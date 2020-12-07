using ContactTesting.Outputer;
using PactNet;
using PactNet.Infrastructure.Outputters;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace ContactTesting.Tests
{
    public class ProviderPactTest
    {
        private readonly ITestOutputHelper _output;

        public ProviderPactTest(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void TestProvider()
        {

            var config = new PactVerifierConfig
            {
                Outputters = new List<IOutput> //NOTE: We default to using a ConsoleOutput, however xUnit 2 does not capture the console output, so a custom outputter is required.
                {
                    new XUnitOutput(_output)
                },
                Verbose = true //Output verbose verification logs to the test output
            };

            new PactVerifier(config)
                .ServiceProvider("EmployeeList", "http://localhost:60880/api")
                .HonoursPactWith("Service_Consumer")
                .PactUri(@"C:\contractUdemy\pacts\service_consumer-employeelist.json")
                .Verify();
        }
    }
}
