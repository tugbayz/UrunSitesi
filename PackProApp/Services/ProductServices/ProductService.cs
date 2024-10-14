using Mapster;
using PackProApp.DTOs.ProductDTOs;
using PackProApp.Entities;
using PackProApp.Repositories.CategoryRepositories;
using PackProApp.Repositories.ProductRepositories;
using PackProApp.Utilities.Concretes;
using PackProApp.Utilities.Interfaces;
using IResult = PackProApp.Utilities.Interfaces.IResult;

namespace PackProApp.Services.ProductServices
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ProductService(IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        // Ürün ekleme işlemi
        public async Task<IResult> AddAsync(ProductCreateDTO productCreateDTO)
        {
            if (await _productRepository.AnyAsync(x => x.Name.ToLower() == productCreateDTO.Name.ToLower()))
            {
                return new ErrorResult("Ürün sistemde kayıtlı");
            }

            // Ürünün atanacağı kategorinin varlığını kontrol ediyoruz
            var category = await _categoryRepository.GetByIdAsync(productCreateDTO.CategoryId);
            if (category == null)
            {
                return new ErrorResult("Belirtilen kategori bulunamadı");
            }

            try
            {
                var newProduct = productCreateDTO.Adapt<Product>();
                await _productRepository.AddAsync(newProduct);
                await _productRepository.SaveChangeAsync();
                return new SuccessResult("Ürün ekleme başarılı");
            }
            catch (Exception ex)
            {
                return new ErrorResult("Hata: " + ex.Message);
            }
        }

        // Ürün silme işlemi
        public async Task<IResult> DeleteAsync(Guid id)
        {
            var deletingProduct = await _productRepository.GetByIdAsync(id);
            if (deletingProduct == null)
            {
                return new ErrorResult("Ürün sistemde bulunamadı");
            }
            try
            {
                await _productRepository.DeleteAsync(deletingProduct);
                await _productRepository.SaveChangeAsync();
                return new SuccessResult("Ürün silme işlemi başarılı");
            }
            catch (Exception ex)
            {
                return new ErrorResult("Hata: " + ex.Message);
            }
        }

        // Tüm ürünleri getirme
        public async Task<IDataResult<List<ProductListDTO>>> GetAllAsync()
        {
            // Tüm ürünleri getiriyoruz
            var products = await _productRepository.GetAllAsync();

            // Her ürün için kategoriyi manuel olarak getirip DTO'ya ekliyoruz
            var productListDTO = new List<ProductListDTO>();

            foreach (var product in products)
            {
                var productDTO = product.Adapt<ProductListDTO>();

                // Kategori verisini manuel olarak çekiyoruz
                var category = await _categoryRepository.GetByIdAsync(product.CategoryId);
                if (category != null)
                {
                    productDTO.CategoryName = category.Name;
                }

                productListDTO.Add(productDTO);
            }

            if (!productListDTO.Any())
            {
                return new ErrorDataResult<List<ProductListDTO>>(productListDTO, "Listelenecek ürün bulunamadı");
            }

            return new SuccessDataResult<List<ProductListDTO>>(productListDTO, "Ürünler listelendi");
        }

        public async Task<IEnumerable<object>> GetAllProductsAsync()
        {
            var products = await _productRepository.GetAllAsync();
            var productDTOs = new List<ProductDTO>();

            foreach (var product in products)
            {
                var productDTO = product.Adapt<ProductDTO>();

                // Kategori verisini manuel olarak çekiyoruz
                var category = await _categoryRepository.GetByIdAsync(product.CategoryId);
                if (category != null)
                {
                    productDTO.CategoryName = category.Name;
                }

                productDTOs.Add(productDTO);
            }

            return productDTOs;
        }


        // Tüm ürünleri alır ve ProductDTO listesi döndürür
        //public async Task<List<ProductDTO>> GetAllProductsAsync()
        //{
        //    var products = await _productRepository.GetAllAsync();
        //    var productDTOs = new List<ProductDTO>();

        //    foreach (var product in products)
        //    {
        //        var productDTO = product.Adapt<ProductDTO>();

        //        // Kategori verisini manuel olarak çekiyoruz
        //        var category = await _categoryRepository.GetByIdAsync(product.CategoryId);
        //        if (category != null)
        //        {
        //            productDTO.CategoryName = category.Name;
        //        }

        //        productDTOs.Add(productDTO);
        //    }

        //    return productDTOs;
        //}

        // Belirli bir ürünü ID ile getirir
        public async Task<IDataResult<ProductDTO>> GetByIdAsync(Guid id)
        {
            var product = await _productRepository.GetByIdAsync(id);

            if (product == null)
            {
                return new ErrorDataResult<ProductDTO>(null, "Gösterilecek ürün bulunamadı");
            }

            // DTO'yu oluşturun ve gerekli bilgileri ekleyin
            var productDTO = product.Adapt<ProductDTO>();

            // Kategori verisini manuel olarak çekiyoruz
            var category = await _categoryRepository.GetByIdAsync(product.CategoryId);
            if (category != null)
            {
                productDTO.CategoryName = category.Name;
            }

            return new SuccessDataResult<ProductDTO>(productDTO, "Ürün bulundu");
        }

        // Ürün güncelleme işlemi
        public async Task<IResult> UpdateAsync(ProductUpdateDTO productUpdateDTO)
        {
            var updatingProduct = await _productRepository.GetByIdAsync(productUpdateDTO.Id);
            if (updatingProduct == null)
            {
                return new ErrorResult("Güncellenecek ürün bulunamadı");
            }

            // Ürünün atanacağı kategorinin varlığını kontrol ediyoruz
            var category = await _categoryRepository.GetByIdAsync(productUpdateDTO.CategoryId);
            if (category == null)
            {
                return new ErrorResult("Belirtilen kategori bulunamadı");
            }

            try
            {
                var updatedProduct = productUpdateDTO.Adapt(updatingProduct);
                await _productRepository.UpdateAsync(updatedProduct);
                await _productRepository.SaveChangeAsync();
                return new SuccessResult("Ürün güncelleme başarılı");
            }
            catch (Exception ex)
            {
                return new ErrorResult("Hata: " + ex.Message);
            }
        }

        //Task<IEnumerable<object>> IProductService.GetAllProductsAsync()
        ////{
        ////    throw new NotImplementedException();
        ////}
    }
}
