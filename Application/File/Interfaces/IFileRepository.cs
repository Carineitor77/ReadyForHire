using Microsoft.AspNetCore.Http;

namespace Application.File.Interfaces;

public interface IFileRepository
{
    Task<bool> LoadFile(IFormFile file, string key, CancellationToken cancellationToken);
    Task<bool> LoadFile(Stream fileStream, string contentType, string key, CancellationToken cancellationToken);
    Task<Stream> DownloadFile(string key, CancellationToken cancellationToken);
}