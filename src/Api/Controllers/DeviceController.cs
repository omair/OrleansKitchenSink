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
    public class DeviceController : ControllerBase
    {
        private readonly IGrainFactory _grainFactory;

        public DeviceController(IGrainFactory grainFactory)
        {
            _grainFactory = grainFactory;
        }

        [HttpGet("{deviceId}/ping")]
        public Task<long> Ping(string deviceId) {
            var client = _grainFactory.GetGrain<IDeviceGrain>(deviceId);
            return client.Ping(DateTime.UtcNow);
        }

        [HttpGet("{deviceId}/events")]
        public Task<IReadOnlyList<IDeviceEvent>> Events(string deviceId)
        {
            var client = _grainFactory.GetGrain<IDeviceGrain>(deviceId);
            return client.RetrieveConfirmedEvents();
        }


        [HttpGet("{deviceId}/status")]
        public Task<string> Status(string deviceId)
        {
            var client = _grainFactory.GetGrain<IDeviceGrain>(deviceId);
            return client.Status();
        }

    }
}
