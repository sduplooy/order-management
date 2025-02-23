using NetArchTest.Rules;
using Shouldly;

namespace OrderManagement.Api.UnitTests;

public class ArchitectureTests
{
    [Fact] 
    public void Infrastructure_should_not_depend_on_web()
    {
        var result = Types.InAssembly(typeof(IApiAssemblyMarker).Assembly)
            .That()
            .ResideInNamespace("OrderManagement.Api.Infrastructure")
            .ShouldNot()
            .HaveDependencyOn("OrderManagement.Api.Web")
            .GetResult();

        result.IsSuccessful.ShouldBeTrue();
    }
    
    [Fact]
    public void Infrastructure_should_not_depend_on_application()
    {
        var result = Types.InAssembly(typeof(IApiAssemblyMarker).Assembly)
            .That()
            .ResideInNamespace("OrderManagement.Api.Infrastructure")
            .ShouldNot()
            .HaveDependencyOn("OrderManagement.Api.Application")
            .GetResult();

        result.IsSuccessful.ShouldBeTrue();
    }
    
    [Fact]
    public void Domain_objects_should_not_be_public()
    {

    }
}