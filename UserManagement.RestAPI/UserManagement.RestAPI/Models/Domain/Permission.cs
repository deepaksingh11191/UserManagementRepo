using System.ComponentModel.DataAnnotations;

namespace UserManagement.RestAPI.Models.Domain;

public class Permission
{
    [Key]
    public int PermissionID { get; set; }

    [Required, MaxLength(100)]
    public string PermissionName { get; set; } = null!;

    [MaxLength(255)]
    public string? Description { get; set; }

    // Navigation
    public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
}
