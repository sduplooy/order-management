using Microsoft.AspNetCore.Http;
using Moq;
using OrderManagement.Api.Web.Handlers;
using Shouldly;

namespace OrderManagement.Api.UnitTests.Web.Handlers;

public class When_catching_an_unhandled_exception
{
    [Fact]
    public async Task It_should_generate_a_problem_details_response()
    {
        var exception = new Exception("Test exception");
    
        var problemDetailsService = new Mock<IProblemDetailsService>();
        problemDetailsService
            .Setup(s => s.TryWriteAsync(It.Is<ProblemDetailsContext>(c => Match(c))))
            .Verifiable();

        var handler = new GlobalExceptionHandler(problemDetailsService.Object);
        await handler.TryHandleAsync(new DefaultHttpContext(), exception, CancellationToken.None);
    
        problemDetailsService.Verify();
    }

    private static bool Match(ProblemDetailsContext context)
    {
        context.Exception.Message.ShouldBe("Test exception");
        context.ProblemDetails.Title.ShouldBe("An error occurred");
        context.ProblemDetails.Type.ShouldBe(nameof(Exception));
        context.ProblemDetails.Detail.ShouldBe("Test exception");
        context.ProblemDetails.Status.ShouldBe(StatusCodes.Status500InternalServerError);
        return true;
    }
}