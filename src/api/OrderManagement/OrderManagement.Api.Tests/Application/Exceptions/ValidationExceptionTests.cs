using FluentValidation.Results;
using OrderManagement.Api.Application.Exceptions;
using Shouldly;

namespace OrderManagement.Api.UnitTests.Application.Exceptions;

public class When_constructing_a_validation_exception
{
    [Fact]
    public void It_should_create_an_empty_error_dictionary()
    {
        var errors = new ValidationException().Errors;
        errors.ShouldBeEmpty();
    }
}

public class When_constructing_a_validation_exception_with_failures
{

    [Fact]
    public void It_should_add_the_validation_failure()
    {
        var failures = new List<ValidationFailure>
        {
            new("Age", "must be over 18"),
        };

        var actual = new ValidationException(failures).Errors;

        actual.Keys.ShouldContain("Age");
        actual["Age"].ShouldBe(["must be over 18"]);
    }

    [Fact] public void It_should_add_multiple_validation_failures()
    {
        var failures = new List<ValidationFailure>
        {
            new("Age", "must be 18 or older"),
            new("Age", "must be 25 or younger"),
            new("Password", "must contain at least 8 characters"),
            new("Password", "must contain a digit"),
            new("Password", "must contain upper case letter"),
            new("Password", "must contain lower case letter"),
        };
    
        var actual = new ValidationException(failures).Errors;
    
        actual.Keys.ShouldBe(["Age", "Password"]);
    
        actual["Age"].ShouldBe([
            "must be 18 or older",
            "must be 25 or younger"
        ]);
    
        actual["Password"].ShouldBe([
            "must contain at least 8 characters",
            "must contain a digit",
            "must contain upper case letter",
            "must contain lower case letter"
        ]);
    }}