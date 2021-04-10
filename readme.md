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
- [Step 4 (Add Unit and Integration tests)](#step-4-add-unit-and-integration-tests)
  - [Integration Tests](#integration-tests)
  - [Unit Tests](#unit-tests)
- [Step 5 (Add Journaled Grain)](#step-5-add-journaled-grain)
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

## Step 4 (Add Unit and Integration tests)

Added unit and integration tests.

### Integration Tests

Integration tests added for both [Orleans](https://dotnet.github.io/orleans/docs/tutorials_and_samples/testing.html) and [API](https://docs.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-5.0)

### Unit Tests
Unit tests added for Grains. Used [OrleansTestKit](https://github.com/OrleansContrib/OrleansTestKit) for adding Orleans unit tests.


## Step 5 (Add Journaled Grain)

Added one grain that uses EventSourcing (JournaledGrain in orleans). Added relevant integration tests. Looks like Unit Tests for Journaled Grains are not supported. Every thing is using in memory storage for now.

JournaledGrains on receiving a message raise an event. Grain can decide not to raise event (e.g. if message fails validation). Events are then handled in State and result in state change. In our example Ping raises a PingEvent which results in few changes in State. If Ping received is older then last received ping then it is ignored and no event is raised. 

One of the changes PingEvent does is to change Device state to Online. There is no Event to change Device state back to Offline. [Orleans Reminder](https://dotnet.github.io/orleans/docs/grains/timers_and_reminders.html) looks like a good way to handle this.



Few other minor changes (e.g. renamed OrleansController to HelloController)


## References

- [AspNetCore Cohosting](https://github.com/dotnet/orleans/tree/main/Samples/3.0/AspNetCoreCohosting)
- [What is Microsoft Orleans - Code With Stu](https://www.youtube.com/watch?v=yM-gpuw1uhM)
- [Grain Persistence](https://dotnet.github.io/orleans/docs/grains/grain_persistence/index.html)
- [Testing Orleans](https://dotnet.github.io/orleans/docs/tutorials_and_samples/testing.html)
- [Testing Example](https://github.com/dotnet/orleans/blob/main/Samples/2.3/UnitTesting/test/Grains.Tests/Hosted/Cluster/ClusterFixture.cs)
- [Ignite Demo by Shengjie Yan](https://github.com/sheng-jie/Ignite2019.IoT.Orleans)