using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PackProApp.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize(Roles = "Customer")]
    public class CustomerBaseController : Controller
    {
        protected INotyfService notyfService =>
            HttpContext.RequestServices.GetService(typeof(INotyfService)) as INotyfService;

        protected void SuccesNotyf(string message)
        {
            notyfService.Success(message);
        }

        protected void ErrorNotyf(string message)
        {
            notyfService.Error(message);
        }
    }
}
