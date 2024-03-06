namespace Evoltis.Helpers
{
    public interface IHelperFile
    {
        byte[] ConvertFileToArrayByte(string path, string nameFile);
        byte[] ConvertIFormFileToArrayByte(IFormFile file);
        bool DeleteFile(string path, string nameFile);
        FileStream GetFile(string path, string nameFile);
        string GetFileName(IFormFile file);
        string GetPathClubImage();
        bool ReplaceFileDecrypt(string path, string currentFilename, string newFilename, byte[] file);
        Task<bool> Upload(string path, string nameFile, IFormFile file);
    }
}
