namespace Evoltis.Helpers
{
    public interface ICrypto
    {
        string EncryptText(string inputText);
        string DecryptText(string inputText);
    }
}
