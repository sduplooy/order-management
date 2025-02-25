using FluentValidation;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Api.Infrastructure.Database;

namespace OrderManagement.Api.Application.Products.UpdateProduct;

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    private readonly OrderManagementDbContext _context;

    public UpdateProductCommandValidator(OrderManagementDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));

        RuleFor(v => v.Name)
            .NotEmpty()
            .MaximumLength(200)
            .MustAsync(BeUnique)
            .Unless((command, validationContext) => command.Id == validationContext.InstanceToValidate.Id)
            .WithMessage("{PropertyName} must be unique.")
            .WithErrorCode("ProductNameNotUnique");

        RuleFor(v => v.Price)
            .GreaterThanOrEqualTo(0)
            .WithMessage("{PropertyName} must be greater than or equal to 0.")
            .WithErrorCode("PriceLessThanZero");
    }

    private async Task<bool> BeUnique(string name,  CancellationToken cancellationToken)
    {
        return !await _context.Products.AnyAsync(l => l.Name == name, cancellationToken);
    }
}