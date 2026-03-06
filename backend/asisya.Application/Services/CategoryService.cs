using asisya.Application.DTOs.Category;
using asisya.Application.Interfaces;
using asisya.Application.Mappers;
using asisya.Domain.Interfaces;

namespace asisya.Application.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _repository;

    public CategoryService(ICategoryRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<CategoryDto>> GetAllAsync()
    {
        var categories = await _repository.GetAllAsync();
        return categories.Select(CategoryMapper.ToDto);
    }

    public async Task<CategoryDto?> GetByIdAsync(int id)
    {
        var category = await _repository.GetByIdAsync(id);
        return category is null ? null : CategoryMapper.ToDto(category);
    }

    public async Task<CategoryDto> CreateAsync(CreateCategoryDto dto)
    {
        var entity = CategoryMapper.ToEntity(dto);
        var created = await _repository.AddAsync(entity);
        return CategoryMapper.ToDto(created);
    }
}
