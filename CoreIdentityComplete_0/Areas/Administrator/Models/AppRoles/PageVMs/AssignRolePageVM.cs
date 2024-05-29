using CoreIdentityComplete_0.Areas.Administrator.Models.AppRoles.ResponseModels;

namespace CoreIdentityComplete_0.Areas.Administrator.Models.AppRoles.PageVMs
{
    public class AssignRolePageVM
    {
        public int UserID { get; set; }
        public List<AppRoleResponseModel> Roles { get; set; }
    }
}
