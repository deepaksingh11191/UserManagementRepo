using System.ComponentModel.DataAnnotations;

namespace UserManagement.RestAPI.Models.Domain;

public class UserRole
{
    [Required]
    public int UserID { get; set; }

    [Required]
    public int RoleID { get; set; }

    [Required]
    public DateTime AssignedAt { get; set; }

    // Navigation
    public User User { get; set; } = null!;
    public Role Role { get; set; } = null!;
}
