using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;

namespace MeuEstoque.Domain.SharedKernel
{
    public abstract class Entity : IEquatable<Entity>
    {
        [Required]
        public string Id { get; private set; } = GenerateUUID();

        [Required]
        public DateTime CreatedAt { get; private set; } = DateTime.Now;

        [Required]
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public bool Equals(Entity obj)
        {
            return this.Id == obj.Id;
        }

        public static string GenerateUUID()
        {
            var bytes = new byte[6];

            using (var crypto = new RNGCryptoServiceProvider())
                crypto.GetBytes(bytes);

            return Uri.EscapeUriString(Convert.ToBase64String(bytes));
        }
    }
}