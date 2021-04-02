using System.Collections.Generic;
using System.Threading.Tasks;

namespace GrainInterfaces
{
    public interface IHelloGrain : Orleans.IGrainWithGuidKey
    {
        Task<List<string>> GetAllGreetings();
        Task<string> SayHello(string from);
    }
}
