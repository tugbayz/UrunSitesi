using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Localization;
using PackProApp.Areas.Admin.Modelss.CustomerVMs;
using PackProApp.Areas.Customer.Models;
using PackProApp.DTOs.CustomerDTOs;
using PackProApp.Services.CustomerServices;

namespace PackProApp.Areas.Admin.Controllers
{

    public class CustomerController : AdminBaseController
    {
        private readonly ICustomerService _customerService;
        private readonly IStringLocalizer<SharedResource> _stringLocalizer;
        

        public CustomerController(ICustomerService customerService, IStringLocalizer<SharedResource> stringLocalizer)
        {
            _customerService = customerService;
            _stringLocalizer = stringLocalizer;
            
        }

        public async Task<IActionResult> Index()
        {
            
            

            var result = await _customerService.GetAllAsync();
            var customerVMs = result.Data.Adapt<List<AdminCustomerListVM>>();
            if (!result.IsSuccess)
            {
                var msg = _stringLocalizer["Customer list not found"];
                ErrorNotyf(msg);
                return View(customerVMs);
            }
            var message = _stringLocalizer["Customer listing successfull"];
            SuccesNotyf(message);
            return View(customerVMs);
        }

        public async Task<IActionResult> Create()
        {
            var customerCreateVM = new AdminCustomerCreateVM();
            return View(customerCreateVM);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AdminCustomerCreateVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var customerCreateDTO = model.Adapt<CustomerCreateDTO>();
            var result = await _customerService.AddAsync(customerCreateDTO);
            if (!result.IsSuccess)
            {
                var message = _stringLocalizer["Email address is already in use"];
                ErrorNotyf(message);
                return View(model);
            }
            var message2=_stringLocalizer["Customer added successfully"];
            SuccesNotyf(message2);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Update(Guid customerId)
        {
            var result = await _customerService.GetByIdAsync(customerId);

            if (!result.IsSuccess)
            {
                var message = _stringLocalizer["Customer not found"];
                ErrorNotyf(message);
                return RedirectToAction("Index");
            }

            var customerUpdateVM = result.Data.Adapt<AdminCustomerUpdateVM>();
            return View(customerUpdateVM);
        }

        [HttpPost]
        public async Task<IActionResult> Update(AdminCustomerUpdateVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }


            var result = await _customerService.UpdateByAsync(model.Adapt<CustomerUpdateDTO>());
            if (!result.IsSuccess)
            {
                var message = _stringLocalizer["An unknown error occurred."];
                ErrorNotyf(message);
                return View(model);
            }
            var message2 = _stringLocalizer["Customer updated successfully"];
            SuccesNotyf(result.Message);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(Guid customerId)
        {
            var result = await _customerService.DeleteByAsync(customerId);
            if (!result.IsSuccess)
            {
                var message = _stringLocalizer["Customer could not be deleted"];
                ErrorNotyf(message);
                return RedirectToAction("Index");
            }
            var message2 = _stringLocalizer["Customer deleted successfully"];
            SuccesNotyf(message2);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Details(Guid customerId)
        {
            var result = await _customerService.GetByIdAsync(customerId);
            if (!result.IsSuccess)
            {
                var message = _stringLocalizer["Customer not found"];
                ErrorNotyf(message);
                return RedirectToAction("Index");
            }
            return View(result.Data.Adapt<AdminCustomerDetailVM>());
        }
    }
}
