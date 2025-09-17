using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace GroupProject421_521.Models
{
    public class Users
    {
        [Key]
        public int UserID { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public bool? IsAdmin { get; set; }
        public DateTime? CreatedAt { get; set; }
 
        // public List<UserPrompt> UserPrompts { get; set; } = new List<UserPrompt>();
    }
}
