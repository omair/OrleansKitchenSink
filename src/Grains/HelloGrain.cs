using GrainInterfaces;
using System.Threading.Tasks;
using Orleans.Runtime;
using System.Collections.Generic;

namespace Grains
{
    public class HelloGrain : Orleans.Grain, IHelloGrain
    {
        private readonly IPersistentState<GreetingState> _greetings;

        public HelloGrain([PersistentState("greetingStore","HelloGrainStorage")] IPersistentState<GreetingState> greetings)
        {
            _greetings = greetings;
        }

        public async Task<string> SayHello(string from) {
            var greeting = $"Got Hello, World from {from}";
            _greetings.State.Archive.Add(greeting);
            _greetings.State.Count = _greetings.State.Count+1;
            await _greetings.WriteStateAsync();
            return greeting;
        }

        public Task<List<string>> GetAllGreetings()
        {
            return Task.FromResult(_greetings.State.Archive);
        }


    }

    public class GreetingState
    {
        public List<string> Archive { get; } = new List<string>();
        public long Count { get; set;  } = 0;
    }
}
