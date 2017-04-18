using System.Collections.Generic;
using CakeBuildDemo.ApiApp.Controllers;
using MyTested.WebApi;
using MyTested.WebApi.Builders.Contracts.Controllers;
using Xunit;

namespace CakeBuildDemo.UnitTests
{
    public class ValuesTests
    {
        private readonly IControllerBuilder<ValuesController> _controller;

        public ValuesTests()
        {
            _controller = MyWebApi.Controller<ValuesController>();
        }

        [Fact]
        public void TestGetAllMethod()
        {
            _controller
                .Calling(controller => controller.Get())
                .ShouldReturn()
                .Ok()
                .WithResponseModelOfType<IEnumerable<string>>()
                .Passing(response =>
                    {
                        Assert.NotEmpty(response);
                    }
                );
        }

        [Fact]
        public void TestGetByIdMethod()
        {
            _controller
                .Calling(controller => controller.Get(42)) // demo purpose
                .ShouldReturn()
                .Ok()
                .WithResponseModelOfType<string>()
                .Passing(response =>
                    {
                        Assert.Equal("value", response);
                    }
                );
        }
    }
}
