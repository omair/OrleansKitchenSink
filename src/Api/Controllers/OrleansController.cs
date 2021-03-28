using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrleansController : ControllerBase
    {

        [HttpGet]
        public string Get() => "Hello, Workd";
    }
}
