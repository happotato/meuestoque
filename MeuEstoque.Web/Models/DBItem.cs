using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;

namespace MeuEstoque.Models
{
    public abstract class DBItem
    {
        [Key]
        public string Id { get; set; } = GenerateUUID();

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Required]
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public static string GenerateUUID()
        {
            var bytes = new byte[6];

            using (var crypto = new RNGCryptoServiceProvider())
                crypto.GetBytes(bytes);

            return Uri.EscapeUriString(Convert.ToBase64String(bytes));
        }
    }
}