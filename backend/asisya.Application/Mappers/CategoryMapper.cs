using asisya.Application.DTOs.Category;
using asisya.Domain.Entities;

namespace asisya.Application.Mappers;

public static class CategoryMapper
{
    public static CategoryDto ToDto(Category entity) => new()
    {
        CategoryID = entity.CategoryID,
        CategoryName = entity.CategoryName,
        Description = entity.Description,
        PictureBase64 = entity.Picture is not null
            ? Convert.ToBase64String(entity.Picture)
            : null
    };

    public static Category ToEntity(CreateCategoryDto dto) => new()
    {
        CategoryName = dto.CategoryName,
        Description = dto.Description,
        Picture = dto.PictureBase64 is not null
            ? Convert.FromBase64String(dto.PictureBase64)
            : null
    };
}
