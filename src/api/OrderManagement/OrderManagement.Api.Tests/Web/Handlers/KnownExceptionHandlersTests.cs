using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.Api.Application.Exceptions;
using OrderManagement.Api.Domain.Entities;
using OrderManagement.Api.Web.Handlers;
using Shouldly;

namespace OrderManagement.Api.UnitTests.Web.Handlers;

public class When_calling_try_handle_async_on_known_exceptions_handler
{
    private readonly KnownExceptionsHandler _handler = new();
    private readonly DefaultHttpContext _httpContext = new();

    [Fact]
    public async Task It_should_handle_validation_exception()
    {
        var exception = new ValidationException(new List<FluentValidation.Results.ValidationFailure>
        {
            new("Property", "Error message")
        });

        var result = await _handler.TryHandleAsync(_httpContext, exception, CancellationToken.None);
        
        result.ShouldBeTrue();
        _httpContext.Response.StatusCode.ShouldBe(StatusCodes.Status400BadRequest);
    }

    [Fact]
    public async Task It_should_handle_entity_not_found_exception()
    {
        var exception = new EntityNotFoundException(typeof(Product), Guid.NewGuid());

        var result = await _handler.TryHandleAsync(_httpContext, exception, CancellationToken.None);

        result.ShouldBeTrue();
        _httpContext.Response.StatusCode.ShouldBe(StatusCodes.Status404NotFound);
    }

    [Fact]
    public async Task It_should_return_false_for_unknown_exception()
    {
        var exception = new Exception("Unknown exception");

        var result = await _handler.TryHandleAsync(_httpContext, exception, CancellationToken.None);

        result.ShouldBeFalse();
    }
}