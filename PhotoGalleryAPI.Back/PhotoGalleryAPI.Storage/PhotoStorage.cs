using Microsoft.AspNetCore.Http;

namespace PhotoGalleryAPI.Storage
{
    public static class PhotoStorage
    {
        private static readonly string _photosDirectory;

        static PhotoStorage()
        {
            _photosDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Photos");

            // Ensure the Photos directory exists
            if (!Directory.Exists(_photosDirectory))
            {
                Directory.CreateDirectory(_photosDirectory);
            }
        }

        public static async Task<string> UploadFileAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("File is empty or null.");
            }

            // Generate a unique file name
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(_photosDirectory, fileName);

            // Save the file to the Photos directory
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return fileName; // Return the unique file name
        }

        public static async Task<bool> DeleteFileAsync(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentException("File name is null or empty.");
            }

            var filePath = Path.Combine(_photosDirectory, fileName);

            if (!File.Exists(filePath))
            {
                return false; // File does not exist
            }

            // Delete the file
            File.Delete(filePath);
            return true;
        }

        public static string GetFilePath(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentException("File name is null or empty.");
            }

            return Path.Combine(_photosDirectory, fileName);
        }
    }
}
