using System.Web.Http;

namespace CakeBuildDemo.ApiApp.Controllers
{
    public class ValuesController : ApiController
    {

        public IHttpActionResult Get()
        {
            return Ok(new [] { "value1", "value2" });
        }

        public IHttpActionResult Get(int id)
        {
            return Ok("value");
        }
    }
}
