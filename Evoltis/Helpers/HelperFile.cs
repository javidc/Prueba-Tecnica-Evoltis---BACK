namespace Evoltis.Helpers
{
    public class HelperFile: IHelperFile
    {
        private readonly IWebHostEnvironment env;
        private readonly string nameFolderImages = "Images";
        private readonly string pathRoot;

        public HelperFile(IWebHostEnvironment env)
        {
            this.env = env;
            this.pathRoot = env.WebRootPath;
        }

        public async Task<bool> Upload(string path, string nameFile, IFormFile file)
        {
            bool result = false;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            if (file.Length > 0)
            {
                string pathFull = Path.Combine(path, nameFile);

                using (FileStream stream = new FileStream(pathFull, FileMode.Create))
                {
                    await file.CopyToAsync(stream);

                    result = true;
                }

            }

            return result;
        }

        public FileStream GetFile(string path, string nameFile)
        {
            string pathFile = Path.Combine(path, nameFile);
            if (!System.IO.File.Exists(pathFile))
            {
                return null;
            }

            FileStream file = System.IO.File.OpenRead(pathFile);
            return file;
        }

        public bool DeleteFile(string path, string nameFile)
        {
            bool result = false;
            string pathFull = Path.Combine(path, nameFile);
            if (System.IO.File.Exists(pathFull))
            {
                System.IO.File.Delete(pathFull);
                result = true;
            }
            return result;
        }

        public string GetPathClubImage()
        {
            return Path.Combine(pathRoot, nameFolderImages, "Club");
        }

        public byte[] ConvertFileToArrayByte(string path, string nameFile)
        {
            string pathFile = Path.Combine(path, nameFile);
            if (!System.IO.File.Exists(pathFile))
            {
                return null;
            }

            return System.IO.File.ReadAllBytes(pathFile);
        }

        public byte[] ConvertIFormFileToArrayByte(IFormFile file)
        {
            byte[] data = null;

            using (var ms = new MemoryStream())
            {
                file.CopyTo(ms);
                data = ms.ToArray();
            }

            return data;
        }

        public string GetFileName(IFormFile file)
        {
            string extension = Path.GetExtension(file.FileName).ToLower();
            Guid guid = Guid.NewGuid();
            string fileName = $"{guid.ToString()}{extension}";

            return fileName;
        }

                public bool ReplaceFileDecrypt(string path, string currentFilename, string newFilename, byte[] file)
        {
            bool delete = DeleteFile(path, currentFilename);

            if (!delete)
            {
                return false;
            }

            string pathFull = Path.Combine(path, newFilename);

            try
            {
                using (BinaryWriter writer = new BinaryWriter(System.IO.File.OpenWrite(pathFull)))
                {
                    writer.Write(file);
                    writer.Flush();
                    writer.Close();
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;

        }
    }
}
