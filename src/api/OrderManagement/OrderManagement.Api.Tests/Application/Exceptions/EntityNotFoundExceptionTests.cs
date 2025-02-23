using OrderManagement.Api.Application.Exceptions;
using OrderManagement.Api.Domain.Entities;
using Shouldly;

namespace OrderManagement.Api.UnitTests.Application.Exceptions;

public class EntityNotFoundExceptionTests
{
    [Fact]
    public void It_should_set_the_error_message_based_on_type_and_id()
    {
        var id = Guid.NewGuid();
        var exception = new EntityNotFoundException(typeof(Product), id);

        exception.Message.ShouldBe($"Entity of type Product with id {id} was not found.");
    }
}