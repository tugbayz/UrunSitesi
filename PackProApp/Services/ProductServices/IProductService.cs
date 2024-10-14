using PackProApp.DTOs.ProductDTOs;
using PackProApp.Utilities.Interfaces;
using IResult = PackProApp.Utilities.Interfaces.IResult;

namespace PackProApp.Services.ProductServices
{
    public interface IProductService
    {
        Task<IResult> AddAsync(ProductCreateDTO productCreateDTO);
        Task<IDataResult<List<ProductListDTO>>> GetAllAsync();
        Task<IResult> DeleteAsync(Guid id);
        Task<IDataResult<ProductDTO>> GetByIdAsync(Guid id);
        Task<IResult> UpdateAsync(ProductUpdateDTO productUpdateDTO);
        Task<IEnumerable<object>> GetAllProductsAsync();
    }
}
