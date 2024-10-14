namespace PackProApp
{
    public static class FormFileExtensions
    {
        public static async Task<byte[]> StringToByteArrayAsync(this IFormFile formFile)
        {
            using MemoryStream memory = new MemoryStream();
            await formFile.CopyToAsync(memory);
            return memory.ToArray();
        }
    }
}
