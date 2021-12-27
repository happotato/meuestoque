namespace MeuEstoque.Infrastructure.Cryptography;

public interface IEncypter
{
    string Encrypt(string text);

    string Decrypt(string text);
}
