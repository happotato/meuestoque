using System.ComponentModel.DataAnnotations;

namespace MeuEstoque.Web.DTO
{
    public struct CreateUserDTO
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [MinLength(4)]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(8)]
        [MaxLength(24)]
        public string Password { get; set; }
    }
}
