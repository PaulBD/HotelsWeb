namespace library.common
{
    public interface IEncryptionService
    {
        string EncryptText(string input, string password);
        string DecryptText(string input, string password);
    }
}
