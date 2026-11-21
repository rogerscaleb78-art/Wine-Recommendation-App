using System.ComponentModel.DataAnnotations;

namespace GroupProject421_521.Models
{
    /// <summary>
    /// Application user profile, separate from the ASP.NET Identity account.
    /// The <see cref="IsAdmin"/> flag controls access to the admin page.
    /// </summary>
    public class Users
    {
        [Key]
        public int UserID { get; set; }

        public string Username { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

        public bool? IsAdmin { get; set; }

        public DateTime? CreatedAt { get; set; }
    }
}
