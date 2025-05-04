namespace QrCodeGenerator.DTO;

public class S3Options
{
    public string? BucketName { get; set; } = string.Empty;
    public string? Region { get; set; } = string.Empty;
    public static string SectionName = nameof(S3Options);
}