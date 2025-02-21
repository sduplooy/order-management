using NetArchTest.Rules;
using Shouldly;

namespace OrderManagement.Api.UnitTests;

public class ArchitectureTests
{
    [Fact] 
    public void Infrastructure_types_should_be_static()
    {
         Types.InAssembly(typeof(IApiAssemblyMarker).Assembly)
             .That().ResideInNamespace("OrderManagement.Api.Infrastructure.Composition")
             .Should().BeStatic()
             .GetResult()
             .IsSuccessful.ShouldBeTrue();
    }
    
    [Fact]
    public void Domain_objects_should_not_be_public()
    {

    }
}