using System.ComponentModel.DataAnnotations;

namespace UserManagement.RestAPI.Models.Domain;

public class User
{
    [Key]
    public int UserID { get; set; }

    [Required, MaxLength(50)]
    public string Username { get; set; } = null!;

    [Required, MaxLength(100), EmailAddress]
    public string Email { get; set; } = null!;

    [Required, MaxLength(255)]
    public string PasswordHash { get; set; } = null!;

    [Required]
    public bool IsActive { get; set; } = true;

    [Required]
    public DateTime CreatedAt { get; set; }

    // Navigation
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
