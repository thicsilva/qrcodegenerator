using Microsoft.AspNetCore.Mvc;
using QrCodeGenerator.Contracts;
using QrCodeGenerator.DTO;

namespace QrCodeGenerator.Controllers;

[Route("qrcode")]
public class QrCodeController(IQrCodeGeneratorService service) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Generate([FromBody]QrCodeGenerateRequest body)
    {
        try
        {
            var result = await service.GenerateAndUploadQrCodeAsync(body.Text);
            return Ok(result);
        }
        catch
        {
            return Problem();
        }
       
    }
    
    [HttpPost("local")]
    public async Task<IActionResult> GenerateLocal([FromBody]QrCodeGenerateRequest body)
    {
        try
        {
            var result = await service.GenerateLocalQrCodeAsync(body.Text);
            return File(result, "image/png");
        }
        catch
        {
            return Problem();
        }
       
    }
}