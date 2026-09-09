using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace techXpress.UI.ActionRequests
{
    public class UpdateRoleActionRequest
    {
        public Guid Id { get; set; }
        [Required]
        public string RoleName { get; set; }

    }

    public static class UpdateRoleActionRequestExtenstions
    {
        public static UpdateRoleActionRequest ToActionRequest(this IdentityRole<Guid> role)
        {
            return new UpdateRoleActionRequest
            {
                Id = role.Id,
                RoleName = role.Name
            };
        }
    }
}
