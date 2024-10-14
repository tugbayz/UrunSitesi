using PackProApp.DTOs.CategoryDTOs;
using PackProApp.DTOs.SubCategoryDTOs;
using PackProApp.Utilities.Interfaces;
using IResult = PackProApp.Utilities.Interfaces.IResult;

namespace PackProApp.Services.CategoryServices
{
    public interface ICategoryService
    {
        Task<IResult> AddAsync(CategoryCreateDTO categoryCreateDTO);
        Task<IResult> DeleteAsync(Guid id);
        Task<IDataResult<List<CategoryListDTO>>> GetAllAsync();
        Task<IDataResult<CategoryDTO>> GetByIdAsync(Guid id);
        Task<IResult> UpdateAsync(CategoryUpdateDTO categoryUpdateDTO);

        // Yeni metodlar
        Task<IDataResult<List<CategoryDTO>>> GetSubCategoriesAsync(Guid parentId);

        Task<IDataResult<List<CategoryDTO>>> GetCategoriesWithSubCategoriesAsync();






        Task<IResult> AddSubCategoryAsync(SubCategoryCreateDTO subCategoryCreateDTO);
           

    }
}
