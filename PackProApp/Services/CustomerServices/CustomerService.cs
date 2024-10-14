using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PackProApp.DTOs.CustomerDTOs;
using PackProApp.Entities;
using PackProApp.Repositories.CustomerRepositories;
using PackProApp.Services.AccountServices;
using PackProApp.Utilities.Concretes;
using PackProApp.Utilities.Interfaces;

namespace PackProApp.Services.CustomerServices
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IAccountService _accountService;

        public CustomerService(ICustomerRepository customerRepository, IAccountService accountService)
        {
            _customerRepository = customerRepository;
            _accountService = accountService;
        }

        public async Task<Utilities.Interfaces.IResult> AddAsync(CustomerCreateDTO customerCreateDTO)
        {
            if (await _accountService.AnyAsync(x => x.Email == customerCreateDTO.Email))
            {
                return new ErrorResult("Email adresi kullanılıyor");
            }
            IdentityUser user = new()
            {
                Email = customerCreateDTO.Email,
                NormalizedEmail = customerCreateDTO.Email.ToUpperInvariant(),
                UserName = customerCreateDTO.Email,
                NormalizedUserName = customerCreateDTO.Email.ToUpperInvariant(),
                EmailConfirmed = true
            };

            Result result = new Result();
            var str = await _customerRepository.CreateExecutionStrategy();
            await str.ExecuteAsync(async () =>
            {
                var transactionScope = await _customerRepository.BeginTransactionAsync().ConfigureAwait(false);
                try
                {
                    // IdentityUser oluşturuluyor
                    var identityResult = await _accountService.CreateUserAsync(user, Enums.Roles.Customer);
                    if (!identityResult.Succeeded)
                    {
                        result = new ErrorResult(identityResult.ToString());
                        transactionScope.Rollback();
                        return;
                    }

                    // Müşteri oluşturuluyor
                    var newCustomer = customerCreateDTO.Adapt<Customer>();
                    newCustomer.IdentityId = user.Id; // IdentityId set ediliyor
                    await _customerRepository.AddAsync(newCustomer);
                    await _customerRepository.SaveChangeAsync();
                    result = new SuccessResult("Müşteri ekleme başarılı");
                    transactionScope.Commit();
                }
                catch (Exception ex)
                {
                    result = new ErrorResult("Hata: " + ex.Message);
                    transactionScope.Rollback();
                }
                finally
                {
                    transactionScope.Dispose();
                }
            });
            return result;
        }

        public async Task<Utilities.Interfaces.IResult> DeleteByAsync(Guid id)
        {
            // Müşteriyi ID'ye göre veritabanından alıyoruz.
            var customer = await _customerRepository.GetByIdAsync(id);

            // Eğer müşteri bulunamazsa, hata sonucu döndür.
            if (customer == null)
            {
                return new ErrorResult("Müşteri bulunamadı");
            }

            Result result = new Result();

            // Veritabanı işlem stratejisi oluşturuyoruz.
            var str = await _customerRepository.CreateExecutionStrategy();
            await str.ExecuteAsync(async () =>
            {
                var transactionScope = await _customerRepository.BeginTransactionAsync().ConfigureAwait(false);
                try
                {
                    // Müşteriyi veritabanından siliyoruz.
                    await _customerRepository.DeleteAsync(customer);

                    // İlgili müşteri kimliğini kullanarak hesabı siliyoruz.
                    var identityResult = await _accountService.DeleteByAsync(customer.IdentityId);

                    // Eğer hesap silme işlemi başarısız olursa, hata sonucu döndür ve işlemi geri al.
                    if (!identityResult.Succeeded)
                    {
                        result = new ErrorResult(identityResult.ToString());
                        transactionScope.Rollback();
                        return;
                    }
                    // Veritabanı değişikliklerini kaydediyoruz.
                    await _customerRepository.SaveChangeAsync();
                    result = new SuccessResult("Müşteri silme başarılı");
                    transactionScope.Commit();
                }
                catch (Exception ex)
                {
                    // Bir hata oluşursa, hata sonucu döndür ve işlemi geri al.
                    result = new ErrorResult("Hata: " + ex.Message);
                    transactionScope.Rollback();
                }
                finally
                {
                    // İşlem kapsamını temizliyoruz.
                    transactionScope.Dispose();
                }
            });

            return result;
        }

        public async Task<IDataResult<List<CustomerListDTO>>> GetAllAsync()
        {
            var customerDTOs = (await _customerRepository.GetAllAsync()).Adapt<List<CustomerListDTO>>();
            return new SuccessDataResult<List<CustomerListDTO>>(customerDTOs, "Müşteri listeleme başarılı");
        }

        public async Task<IDataResult<CustomerDTO>> GetByIdAsync(Guid id)
        {
            try
            {
                var customer = await _customerRepository.GetByIdAsync(id);
                if (customer == null)
                {
                    return new ErrorDataResult<CustomerDTO>("Müşteri bulunamadı");
                }
                var customerDTO = customer.Adapt<CustomerDTO>();
                return new SuccessDataResult<CustomerDTO>(customerDTO, "Müşteri başarıyla getirildi");
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<CustomerDTO>("Hata: " + ex.Message);
            }
        }

        public async Task<Utilities.Interfaces.IResult> UpdateByAsync(CustomerUpdateDTO customerUpdateDTO)
        {
            // Müşteriyi ID'ye göre veritabanından alıyoruz.
            var customer = await _customerRepository.GetByIdAsync(customerUpdateDTO.Id);

            // Eğer müşteri bulunamazsa, hata sonucu döndür.
            if (customer == null)
            {
                return new ErrorResult("Müşteri bulunamadı");
            }

            Result result = new Result();
            var str = await _customerRepository.CreateExecutionStrategy();

            // İşlem stratejisini kullanarak işlemleri yürütüyoruz.
            await str.ExecuteAsync(async () =>
            {
                var transactionScope = await _customerRepository.BeginTransactionAsync().ConfigureAwait(false);
                try
                {
                    // Gelen DTO'dan müşteriye verileri kopyalıyoruz.
                    customer = customerUpdateDTO.Adapt(customer);
                    // Müşteriyi veritabanında güncelliyoruz.
                    await _customerRepository.UpdateAsync(customer);

                    // Eğer e-posta adresi değiştiyse, Identity kullanıcısını da güncelle
                    if (customer.Email != customerUpdateDTO.Email)
                    {
                        var identityResult = await _accountService.UpdateEmailAsync(customer.IdentityId, customerUpdateDTO.Email);

                        // Eğer hesap güncelleme işlemi başarısız olursa, hata sonucu döndür ve işlemi geri al.
                        if (!identityResult.Succeeded)
                        {
                            result = new ErrorResult(identityResult.ToString());
                            transactionScope.Rollback();
                            return;
                        }
                    }
                    // Veritabanı değişikliklerini kaydediyoruz.
                    await _customerRepository.SaveChangeAsync();
                    result = new SuccessResult("Müşteri güncelleme başarılı");
                    transactionScope.Commit();
                }
                catch (Exception ex)
                {
                    result = new ErrorResult("Hata: " + ex.Message);
                    transactionScope.Rollback();
                }
                finally
                {
                    // İşlem kapsamını temizliyoruz.
                    transactionScope.Dispose();
                }
            });
            return result;
        }
    }
}
