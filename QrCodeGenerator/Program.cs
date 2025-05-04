using QrCodeGenerator.Contracts;
using QrCodeGenerator.DTO;
using QrCodeGenerator.Infrastructure;
using QrCodeGenerator.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<S3Options>(builder.Configuration.GetSection(S3Options.SectionName));
builder.Services.AddScoped<IStoragePort, S3StorageAdapter>();
builder.Services.AddScoped<IQrCodeGeneratorService, QrCodeGeneratorService>();
builder.Services.AddControllers();

var app = builder.Build();
app.MapControllers();
app.Run();