using Mapster;
using PackProApp.DTOs.CategoryDTOs;
using PackProApp.DTOs.ProductDTOs;
using PackProApp.DTOs.SubCategoryDTOs;
using PackProApp.Entities;
using PackProApp.Repositories.CategoryRepositories;
using PackProApp.Utilities.Concretes;
using PackProApp.Utilities.Interfaces;
using IResult = PackProApp.Utilities.Interfaces.IResult;

namespace PackProApp.Services.CategoryServices
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        // Kategori ekleme işlemi
        public async Task<IResult> AddAsync(CategoryCreateDTO categoryCreateDTO)
        {
            if (await _categoryRepository.AnyAsync(x => x.Name.ToLower() == categoryCreateDTO.Name.ToLower()))
            {
                return new ErrorResult("Kategori sistemde kayıtlı");
            }

            try
            {
                // Yeni kategori oluşturma
                var newCategory = categoryCreateDTO.Adapt<Category>();

                // Üst kategori belirlenmişse, ParentCategoryId ile ilişkilendir
                newCategory.ParentCategoryId = categoryCreateDTO.ParentCategoryId;

                await _categoryRepository.AddAsync(newCategory);
                await _categoryRepository.SaveChangeAsync();

                // Eğer alt kategoriler mevcutsa, onları da ekleyin
                if (categoryCreateDTO.Subcategories != null && categoryCreateDTO.Subcategories.Any())
                {
                    foreach (var subCategory in categoryCreateDTO.Subcategories)
                    {
                        var newSubCategory = new Category
                        {
                            Name = subCategory.Name,
                            Description = subCategory.Description,
                            ParentCategoryId = newCategory.Id
                        };
                        await _categoryRepository.AddAsync(newSubCategory);
                    }
                    await _categoryRepository.SaveChangeAsync();
                }

                return new SuccessResult("Kategori ekleme başarılı");
            }
            catch (Exception ex)
            {
                return new ErrorResult("Hata: " + ex.Message);
            }
        }

        // Kategori silme işlemi
        public async Task<IResult> DeleteAsync(Guid id)
        {
            var deletingCategory = await _categoryRepository.GetByIdAsync(id);
            if (deletingCategory == null)
            {
                return new ErrorResult("Kategori sistemde bulunamadı");
            }
            try
            {
                await _categoryRepository.DeleteAsync(deletingCategory);
                await _categoryRepository.SaveChangeAsync();
                return new SuccessResult("Kategori silme işlemi başarılı");
            }
            catch (Exception ex)
            {
                return new ErrorResult("Hata: " + ex.Message);
            }
        }

        // Tüm kategorileri getirme
        public async Task<IDataResult<List<CategoryListDTO>>> GetAllAsync()
        {
            var categories = await _categoryRepository.GetAllAsync(c => c.ParentCategoryId == null);
            var categoryListDTO = categories.Adapt<List<CategoryListDTO>>();

            if (!categories.Any())
            {
                return new ErrorDataResult<List<CategoryListDTO>>(categoryListDTO, "Listelenecek kategori bulunamadı");
            }

            return new SuccessDataResult<List<CategoryListDTO>>(categoryListDTO, "Kategoriler listelendi");
        }

        // Belirli bir kategoriyi ID ile getirme
        public async Task<IDataResult<CategoryDTO>> GetByIdAsync(Guid id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                return new ErrorDataResult<CategoryDTO>(null, "Gösterilecek kategori bulunamadı");
            }

            // Kategoriyi ve alt kategorilerini dönüştür
            var categoryDTO = category.Adapt<CategoryDTO>();
            var subCategoriesResult = await GetSubCategoriesAsync(id);

            if (subCategoriesResult.IsSuccess)
            {
                categoryDTO.SubCategories = subCategoriesResult.Data;
            }

            return new SuccessDataResult<CategoryDTO>(categoryDTO, "Kategori bulundu");
        }

        // Kategori güncelleme işlemi
        public async Task<IResult> UpdateAsync(CategoryUpdateDTO categoryUpdateDTO)
        {
            var updatingCategory = await _categoryRepository.GetByIdAsync(categoryUpdateDTO.Id);
            if (updatingCategory == null)
            {
                return new ErrorResult("Güncellenecek kategori bulunamadı");
            }

            try
            {
                updatingCategory.Name = categoryUpdateDTO.Name;
                updatingCategory.Description = categoryUpdateDTO.Description;

                var existingSubCategories = updatingCategory.SubCategories.ToList();

                foreach (var subCategoryDTO in categoryUpdateDTO.SubCategories)
                {
                    var existingSubCategory = existingSubCategories.FirstOrDefault(sc => sc.Id == subCategoryDTO.Id);
                    if (existingSubCategory != null)
                    {
                        existingSubCategory.Name = subCategoryDTO.Name;
                        existingSubCategory.Description = subCategoryDTO.Description;
                    }
                }

                await _categoryRepository.UpdateAsync(updatingCategory);
                await _categoryRepository.SaveChangeAsync();

                return new SuccessResult("Ana kategori ve alt kategoriler güncelleme başarılı");
            }
            catch (Exception ex)
            {
                return new ErrorResult("Hata: " + ex.Message);
            }
        }

        // Alt kategorileri getirme işlemi
        public async Task<IDataResult<List<CategoryDTO>>> GetSubCategoriesAsync(Guid parentId)
        {
            try
            {
                var subCategories = await _categoryRepository.GetAllAsync(c => c.ParentCategoryId == parentId);
                var subCategoryDTOs = subCategories.Adapt<List<CategoryDTO>>();

                if (!subCategories.Any())
                {
                    return new ErrorDataResult<List<CategoryDTO>>(subCategoryDTOs, "Alt kategori bulunamadı");
                }

                return new SuccessDataResult<List<CategoryDTO>>(subCategoryDTOs, "Alt kategoriler listelendi");
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<List<CategoryDTO>>(null, $"Bir hata oluştu: {ex.Message}");
            }
        }

        // Alt kategori ekleme işlemi
        public async Task<IResult> AddSubCategoryAsync(SubCategoryCreateDTO subCategoryCreateDTO)
        {
            if (subCategoryCreateDTO.ParentCategoryId == null)
            {
                return new ErrorResult("Üst kategori belirtmelisiniz.");
            }

            var newSubCategory = new Category
            {
                Name = subCategoryCreateDTO.Name,
                Description = subCategoryCreateDTO.Description,
                ParentCategoryId = subCategoryCreateDTO.ParentCategoryId.Value
            };

            try
            {
                await _categoryRepository.AddAsync(newSubCategory);
                await _categoryRepository.SaveChangeAsync();
                return new SuccessResult("Alt kategori başarıyla eklendi.");
            }
            catch (Exception ex)
            {
                return new ErrorResult("Hata: " + ex.Message);
            }
        }

        // Tüm kategoriler ve alt kategorilerle birlikte getirme işlemi
        public async Task<IDataResult<List<CategoryDTO>>> GetCategoriesWithSubCategoriesAsync()
        {
            var categories = await _categoryRepository.GetAllAsync(c => c.ParentCategoryId == null);

            var categoryList = categories.Select(category => new CategoryDTO
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                SubCategories = category.SubCategories.Select(sub => new CategoryDTO
                {
                    Id = sub.Id,
                    Name = sub.Name,
                    Description = sub.Description
                }).ToList(),
                Products = category.Products.Select(product => new ProductDTO
                {
                    Id = product.Id,
                    Name = product.Name,
                    Price = product.Price
                }).ToList()
            }).ToList();

            return new SuccessDataResult<List<CategoryDTO>>(categoryList, "Kategoriler başarıyla getirildi.");
        }
    }
}
