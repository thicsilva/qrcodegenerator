using QrCodeGenerator.DTO;

namespace QrCodeGenerator.Contracts;

public interface IQrCodeGeneratorService
{
    Task<QrCodeGenerateResponse> GenerateAndUploadQrCodeAsync(string text);
    Task<Stream> GenerateLocalQrCodeAsync(string text);
}