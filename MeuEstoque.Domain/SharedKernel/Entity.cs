using System;
using System.Security.Cryptography;

namespace MeuEstoque.Domain.SharedKernel
{
    public abstract class Entity : IEquatable<Entity>
    {
        public string Id { get; } = GenerateUUID();
        public DateTime CreatedAt { get; } = DateTime.Now;
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