using PackProApp.DTOs.CustomerDTOs;
using PackProApp.DTOs.ProductDTOs;
using PackProApp.Utilities.Interfaces;
using System.Threading.Tasks;

namespace PackProApp.Services.CustomerServices
{
    public interface ICustomerService
    {
        Task<Utilities.Interfaces.IResult> AddAsync(CustomerCreateDTO customerCreateDTO);
        Task<IDataResult<List<CustomerListDTO>>> GetAllAsync();
        Task<Utilities.Interfaces.IResult> DeleteByAsync(Guid id);
        Task<IDataResult<CustomerDTO>> GetByIdAsync(Guid id);
        Task<Utilities.Interfaces.IResult> UpdateByAsync(CustomerUpdateDTO customerUpdateDTO);
    }
}
