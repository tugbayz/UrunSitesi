namespace PackProApp.Areas.Admin.Modelss.UserVMs
{
    public class UserRolesVM
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public IList<string> Roles { get; set; }
    }
}
