
namespace PackProApp.Services.ImagesServices
{
    public class ImageService : IImageService
    {
        public Task<string> GetImagePath(Guid productId)
        {
            string imagePath = Path.Combine("~/uploads/products/", productId.ToString() + ".jpg");
            return Task.FromResult(imagePath);
        }
    }
}
