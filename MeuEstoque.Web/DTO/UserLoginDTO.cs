using System.ComponentModel.DataAnnotations;

namespace MeuEstoque.Web.DTO
{
    public struct UserLoginDTO
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
