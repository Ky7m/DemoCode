using System.Web.Http;
using Swashbuckle.Application;
using WebActivatorEx;
using CakeBuildDemo.ApiApp;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace CakeBuildDemo.ApiApp
{
    public class SwaggerConfig
    {
        public static void Register()
        {
            GlobalConfiguration.Configuration
                .EnableSwagger(c =>
                    {
                        c.SingleApiVersion("v1", "CakeBuildDemo.ApiApp");
                    });
        }
    }
}