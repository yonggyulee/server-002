using Microsoft.AspNetCore.Mvc;

namespace Mirero.DAQ.Service.Api.Offline.Controllers;

[Route("/api/v1/files")]
[ApiController]
public class FileStoreController : ControllerBase
{
    [HttpGet("{volume}/{*path}")]
    public IActionResult Get(string volume, string path)
    {
        // return Ok(new
        // {
        //     Volume = volume,
        //     Path = path
        // });
        const string baseDir = "c:/temp";
        // var fullPath = Path.Combine(baseDir, $"./volume", path);
        var fullPath = $"{baseDir}/{volume}/{path}";
        var name = Path.GetFileName(path);
        // return Ok(new
        // {
        //     Path = path,
        //     Name = name,
        //     Volume= volume,
        // });
        var stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.Read);
        return new FileStreamResult(stream, "application/octet-stream"); // application/octet-stream
    }
}