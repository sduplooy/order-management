using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Moq;
using OrderManagement.Api.Application.Behaviours;
using Shouldly;

namespace OrderManagement.Api.UnitTests.Application.Behaviours;

public class When_calling_handle_on_validation_behaviour
{
    [Fact]
    public void It_should_call_the_next_middleware_when_there_are_no_validators_registered()
    {
        var next = new Mock<RequestHandlerDelegate<string>>();
        next.Setup(n => n.Invoke()).ReturnsAsync("result").Verifiable();

        var behaviour = new ValidationBehaviour<string, string>(new List<IValidator<string>>());
        _ = behaviour.Handle("request", next.Object, CancellationToken.None);

        next.Verify();
    }
    
    [Fact]
    public async Task It_should_throw_a_validation_exception_when_a_validator_detects_a_validation_issue()
    {
        var next = new Mock<RequestHandlerDelegate<string>>();
        
        var validator = new Mock<IValidator<string>>();
        validator
            .Setup(v => v
            .ValidateAsync(It.IsAny<ValidationContext<string>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult(new List<ValidationFailure> { new("property", "error") }));
        
        var behaviour = new ValidationBehaviour<string, string>(new List<IValidator<string>> { validator.Object });
        await behaviour.Handle("request", next.Object, CancellationToken.None).ShouldThrowAsync<Api.Application.Exceptions.ValidationException>();
        
        next.VerifyNoOtherCalls();
    }
    
    [Fact]
    public void It_should_call_the_next_middleware_when_there_are_no_validation_failures()
    {
        var next = new Mock<RequestHandlerDelegate<string>>();
        next.Setup(n => n.Invoke()).ReturnsAsync("result").Verifiable();
        
        var validator = new Mock<IValidator<string>>();
        validator
            .Setup(v => v.ValidateAsync(It.IsAny<ValidationContext<string>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult())
            .Verifiable();

        var behaviour = new ValidationBehaviour<string, string>(new List<IValidator<string>> { validator.Object });
        _ = behaviour.Handle("request", next.Object, CancellationToken.None);

        validator.Verify();
        next.Verify();
    }
}