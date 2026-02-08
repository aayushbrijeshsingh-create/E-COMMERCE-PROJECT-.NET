using ECommerce.Models.DTOs;
using FluentValidation;

namespace ECommerce.Business.Logic.Validators;

public class ProductValidator : AbstractValidator<CreateProductDto>
{
    public ProductValidator()
    {
        RuleFor(p => p.Name)
            .NotEmpty().WithMessage("Product name is required")
            .MaximumLength(200).WithMessage("Product name cannot exceed 200 characters");
        
        RuleFor(p => p.Price)
            .GreaterThan(0).WithMessage("Price must be greater than 0");
        
        RuleFor(p => p.CategoryId)
            .NotEmpty().WithMessage("Category is required");
        
        RuleFor(p => p.StockQuantity)
            .GreaterThanOrEqualTo(0).WithMessage("Stock quantity cannot be negative");
    }
}

public class UpdateProductValidator : AbstractValidator<UpdateProductDto>
{
    public UpdateProductValidator()
    {
        RuleFor(p => p.Id)
            .NotEmpty().WithMessage("Product ID is required");
        
        RuleFor(p => p.Name)
            .NotEmpty().WithMessage("Product name is required")
            .MaximumLength(200).WithMessage("Product name cannot exceed 200 characters");
        
        RuleFor(p => p.Price)
            .GreaterThan(0).WithMessage("Price must be greater than 0");
    }
}

public class CategoryValidator : AbstractValidator<CreateCategoryDto>
{
    public CategoryValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty().WithMessage("Category name is required")
            .MaximumLength(100).WithMessage("Category name cannot exceed 100 characters");
    }
}

public class UpdateCategoryValidator : AbstractValidator<UpdateCategoryDto>
{
    public UpdateCategoryValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty().WithMessage("Category ID is required");
        
        RuleFor(c => c.Name)
            .NotEmpty().WithMessage("Category name is required")
            .MaximumLength(100).WithMessage("Category name cannot exceed 100 characters");
    }
}

public class CartItemValidator : AbstractValidator<AddCartItemDto>
{
    public CartItemValidator()
    {
        RuleFor(c => c.ProductId)
            .NotEmpty().WithMessage("Product ID is required");
        
        RuleFor(c => c.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be greater than 0");
    }
}

public class ReviewValidator : AbstractValidator<CreateReviewDto>
{
    public ReviewValidator()
    {
        RuleFor(r => r.ProductId)
            .NotEmpty().WithMessage("Product ID is required");
        
        RuleFor(r => r.Rating)
            .InclusiveBetween(1, 5).WithMessage("Rating must be between 1 and 5");
        
        RuleFor(r => r.Comment)
            .MaximumLength(1000).WithMessage("Comment cannot exceed 1000 characters");
    }
}

public class PaymentValidator : AbstractValidator<CreatePaymentDto>
{
    public PaymentValidator()
    {
        RuleFor(p => p.OrderId)
            .NotEmpty().WithMessage("Order ID is required");
        
        RuleFor(p => p.Provider)
            .NotEmpty().WithMessage("Payment provider is required");
    }
}

public class LoginValidator : AbstractValidator<LoginDto>
{
    public LoginValidator()
    {
        RuleFor(l => l.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format");
        
        RuleFor(l => l.Password)
            .NotEmpty().WithMessage("Password is required");
    }
}

public class RegisterValidator : AbstractValidator<RegisterDto>
{
    public RegisterValidator()
    {
        RuleFor(r => r.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format");
        
        RuleFor(r => r.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters")
            .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter")
            .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter")
            .Matches("[0-9]").WithMessage("Password must contain at least one digit");
        
        RuleFor(r => r.FirstName)
            .NotEmpty().WithMessage("First name is required")
            .MaximumLength(100).WithMessage("First name cannot exceed 100 characters");
        
        RuleFor(r => r.LastName)
            .NotEmpty().WithMessage("Last name is required")
            .MaximumLength(100).WithMessage("Last name cannot exceed 100 characters");
    }
}
