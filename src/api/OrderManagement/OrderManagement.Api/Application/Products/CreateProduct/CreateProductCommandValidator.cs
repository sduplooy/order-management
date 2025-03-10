using FluentValidation;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Api.Infrastructure.Database;

namespace OrderManagement.Api.Application.Products.CreateProduct;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    private readonly OrderManagementDbContext _context;

    public CreateProductCommandValidator(OrderManagementDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));

        RuleFor(v => v.Name)
            .NotEmpty()
            .MaximumLength(200)
            .MustAsync(BeUnique)
            .WithMessage("{PropertyName} must be unique.")
            .WithErrorCode("ProductNameNotUnique");

        RuleFor(v => v.Price)
            .GreaterThanOrEqualTo(0)
            .WithMessage("{PropertyName} must be greater than or equal to 0.")
            .WithErrorCode("PriceLessThanZero");
    }

    private async Task<bool> BeUnique(string name, CancellationToken cancellationToken)
    {
        return !await _context.Products.AnyAsync(l => l.Name == name, cancellationToken);
    }
}