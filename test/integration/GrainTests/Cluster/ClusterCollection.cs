using Xunit;

namespace Grains.IntegrationTests.Cluster
{
    [CollectionDefinition(Name)]
    public class ClusterCollection : ICollectionFixture<ClusterFixture>
    {
        public const string Name = "ClusterCollection";
    }
}
