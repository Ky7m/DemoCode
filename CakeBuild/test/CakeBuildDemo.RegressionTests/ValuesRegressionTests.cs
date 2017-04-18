using System.Configuration;
using System.Net.Http;
using MyTested.HttpServer;
using Newtonsoft.Json;
using Xunit;

namespace CakeBuildDemo.RegressionTests
{
    public class ValuesRegressionTests
    {
        private readonly string _baseAddress = ConfigurationManager.AppSettings["ApiEndpointBaseAddress"];

        [Fact]
        public void TestGetAllMethod()
        {
            MyHttpServer
                .WorkingRemotely(_baseAddress)
               .WithHttpRequestMessage(req => req
                    .WithMethod(HttpMethod.Get)
                    .WithRequestUri("/api/values"))
                .ShouldReturnHttpResponseMessage()
                .WithSuccessStatusCode()
                .AndAlso()
                .WithContent(content =>
                {
                    var response = JsonConvert.DeserializeObject<string[]>(content);
                    Assert.NotEmpty(response);
                });

        }
        [Fact]
        public void TestGetByIdMethod()
        {
            MyHttpServer
                .WorkingRemotely(_baseAddress)
                .WithHttpRequestMessage(req => req
                    .WithMethod(HttpMethod.Get)
                    .WithRequestUri("/api/values/42")) // demo purpose
                .ShouldReturnHttpResponseMessage()
                .WithSuccessStatusCode()
                .AndAlso()
                .WithContent(content =>
                {
                    var response = JsonConvert.DeserializeObject<string>(content);
                    Assert.Equal("value", response);
                });
        }
    }
}
