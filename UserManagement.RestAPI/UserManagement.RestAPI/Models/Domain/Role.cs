using System.ComponentModel.DataAnnotations;

namespace UserManagement.RestAPI.Models.Domain;

public class Role
{
    [Key]
    public int RoleID { get; set; }

    [Required, MaxLength(50)]
    public string RoleName { get; set; } = null!;

    [MaxLength(255)]
    public string? Description { get; set; }

    // Navigation
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
}
