using System;
using System.ComponentModel.DataAnnotations;
using MeuEstoque.Domain.SharedKernel;

namespace MeuEstoque.Web.DTO;

public abstract class EntityDTO
{
    [Required]
    public string Id { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; }

    [Required]
    public DateTime UpdatedAt { get; set; }

    public EntityDTO(Entity entity)
    {
        Id = entity.Id;
        CreatedAt = entity.CreatedAt;
        UpdatedAt = entity.UpdatedAt;
    }
}
