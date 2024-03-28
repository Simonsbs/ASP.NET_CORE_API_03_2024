using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace HelloWorld.Controllers;

[ApiController]
[Route("api/files")]
[Authorize]
public class FilesController : ControllerBase {
    FileExtensionContentTypeProvider _extProvider;
    public FilesController(FileExtensionContentTypeProvider extProvider) {
        _extProvider = extProvider ?? throw new ArgumentNullException(nameof(extProvider));
    }

    [HttpGet("{fileID}")]
    public ActionResult GetFile(int fileID) {
        string path = string.Empty;
        switch (fileID) {
            case 1:
                path = "dummy.pdf";
                break;
            case 2:
                path = "Placeholder_Person.jpg";
                break;
        }

        if (string.IsNullOrEmpty(path)) {
            return NotFound();
        }

        if(!_extProvider.TryGetContentType(path, out string contentType)) {
            contentType = "application/octet-stream";
        }

        byte[] data = System.IO.File.ReadAllBytes(path);
        return File(data, contentType, Path.GetFileName(path));
    }

    [HttpPost]
    public ActionResult CreateFile(IFormFile file) {
        if (file.Length > 200000 || file.ContentType != "application/pdf") {
            return BadRequest("File too big or not a pdf");
        }

        string filename = $"Upload_{Guid.NewGuid()}.pdf";
        string path = Path.Combine(Directory.GetCurrentDirectory(), filename);

        using (FileStream fs = new FileStream(path, FileMode.Create)) {
            file.CopyTo(fs);
        }

        return Ok("File was uploaded as: " + filename);
    }
}