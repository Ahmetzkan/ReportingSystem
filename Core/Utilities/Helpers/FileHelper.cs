using Core.Business.Rules;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Core.Utilities.Helpers
{
    public class FileHelper : IFileHelper
    {
        private readonly FileBusinessRules _fileBusinessRules;

        public FileHelper(FileBusinessRules fileBusinessRules)
        {
            _fileBusinessRules = fileBusinessRules;
        }

        public async Task<string> Add(IFormFile file)
        {
            string destinationFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "Documents") + Path.DirectorySeparatorChar;

            await _fileBusinessRules.CheckFileExtension(Path.GetExtension(file.FileName));

            if (!Directory.Exists(destinationFolderPath)){ Directory.CreateDirectory(destinationFolderPath); }

            string newFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            return await CreateFile(file, destinationFolderPath + newFileName);
        }

        public async Task Delete(string deletedFilePath)
        {
            await _fileBusinessRules.CheckFileExist(deletedFilePath);
            File.Delete(deletedFilePath);
        }

        public async Task Update(IFormFile file, string oldFilePath)
        {
            await _fileBusinessRules.CheckFileExist(oldFilePath);
            await Delete(oldFilePath); 
            string newFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName); 
            string newFilePath = Path.Combine(Path.GetDirectoryName(oldFilePath), newFileName);
            await CreateFile(file, newFilePath); 
        }
        private async Task<string> CreateFile(IFormFile file, string destinationFilePath)
        {
            using (FileStream fileStream = File.Create(destinationFilePath))
            {
                await file.CopyToAsync(fileStream); 
                await fileStream.FlushAsync();
                return destinationFilePath;
            }
        }
    }
}
