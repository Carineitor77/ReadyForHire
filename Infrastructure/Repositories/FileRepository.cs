using System.Net;
using Amazon.S3;
using Amazon.S3.Model;
using Application.File.Interfaces;
using Domain.Errors;
using Domain.Exceptions;
using Domain.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Infrastructure.Repositories;

public class FileRepository : IFileRepository
{
    private readonly AmazonS3Client _amazonS3Client;
    private readonly AWSSettings _awsSettings;

    public FileRepository(AmazonS3Client amazonS3Client, IOptions<AWSSettings> awsSettings)
    {
        _amazonS3Client = amazonS3Client;
        _awsSettings = awsSettings.Value;
    }
    
    public async Task<bool> LoadFile(IFormFile file, string key, CancellationToken cancellationToken)
    {
        try
        {
            await using var stream = file.OpenReadStream();
            var putRequest = GetPutObjectRequest(file, key, stream);
            var putObjectResponse = await _amazonS3Client.PutObjectAsync(putRequest, cancellationToken);
            return HandlePutObjectResponse(putObjectResponse);
        }
        catch (Exception e)
        {
            throw new ApiException(HttpStatusCode.BadRequest, e.Message);
        }
    }

    public async Task<bool> LoadFile(Stream fileStream, string contentType, string key, CancellationToken cancellationToken)
    {
        try
        {
            var putRequest = GetPutObjectRequest(fileStream, contentType, key);
            var putObjectResponse = await _amazonS3Client.PutObjectAsync(putRequest, cancellationToken);
            return HandlePutObjectResponse(putObjectResponse);
        }
        catch (Exception e)
        {
            throw new ApiException(HttpStatusCode.BadRequest, e.Message);
        }
    }

    public async Task<Stream> DownloadFile(string key, CancellationToken cancellationToken)
    {
        try
        {
            var getObjectRequest = GetGetObjectRequest(key);
            using var getObjectResponse = await _amazonS3Client.GetObjectAsync(getObjectRequest, cancellationToken);
            return await HandleGetObjectResponse(cancellationToken, getObjectResponse);
        }
        catch (Exception e)
        {
            throw new ApiException(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    private static async Task<Stream> HandleGetObjectResponse(CancellationToken cancellationToken, GetObjectResponse getObjectResponse)
    {
        if (getObjectResponse.HttpStatusCode is HttpStatusCode.OK)
        {
            var responseStream = new MemoryStream();
            await getObjectResponse.ResponseStream.CopyToAsync(responseStream, cancellationToken);
            responseStream.Position = 0;
            return responseStream;
        }

        throw new ApiException(HttpStatusCode.NotFound, Errors.File.FailedToDownloadFile);
    }

    private GetObjectRequest GetGetObjectRequest(string key)
        => new()
        {
            BucketName = _awsSettings.Bucket.Name,
            Key = key
        };

    private PutObjectRequest GetPutObjectRequest(IFormFile file, string key, Stream stream)
        => new()
        {
            BucketName = _awsSettings.Bucket.Name,
            Key = key,
            InputStream = stream,
            ContentType = file.ContentType
        };
    
    private PutObjectRequest GetPutObjectRequest(Stream fileStream, string contentType, string key)
        => new()
        {
            BucketName = _awsSettings.Bucket.Name,
            Key = key,
            InputStream = fileStream,
            ContentType = contentType
        };
    
    private static bool HandlePutObjectResponse(PutObjectResponse putObjectResponse)
    {
        if (putObjectResponse?.HttpStatusCode is HttpStatusCode.OK)
        {
            return true;
        }

        throw new ApiException(HttpStatusCode.BadRequest, Errors.File.FailedToLoadFile);
    }
}