using GrainInterfaces;
using System.Threading.Tasks;

namespace Grains
{
    public class HelloGrain : Orleans.Grain, IHelloGrain
    {
        public Task<string> SayHello() => Task.FromResult("Hello, World");
    }
}
