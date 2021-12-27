using System.ComponentModel.DataAnnotations;
using MeuEstoque.Domain.AggregatesModel.UserAggregate;

namespace MeuEstoque.Web.DTO;

public sealed class UserDTO : EntityDTO
{
    [Required]
    public string Name { get; set; }

    [Required]
    public string Username { get; set; }

    [Required]
    public string Email { get; set; }

    public UserDTO(User user) : base(user)
    {
        Name = user.Name;
        Username = user.Username;
        Email = user.Email;
    }
}
