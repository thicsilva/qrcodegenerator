using QrCodeGenerator.Contracts;
using QrCodeGenerator.DTO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using ZXing;
using ZXing.Common;

namespace QrCodeGenerator.Services;

public sealed class QrCodeGeneratorService(IStoragePort storagePort) : IQrCodeGeneratorService
{
    public async Task<QrCodeGenerateResponse> GenerateAndUploadQrCodeAsync(string text)
    {
        var ms = await GenerateMemoryStream(text);
        var url = await storagePort.UploadFileAsync(ms, Guid.NewGuid().ToString(), "image/png");
        await ms.DisposeAsync();

        return new QrCodeGenerateResponse(url);
    }

    private static async Task<MemoryStream> GenerateMemoryStream(string text)
    {
        MemoryStream? ms = null;
        try
        {
            var qrCodeWriter = new BarcodeWriterPixelData()
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new EncodingOptions() { Height = 200, Width = 200, Margin = 1 }
            };
            var pixelData = qrCodeWriter.Write(text);
            using var image = Image.LoadPixelData<Rgba32>(pixelData.Pixels, pixelData.Width, pixelData.Height);
            ms = new MemoryStream();
            await image.SaveAsync(ms, new PngEncoder());
            ms.Position = 0;
            return ms;
        }
        catch
        {
            if (ms != null)
                await ms.DisposeAsync();
            throw;
        }
    }

    public async Task<Stream> GenerateLocalQrCodeAsync(string text)
    {
        return await GenerateMemoryStream(text);
    }
}