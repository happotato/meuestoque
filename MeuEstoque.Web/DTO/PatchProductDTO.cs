using System.ComponentModel.DataAnnotations;

namespace MeuEstoque.Web.DTO;

public struct PatchProductDTO
{
    [Required]
    public string Name { get; set; }

    [Required]
    public string Description { get; set; }

    public string ImageUrl { get; set; }

    [Required]
    public decimal Price { get; set; }
}
