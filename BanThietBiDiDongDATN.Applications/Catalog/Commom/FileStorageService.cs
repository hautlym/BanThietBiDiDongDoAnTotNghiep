using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanThietBiDiDongDATN.Application.Catalog.Commom
{
    public class FileStorageService : IStorageService
    {
        private readonly string _userContentFolder;
        private const string USER_CONTENT_FOLDER_NAME = "user-content";
        public FileStorageService(IWebHostEnvironment webHostEnvironment)
        {
            _userContentFolder = Path.Combine(webHostEnvironment.WebRootPath, USER_CONTENT_FOLDER_NAME);
        }
        public async Task DeleteFileAsync(string folder, string fileName)
        {
            var filePath = Path.Combine(_userContentFolder, folder, fileName);
            if (File.Exists(filePath))
            {
                await Task.Run(() => File.Delete(filePath));
            }
        }

        public string GetFileUrl(string folder, string fileName)
        {
            return $"/{USER_CONTENT_FOLDER_NAME}/{folder}/{fileName}";
        }

        public async Task SaveFileAsync(Stream mediaBinaryStream, string folder, string fileName)
        {
            var checkFolder = Directory.Exists(Path.Combine(_userContentFolder, folder));
            if(!checkFolder)
            {
                Directory.CreateDirectory(Path.Combine(_userContentFolder, folder));
            }
            var filePath = Path.Combine(_userContentFolder, folder, fileName);
            using var output = new FileStream(filePath, FileMode.Create);
            await mediaBinaryStream.CopyToAsync(output);
        }
    }
}
