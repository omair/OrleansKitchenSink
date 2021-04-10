using Orleans.Hosting;
using Orleans.TestingHost;
using System;

namespace GrainTests
{
    public class ClusterFixture : IDisposable
    {

        public ClusterFixture()
        {
            var builder = new TestClusterBuilder(2);
            builder.Options.ServiceId = Guid.NewGuid().ToString();
            builder.AddSiloBuilderConfigurator<SiloBuilderConfigurator>();
            Cluster = builder.Build();
            Cluster.Deploy();
        }

        private class SiloBuilderConfigurator : ISiloConfigurator
        {
            public void Configure(ISiloBuilder siloBuilder)
            {
                siloBuilder
                    //.AddMemoryGrainStorageAsDefault()
                    .AddMemoryGrainStorage(name: "HelloGrainStorage")
                    .AddMemoryGrainStorage(name: "DeviceGrainStorage")
                    .AddLogStorageBasedLogConsistencyProvider("LogStorage");

            }
        }


        public void Dispose()
        {
            Cluster.StopAllSilos();
        }

        public TestCluster Cluster { get; private set; }
    }


}
