using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

public class ImageHelper
{
    private readonly IWebHostEnvironment _webHostEnvironment;

    public ImageHelper(IWebHostEnvironment webHostEnvironment)
    {
        _webHostEnvironment = webHostEnvironment;
    }

    /// <summary>
    /// Saves the uploaded image to wwwroot/uploads/images directory.
    /// </summary>
    /// <param name="file">The image file to be saved</param>
    /// <param name="folderName">Optional folder name to save inside uploads (e.g., "places")</param>
    /// <returns>The path of the saved image relative to wwwroot</returns>
    public async Task<string> SaveImageAsync(IFormFile file, string folderName = "images")
    {
        if (file == null || file.Length == 0)
        {
            throw new ArgumentException("File cannot be null or empty.");
        }

        // Validate if the file is an image
        if (!IsImage(file))
        {
            throw new ArgumentException("File must be an image.");
        }

        // Create the directory if it doesn't exist
        string uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", folderName);
        if (!Directory.Exists(uploadPath))
        {
            Directory.CreateDirectory(uploadPath);
        }

        // Generate a unique file name
        string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

        // Save the file to the target directory
        string filePath = Path.Combine(uploadPath, uniqueFileName);
        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(fileStream);
        }

        // Return the relative path to the saved file (for use in the web)
        string relativePath = Path.Combine("uploads", folderName, uniqueFileName).Replace("\\", "/");
        return relativePath;
    }

    /// <summary>
    /// Returns the full path to the image file in the wwwroot folder.
    /// </summary>
    /// <param name="relativePath">The relative path to the image (e.g., uploads/tourism-places/unique-image-name.jpg)</param>
    /// <returns>The full path to the image on disk, or null if the image does not exist.</returns>
    public string GetImagePath(string relativePath)
    {
        if (string.IsNullOrEmpty(relativePath))
        {
            throw new ArgumentException("Image path cannot be null or empty.");
        }

        // Combine the wwwroot folder with the relative path
        string fullPath = Path.Combine(_webHostEnvironment.WebRootPath, relativePath.Replace("/", Path.DirectorySeparatorChar.ToString()));

        // Check if the file exists
        if (File.Exists(fullPath))
        {
            return fullPath;
        }

        return null; // Return null if the image does not exist
    }



    /// <summary>
    /// Validates if the file is an image based on the content type.
    /// </summary>
    /// <param name="file">The file to validate</param>
    /// <returns>True if the file is an image, otherwise false</returns>
    private bool IsImage(IFormFile file)
    {
        string[] imageTypes = { "image/jpeg", "image/png", "image/gif", "image/jpg" };
        return Array.Exists(imageTypes, type => file.ContentType == type);
    }
}
