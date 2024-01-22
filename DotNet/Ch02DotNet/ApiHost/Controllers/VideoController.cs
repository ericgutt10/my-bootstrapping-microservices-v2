using ApiHost.Entities;
using ApiHost.Lib;
using ApiHost.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiHost.Controllers
{
    [Route("api/videos")]
    [ApiController]
    public class VideoController : ControllerBase
    {
        public VideoController(IVideoServiceRepository videoServiceRepository, IHostEnvironment env)
        {
            VideoServiceRepository = videoServiceRepository;
            Env = env;
        }

        public IVideoServiceRepository VideoServiceRepository { get; }
        public IHostEnvironment Env { get; }

        [HttpGet("{videoId}", Name = "GetVideo")]
        public async Task<ActionResult> GetVideo(Guid videoId)
        {
            // get author from repo
            var videoFromRepo = await VideoServiceRepository.GetVideoAsync(videoId);

            if (videoFromRepo != null)
            {
                var path = Path.Combine(
                    Env.ContentRootPath,
                    videoFromRepo.Path!
                    );

                if (new FileInfo(path).Exists)
                {
                    var inputStream = new FileStream(path, FileMode.Open, FileAccess.Read);

                    return new VideoStreamResult(inputStream, "video/mp4");
                }
            }

            return NotFound();
        }
    }
}