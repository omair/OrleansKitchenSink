using GrainInterfaces;
using Microsoft.AspNetCore.Mvc;
using Orleans;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrleansController : ControllerBase
    {
        private readonly IGrainFactory _grainFactory;

        public OrleansController(IGrainFactory grainFactory)
        {
            _grainFactory = grainFactory;
        }

        [HttpGet]
        public async Task<List<string>> Get() {
            var grainId = Guid.Parse("691c5425-a6b2-4937-81de-2162233e78db");
            var client = _grainFactory.GetGrain<IHelloGrain>(grainId);
            _ = await client.SayHello($"{Guid.NewGuid()}");
            return await client.GetAllGreetings();
        }
    }
}
