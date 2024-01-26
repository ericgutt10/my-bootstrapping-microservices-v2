using ApiSharedLib;
using ApiSharedLib.VideoRequests;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FluentUI.AspNetCore.Components;

namespace ApiHost.Controllers
{
    [Route("api/videos")]
    [ApiController]
    public class VideoController(
        IMediator mediator,
        IHostEnvironment env
            ) : ControllerBase
    {
        public IMediator Mediator { get; } = mediator;
        public IHostEnvironment Env { get; } = env;

        [HttpGet("{videoId}")]
        public async Task<ActionResult> GetVideo(Guid videoId)
        {
            return await Mediator.Send(
                new VideoRequest(videoId, Env.ContentRootPath));
        }

        [HttpGet]
        public async Task<IEnumerable<VideoDto>> GetVideos()
        {
            return await Mediator.Send(
                new VideosRequest());
        }
    }
}