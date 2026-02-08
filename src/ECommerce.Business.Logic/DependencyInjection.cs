using ECommerce.Business.Logic.MappingProfiles;
using ECommerce.Business.Logic.Services;
using ECommerce.Business.Logic.Services.Implementation;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Business.Logic;

public static class DependencyInjection
{
    public static IServiceCollection AddBusinessLogic(this IServiceCollection services)
    {
        // Register AutoMapper
        services.AddAutoMapper(typeof(MappingProfile));

        // Register Services
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<ICartService, CartService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IReviewService, ReviewService>();
        services.AddScoped<IPaymentService, PaymentService>();

        // Register Validators
        services.AddValidatorsFromAssemblyContaining<BusinessLogicAssemblyMarker>();

        return services;
    }
}

public class BusinessLogicAssemblyMarker { }
