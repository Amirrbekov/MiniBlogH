namespace MiniBlogWeb.Repositories;

public interface IImageRepository
{
    Task<string> UploadAsync(IFormFile file);
}
