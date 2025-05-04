namespace QrCodeGenerator.Contracts;

public interface IStoragePort
{
    Task<string> UploadFileAsync(Stream fileData, string fileName, string contentType);
}