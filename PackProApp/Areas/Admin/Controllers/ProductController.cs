using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using PackProApp.Areas.Admin.Modelss.ProductVMs;
using PackProApp.DTOs.ProductDTOs;
using PackProApp.Services.CategoryServices;
using PackProApp.Services.ImagesServices;
using PackProApp.Services.ProductServices;
using X.PagedList;
using X.PagedList.Extensions;




namespace PackProApp.Areas.Admin.Controllers
{
    public class ProductController : AdminBaseController
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IStringLocalizer<SharedResource> _stringLocalizer;
      
        private readonly IImageService _imageService;


        public ProductController(IProductService productService, ICategoryService categoryService, IStringLocalizer<SharedResource> stringLocalizer, IImageService imageService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _stringLocalizer = stringLocalizer;
           
            _imageService = imageService;
        }

        public async Task<IActionResult> Index(int? page)
        {
            
            var pageNumber = page ?? 1; // Eğer page parametresi null ise, 1. sayfa olsun
            var pageSize = 3; // Sayfa başına kaç ürün gösterileceğini belirleyin

            // Ürünleri servisten alıyoruz
            var products = await _productService.GetAllAsync();

            // Eğer sonuç başarısızsa veya veri yoksa, hata mesajını göster ve boş bir liste döndür
            if (products == null || !products.IsSuccess || products.Data == null)
            {
                var message = _stringLocalizer["An unknown error occurred."];
                ErrorNotyf(message);
                return View(new PagedList<AdminProductListVM>(new List<AdminProductListVM>(), pageNumber, pageSize));
            }

            // Veritabanındaki resim dosya yolunu ürünlere ekleyin
            var productList = products.Data.ToList(); // Listeye dönüştür
            foreach (var product in productList)
            {
                var imagePath = await _imageService.GetImagePath(product.Id); // Resim yolunu asenkron olarak al

                // Resim dosyasını baytlara dönüştür
                if (System.IO.File.Exists(imagePath)) // Dosyanın mevcut olduğunu kontrol et
                {
                    product.Image = await System.IO.File.ReadAllBytesAsync(imagePath); // Baytları oku
                }
            }

            // Ürün verilerini List<AdminProductListVM> formatına çevir ve sayfalama uygula
            var pagedProducts = productList
                                    .Adapt<List<AdminProductListVM>>() // Mapster kullanarak dönüşüm
                                    .ToPagedList(pageNumber, pageSize); // ToPagedList ile sayfalama

            // Sayfalı listeyi View'e gönder
            return View(pagedProducts);
        }



        //public async Task<IActionResult> Index(int? page)
        //{
        //    var pageNumber = page ?? 1; // Eğer page parametresi null ise, 1. sayfa olsun
        //    var pageSize = 3; // Sayfa başına kaç ürün gösterileceğini belirleyin

        //    // Ürünleri servisten alıyoruz
        //    var products = await _productService.GetAllAsync();

        //    // Eğer sonuç başarısızsa veya veri yoksa, hata mesajını göster ve boş bir liste döndür
        //    if (products == null || !products.IsSuccess || products.Data == null)
        //    {
        //        ErrorNotyf("An unknown error occurred.");
        //        return View(new PagedList<AdminProductListVM>(new List<AdminProductListVM>(), pageNumber, pageSize));
        //    }

        //    // Veritabanındaki resim dosya yolunu ürünlere ekleyin
        //    foreach (var product in products.Data)
        //    {
        //        product.Image =_imageService.GetImagePath(product.Id); // Resim yolunu al ve ürüne ekle
        //    }

        //    // Ürün verilerini List<AdminProductListVM> formatına çevir ve sayfalama uygula
        //    var pagedProducts = products.Data
        //                                .Adapt<List<AdminProductListVM>>() // Mapster kullanarak dönüşüm
        //                                .ToPagedList(pageNumber, pageSize); // ToPagedList ile sayfalama

        //    // Sayfalı listeyi View'e gönder
        //    return View(pagedProducts);
        //}


        //public async Task<IActionResult> Index(int? page)
        //{
        //    var pageNumber = page ?? 1; // Eğer page parametresi null ise, 1. sayfa olsun
        //    var pageSize = 3; // Sayfa başına kaç ürün gösterileceğini belirleyin
        //    var products = await _productService.GetAllAsync();

