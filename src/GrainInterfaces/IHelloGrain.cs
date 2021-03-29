using System.Threading.Tasks;

namespace GrainInterfaces
{
    public interface IHelloGrain : Orleans.IGrainWithGuidKey
    {
        Task<string> SayHello();
    }
}
