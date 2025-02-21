using FluentValidation;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Api.Infrastructure.Database;

namespace OrderManagement.Api.Application.Products.CreateProduct;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    private readonly OrderManagementDbContext _context;

    public CreateProductCommandValidator(OrderManagementDbContext context)
    {
        _context = context;

        RuleFor(v => v.Name)
            .NotEmpty()
            .MaximumLength(200)
            .MustAsync(BeUniqueName)
            .WithMessage("{PropertyName} must be unique.")
            .WithErrorCode("ProductNameNotUnique");

        RuleFor(v => v.Price)
            .GreaterThan(0)
            .WithMessage("{PropertyName} must be greater than 0.00")
            .WithErrorCode("PriceGreaterThanZero");
    }

    private async Task<bool> BeUniqueName(string title, CancellationToken cancellationToken)
    {
        return !await _context.Products.AnyAsync(l => l.Name == title, cancellationToken);
    }
}