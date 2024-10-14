namespace PackProApp.Areas.Admin.Modelss
{
    public class AdminProfilVM
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public byte[]? AdminImage { get; set; } // Profilde gösterilecek resim
        public IFormFile? NewImage { get; set; } // Yeni resim yükleme için
    }
}
