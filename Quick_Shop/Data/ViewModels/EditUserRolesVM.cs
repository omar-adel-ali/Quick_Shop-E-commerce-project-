using System.Collections.Generic;

namespace Quick_Shop.Data.ViewModels
{
    public class EditUserRolesVM
    {
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public List<RoleSelectionVM> UserRoles { get; set; }
    }
}