        //    if (products == null || !products.IsSuccess || products.Data == null)
        //    {
        //        ErrorNotyf("An unknown error occurred.");
        //        return View(new PagedList<AdminProductListVM>(new List<AdminProductListVM>(), pageNumber, pageSize));
        //    }

        //    // Veriyi IPagedList'e dönüştürüyoruz
        //    var pagedProducts = products.Data.Adapt<List<AdminProductListVM>>()
        //                               .ToPagedList(pageNumber, pageSize);

        //    return View(pagedProducts);
        //}


        //public async Task<IActionResult> Index()
        //{
        //    ViewData["Message"] = _stringLocalizer["Message"];

        //    var result = await _productService.GetAllAsync();

        //    if (result == null || !result.IsSuccess || result.Data == null)
        //    {
        //        ErrorNotyf(result?.Message ?? "An unknown error occurred.");
        //        return View(new List<AdminProductListVM>());
        //    }

        //    var adminProductListVm = result.Data.Adapt<List<AdminProductListVM>>();
        //    SuccesNotyf(result.Message);
        //    return View(/*pagedList*/);
        //}

        private async Task<SelectList> GetCategories(Guid? categoryId = null)
        {
            var categories = (await _categoryService.GetAllAsync()).Data;
            return new SelectList(categories, "Id", "Name", categoryId);
        }

        public async Task<IActionResult> Create()

        {
            

            AdminProductCreateVM vm = new AdminProductCreateVM
            {
                Categories = await GetCategories()
            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AdminProductCreateVM model)
        {
            if (model.Name == null)
            {
                var message = _stringLocalizer["Please enter product name"];
                ErrorNotyf(message); // Uygun bir hata mesajı gösterebilirsiniz.
                return RedirectToAction("Index"); // Veya uygun bir hata dönüşü yapın.
            }
            if(model.Price<=0)
            {
                var msg = _stringLocalizer["Please enter a valid price"];
                ErrorNotyf(msg);
                return RedirectToAction("Index");
            }

            if (!ModelState.IsValid)
            {
                model.Categories = await GetCategories();
                return View(model);
            }

            var productCreateDTO = model.Adapt<ProductCreateDTO>();

            if (model.NewImage != null && model.NewImage.Length > 0)
            {
                productCreateDTO.Image = await model.NewImage.StringToByteArrayAsync();
            }

            var result = await _productService.AddAsync(productCreateDTO);

            if (!result.IsSuccess)
            {
                var message = _stringLocalizer["Product is registered in the system"];
                ErrorNotyf(message);
                model.Categories = await GetCategories();
                return View(model);
            }

            var message2=_stringLocalizer["Product added successfully"];
            SuccesNotyf(message2);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Update(Guid id)
        {
            

            var result = await _productService.GetByIdAsync(id);
            if (result == null || !result.IsSuccess)
            {
                var mesage = _stringLocalizer["Specified product not found"];
                ErrorNotyf(mesage ?? "An unknown error occurred.");
                return RedirectToAction("Index");
            }

            var vm = result.Data.Adapt<AdminProductUpdateVM>();
            vm.Categories = await GetCategories(vm.CategoryId);
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Update(AdminProductUpdateVM model)
        {
            

            if (!ModelState.IsValid)
            {
                model.Categories = await GetCategories(model.CategoryId);
                return View(model);
            }

            var updateProductDTO = model.Adapt<ProductUpdateDTO>();
            var result = await _productService.UpdateAsync(updateProductDTO);

            if (!result.IsSuccess)
            {
                var message = _stringLocalizer["Product could not be updated"];
                ErrorNotyf(message);
                model.Categories = await GetCategories(model.CategoryId);
                return View(model);
            }

            var mesage2 = _stringLocalizer["Product updated successfully"];
            SuccesNotyf(mesage2);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            

            var result = await _productService.DeleteAsync(id);
            if (!result.IsSuccess)
            {
                var message = _stringLocalizer["Product could not be deleted"];
                ErrorNotyf(message);
                return RedirectToAction("Index");
            }
            var message1=_stringLocalizer["Product has been deleted"];
            SuccesNotyf(message1);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Details(Guid id)
        {
            

            var result = await _productService.GetByIdAsync(id);
            if (!result.IsSuccess)
            {
                var message = _stringLocalizer["Product not found"];
                ErrorNotyf(message);
                return RedirectToAction("Index");
            }

            return View(result.Data.Adapt<AdminProductDetailVM>());
        }
    }
}
