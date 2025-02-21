using NetArchTest.Rules;

namespace OrderManagement.Api.UnitTests;

public class ArchitectureTests
{
    [Fact] public void Infrastructure_types_should_be_static()
    {
        var types = Types.InAssembly(typeof(IApiAssemblyMarker).Assembly);
        types.That().ResideInNamespace("OrderManagement.Api.Infrastructure").Should().BeStatic();
    }
}