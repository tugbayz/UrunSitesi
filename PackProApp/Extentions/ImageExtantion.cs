namespace PackProApp.Extentions
{
    public static class ImageExtantion
    {
        public static string ToBase64String(this byte[] bytes)
        {
            return Convert.ToBase64String(bytes);
        }
    }
}
