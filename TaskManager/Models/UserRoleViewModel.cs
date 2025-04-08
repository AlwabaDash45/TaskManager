namespace TaskManager.Models
{
    public class UserRoleViewModel
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string CellPhone { get; set; }
        public IList<string> Roles { get; set; }
    }

}
