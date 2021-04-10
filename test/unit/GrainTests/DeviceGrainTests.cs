using GrainTests;
using Orleans.TestKit;
using System;
using System.Threading.Tasks;
using Xunit;
using Shouldly;
using Orleans.Runtime;
using Moq;
using System.Reflection;
using Grains;

namespace GrainTests
{
    public class DeviceGrainTests : TestKitBase
    {
        /** Looks like OrleansTestKit does not support JournaledGrains
         * https://github.com/OrleansContrib/OrleansTestKit/issues/104
         */

    }
}
