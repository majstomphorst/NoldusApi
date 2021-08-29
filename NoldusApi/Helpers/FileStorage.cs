using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace NoldusApi.Helpers
{
    public static class FileStorage
    {
        static string _filePath = Directory.GetCurrentDirectory() + "/AppData/";
        
        public static async Task<string> StoreImage(IFormFile file)
        {
            MkdirIfNotExists();
            var randomFileName = Path.GetRandomFileName() + ".png";
            var path = Path.Combine(_filePath, randomFileName);

            using (var stream = System.IO.File.Create(path))
            {
                await file.CopyToAsync(stream);
            }
            
            return randomFileName;
        }

        public static void RmFileIfExists(string fileName)
        {
            if (File.Exists(_filePath + fileName))
            {
                File.Delete(_filePath + fileName);
            } 
        }

        private static void MkdirIfNotExists()
        {
            if (!Directory.Exists(_filePath))
            {
                Directory.CreateDirectory(_filePath);
            }
        }



    }
}