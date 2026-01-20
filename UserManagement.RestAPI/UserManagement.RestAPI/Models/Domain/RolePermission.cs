using System.ComponentModel.DataAnnotations;

namespace UserManagement.RestAPI.Models.Domain;

public class RolePermission
{
    [Required]
    public int RoleID { get; set; }

    [Required]
    public int PermissionID { get; set; }

    [Required]
    public DateTime GrantedAt { get; set; }

    // Navigation
    public Role Role { get; set; } = null!;
    public Permission Permission { get; set; } = null!;
}
