using AutoMapper;
using ECommerce.Models.DTOs;
using ECommerce.Models.Exceptions;
using Infrastructure.Data.Entities;
using Infrastructure.Repositories;

namespace ECommerce.Business.Logic.Services.Implementation;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;

    public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    public async Task<List<CategoryDto>> GetAllCategoriesAsync()
    {
        var categories = await _categoryRepository.GetAllAsync();
        return _mapper.Map<List<CategoryDto>>(categories) ?? new List<CategoryDto>();
    }

    public async Task<CategoryDto?> GetCategoryByIdAsync(Guid id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        return category != null ? _mapper.Map<CategoryDto>(category) : null;
    }

    public async Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto dto)
    {
        var category = _mapper.Map<Category>(dto) ?? throw new InvalidOperationException("Failed to map category");
        category.Id = Guid.NewGuid();
        
        await _categoryRepository.AddAsync(category);
        await _categoryRepository.SaveChangesAsync();
        
        return _mapper.Map<CategoryDto>(category) ?? throw new InvalidOperationException("Failed to map category");
    }

    public async Task UpdateCategoryAsync(UpdateCategoryDto dto)
    {
        var category = await _categoryRepository.GetByIdAsync(dto.Id);
        if (category == null)
            throw new NotFoundException("Category", dto.Id.ToString());
        
        _mapper.Map(dto, category);
        _categoryRepository.Update(category);
        await _categoryRepository.SaveChangesAsync();
    }

    public async Task DeleteCategoryAsync(Guid id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        if (category == null)
            throw new NotFoundException("Category", id.ToString());
        
        _categoryRepository.Delete(category);
        await _categoryRepository.SaveChangesAsync();
    }
}
