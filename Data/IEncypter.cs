namespace MeuEstoque.Data
{
    public interface IEncypter
    {
        string Encrypt(string text);

        string Decrypt(string text);
    }
}
