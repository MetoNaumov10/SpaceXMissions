using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public required string FirstName { get; set; }

        [Required]
        [MaxLength(100)]
        public required string LastName { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(255)]
        public required string Email { get; set; }

        [Required]
        [MaxLength(512)]
        public required byte[] PasswordHash { get; set; }

        [Required]
        [MaxLength(512)]
        public required byte[] PasswordSalt { get; set; }

        [Required]
        public DateTime DateCreated { get; set; }
    }
}
