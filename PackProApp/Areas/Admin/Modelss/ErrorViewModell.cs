

namespace PackProApp.Areas.Admin.Modelss
{
    public class ErrorViewModell
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
