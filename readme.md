- [Orleans Kitchen Sink](#orleans-kitchen-sink)
- [Step 0 (Added Empty Solution)](#step-0-added-empty-solution)
- [Step 1 (Hello World)](#step-1-hello-world)
  - [Add Api Project](#add-api-project)
  - [Add Project for Grain Interfaces](#add-project-for-grain-interfaces)
  - [Add Project for Grain Implementations](#add-project-for-grain-implementations)
  - [Invoke Grain](#invoke-grain)
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

## References

- [AspNetCore Cohosting](https://github.com/dotnet/orleans/tree/main/Samples/3.0/AspNetCoreCohosting)
- [What is Microsoft Orleans - Code With Stu](https://www.youtube.com/watch?v=yM-gpuw1uhM)