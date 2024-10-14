using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PackProApp.Controllers;

namespace PackProApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AdminBaseController : BaseController
    {

    }
}
