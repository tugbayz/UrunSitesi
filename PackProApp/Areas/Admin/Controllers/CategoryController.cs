using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PackProApp.Areas.Admin.Modelss.CategoryVMs;
using PackProApp.DTOs.CategoryDTOs;
using PackProApp.Services.CategoryServices;
using X.PagedList;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Localization;
using X.PagedList.Extensions;

namespace PackProApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : AdminBaseController
    {
        private readonly ICategoryService _categoryService;
        private readonly IStringLocalizer<SharedResource> _stringLocalizer;

        public CategoryController(ICategoryService categoryService, IStringLocalizer<SharedResource> stringLocalizer)
        {
            _categoryService = categoryService;
            _stringLocalizer = stringLocalizer;
        }

        public async Task<IActionResult> Index(int page = 1, int pageSize = 20)
        {
            var result = await _categoryService.GetAllAsync();

            if (!result.IsSuccess || result.Data == null)
            {
                var message = _stringLocalizer["Category not found"];
                ErrorNotyf(message);
                return View(new PagedList<AdminCategoryListVM>(new List<AdminCategoryListVM>(), page, pageSize));
            }

            var pagedCategories = result.Data.Adapt<List<AdminCategoryListVM>>().ToPagedList(page, pageSize);
            SuccesNotyf(_stringLocalizer["Categories have been listed"]);
            return View(pagedCategories);
        }

        public async Task<IActionResult> Create()
        {
            var categories = await _categoryService.GetAllAsync();
            var vm = new AdminCategoryCreateVM
            {
                ParentCategories = new SelectList(categories.Data, "Id", "Name")
            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AdminCategoryCreateVM model)
        {
            if (!ModelState.IsValid)
            {
                model.ParentCategories = new SelectList((await _categoryService.GetAllAsync()).Data, "Id", "Name");
                return View(model);
            }

            var categoryCreateDTO = model.Adapt<CategoryCreateDTO>();
            categoryCreateDTO.ParentCategoryId = model.ParentCategoryId;

            var result = await _categoryService.AddAsync(categoryCreateDTO);
            if (!result.IsSuccess)
            {
                ErrorNotyf(_stringLocalizer["Category is registered in the system"]);
                model.ParentCategories = new SelectList((await _categoryService.GetAllAsync()).Data, "Id", "Name");
                return View(model);
            }

            SuccesNotyf(_stringLocalizer["Category added successfully"]);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Update(Guid id)
        {
            var result = await _categoryService.GetByIdAsync(id);
            if (!result.IsSuccess || result.Data == null)
            {
                ErrorNotyf(_stringLocalizer["Category not found in the system"]);
                return RedirectToAction("Index");
            }

            var vm = result.Data.Adapt<AdminCategoryUpdateVM>();
            vm.ParentCategories = new SelectList((await _categoryService.GetAllAsync()).Data, "Id", "Name");
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Update(AdminCategoryUpdateVM model)
        {
            if (!ModelState.IsValid)
            {
                model.ParentCategories = new SelectList((await _categoryService.GetAllAsync()).Data, "Id", "Name", model.ParentCategoryId);
                return View(model);
            }

            var categoryUpdateDTO = model.Adapt<CategoryUpdateDTO>();
            var result = await _categoryService.UpdateAsync(categoryUpdateDTO);

            if (!result.IsSuccess)
            {
                ErrorNotyf(_stringLocalizer["Category could not be updated"]);
                model.ParentCategories = new SelectList((await _categoryService.GetAllAsync()).Data, "Id", "Name", model.ParentCategoryId);
                return View(model);
            }

            SuccesNotyf(_stringLocalizer["Category updated successfully"]);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _categoryService.DeleteAsync(id);
            if (!result.IsSuccess)
            {
                ErrorNotyf(_stringLocalizer["Category could not be deleted"]);
                return RedirectToAction("Index");
            }

            SuccesNotyf(_stringLocalizer["Category has been deleted"]);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var result = await _categoryService.GetByIdAsync(id);

            if (!result.IsSuccess || result.Data == null)
            {
                ErrorNotyf(_stringLocalizer["No category found to display"]);
                return RedirectToAction("Index");
            }

            var vm = result.Data.Adapt<AdminCategoryDetailsVM>();
            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> ShowCategories()
        {
            var result = await _categoryService.GetCategoriesWithSubCategoriesAsync();
            if (!result.IsSuccess)
            {
                ErrorNotyf(_stringLocalizer["Categories not found"]);
                return View(new List<CategoryDTO>());
            }

            return View(result.Data);
        }

        [HttpGet]
        public async Task<IActionResult> AddSubCategory()
        {
            var categories = await _categoryService.GetAllAsync();
            var vm = new AddSubCategoryVM
            {
                Categories = new SelectList(categories.Data, "Id", "Name")
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> AddSubCategory(AddSubCategoryVM model)
        {
            if (!ModelState.IsValid)
            {
                model.Categories = new SelectList((await _categoryService.GetAllAsync()).Data, "Id", "Name");
                return View(model);
            }

            var subCategoryDTO = new SubCategoryCreateDTO
            {
                Name = model.SubCategoryName,
                Description = model.SubCategoryDescription,
                ParentCategoryId = model.SelectedCategoryId
            };

            var result = await _categoryService.AddSubCategoryAsync(subCategoryDTO);

            if (!result.IsSuccess)
            {
                ErrorNotyf(_stringLocalizer["Subcategory could not be added"]);
                return View(model);
            }

            SuccesNotyf(_stringLocalizer["Subcategory added successfully"]);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> GetCategoryDescription(Guid categoryId)
        {
            var category = await _categoryService.GetByIdAsync(categoryId);
            return Json(category.IsSuccess ? category.Data.Description : string.Empty);
        }

        [HttpGet]
        public async Task<IActionResult> GetSubCategoryDescription(Guid subCategoryId)
        {
            var subCategory = await _categoryService.GetByIdAsync(subCategoryId);
            return Json(subCategory.IsSuccess ? subCategory.Data.Description : string.Empty);
        }
    }
}
