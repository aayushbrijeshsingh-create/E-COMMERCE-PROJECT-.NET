using AutoMapper;
using ECommerce.Models.DTOs;
using Infrastructure.Data.Entities;

namespace ECommerce.Business.Logic.MappingProfiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Product mappings
        CreateMap<Product, ProductDto>();
        CreateMap<CreateProductDto, Product>();
        CreateMap<UpdateProductDto, Product>();

        // Category mappings
        CreateMap<Category, CategoryDto>();
        CreateMap<CreateCategoryDto, Category>();
        CreateMap<UpdateCategoryDto, Category>();

        // Order mappings
        CreateMap<Order, OrderDto>();
        CreateMap<OrderItem, OrderItemDto>();

        // Cart mappings
        CreateMap<Cart, CartDto>();
        CreateMap<CartItem, CartItemDto>();

        // Customer/User mappings
        CreateMap<Customer, UserDto>();
    }
}
