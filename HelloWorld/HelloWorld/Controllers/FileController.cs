using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace HelloWorld.Controllers;

[ApiController]
[Route("api/files")]
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
}