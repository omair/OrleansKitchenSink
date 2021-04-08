- [Orleans Kitchen Sink](#orleans-kitchen-sink)
- [Step 0 (Added Empty Solution)](#step-0-added-empty-solution)
- [Step 1 (Hello World)](#step-1-hello-world)
  - [Add Api Project](#add-api-project)
  - [Add Project for Grain Interfaces](#add-project-for-grain-interfaces)
  - [Add Project for Grain Implementations](#add-project-for-grain-implementations)
  - [Invoke Grain](#invoke-grain)
- [Step 2 (Add State)](#step-2-add-state)
  - [Add State to Grain](#add-state-to-grain)
  - [Update Silo and configure Storage.](#update-silo-and-configure-storage)
- [Step 3 (Upgrade to .Net 5)](#step-3-upgrade-to-net-5)
- [Step 4](#step-4)
  - [Integration Tests](#integration-tests)
  - [Unit Tests](#unit-tests)
- [References](#references)

## Orleans Kitchen Sink

Project to learn [Microsoft Orleans](https://dotnet.github.io/orleans/)

## Step 0 (Added Empty Solution)

Just getting started. Added empty solution and this readme.

## Step 1 (Hello World)


### Add Api Project

Add a new ASP.NET Core API Project.

Add following dependencies

- Microsoft.Orleans.Client
- Microsoft.Orleans.CodeGenerator.MSBuild
- Microsoft.Orleans.Server

Update Program.cs and setup orleans. For now we will use localhost clustering.

### Add Project for Grain Interfaces

Add a new project (.Net standard class library). Add Grain interface with Guid as key. We also need to add following dependencies

- Microsoft.Orleans.Core.Abstractions
- Microsoft.Orleans.CodeGenerator.MSBuild

### Add Project for Grain Implementations

Add a new .net standard project. Add a new class and implement interface we added in `GrainInterfaces` project. Also add following dependencies

- Microsoft.Orleans.CodeGenerator.MSBuild
- Microsoft.Orleans.Core

### Invoke Grain

Add reference to both `GrainInterfaces` and `Grains` project in `API` project.

Update controller to get reference to grain and call `SayHello` method. Had to update order of how orleans was being configured in `Program.cs`. Configuring Orleans after `ConfigureWebHostDefaults` was throwing `InvalidSchedulingContextException`


## Step 2 (Add State)

For adding state to a grain we need to add a new **State** class. We will then add the state to Grain. To add state we have two options

- Extend `Grain\<TState\>` class
- Inject `IPersistentState\<TState\>` via constructor with `[PersistentState("stateName", "providerName")]` attribute. Using `IPersistentState` is preferred so we will use this method. Using `IPersistentState` we can inject multiple state objects. E.g. for a consumer profile we cab inject one for Profile and one for Cart.

### Add State to Grain

Update Grain class to inject `[PersistentState("greetingStore","HelloGrainStorage")] IPersistentState<GreetingState> greetings` and then update SayHello method to use the state. We also need to update the interface to update signature of existing `SayHello` method and add a new method to get history.

### Update Silo and configure Storage.

Update `Program.cs` and configure storage provider. To begin with we will use in-memory storage.


## Step 3 (Upgrade to .Net 5)

Upgrade to .Net 5 and use [Serilog](https://serilog.net/) for logging.

## Step 4

Added unit and integration tests.

### Integration Tests

Integration tests added for both [Orleans](https://dotnet.github.io/orleans/docs/tutorials_and_samples/testing.html) and [API](https://docs.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-5.0)

### Unit Tests
Unit tests added for Grains. Used [OrleansTestKit](https://github.com/OrleansContrib/OrleansTestKit) for adding Orleans unit tests.

## References

- [AspNetCore Cohosting](https://github.com/dotnet/orleans/tree/main/Samples/3.0/AspNetCoreCohosting)
- [What is Microsoft Orleans - Code With Stu](https://www.youtube.com/watch?v=yM-gpuw1uhM)
- [Grain Persistence](https://dotnet.github.io/orleans/docs/grains/grain_persistence/index.html)
- [Testing Orleans](https://dotnet.github.io/orleans/docs/tutorials_and_samples/testing.html)
- [Testing Example](https://github.com/dotnet/orleans/blob/main/Samples/2.3/UnitTesting/test/Grains.Tests/Hosted/Cluster/ClusterFixture.cs)