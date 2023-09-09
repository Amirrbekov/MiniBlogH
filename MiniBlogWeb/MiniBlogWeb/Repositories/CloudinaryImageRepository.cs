using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace MiniBlogWeb.Repositories;

public class CloudinaryImageRepository : IImageRepository
{
    private readonly IConfiguration _configuration;
    private readonly Account account;

    public CloudinaryImageRepository(IConfiguration configuration)
    {
        _configuration = configuration;
        account = new Account
            (
                configuration.GetSection("Cloudinary")["CloudName"],
                configuration.GetSection("Cloudinary")["ApiKey"],
                configuration.GetSection("Cloudinary")["ApiSecret"]
            );
    }

    public async Task<string> UploadAsync(IFormFile file)
    {
        Cloudinary client = new Cloudinary(account);

        ImageUploadParams uploadParams = new()
        {
            File = new FileDescription(file.FileName, file.OpenReadStream()),
            DisplayName = file.Name,
        };

        var uploadResult = await client.UploadAsync(uploadParams);

        if (uploadResult != null && uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
        {
            return uploadResult.SecureUrl.ToString();
        }

        return null;
    }
}
