namespace PackProApp.Services.ImagesServices
{
    public interface IImageService
    {
       Task<string> GetImagePath(Guid productId);
    }
}
