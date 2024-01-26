using ApiSharedLib.Lib;
using ApiSharedLib.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ApiSharedLib.VideoRequests;

public class VideoRequestHandler(IVideoServiceRepository videoServiceRepository)
    : IRequestHandler<VideoRequest, ActionResult>
{
    public IVideoServiceRepository VideoServiceRepository { get; } = videoServiceRepository;

    async Task<ActionResult> IRequestHandler<VideoRequest, ActionResult>
        .Handle(VideoRequest request, CancellationToken cancellationToken)
    {
        // get author from repo
        var videoFromRepo = await VideoServiceRepository.GetVideoAsync(request.VideoId);

        if (videoFromRepo is not null
            && request.EnvRootPath is not null
            && videoFromRepo.Path is not null)
        {
            var path = Path.Combine(
                request.EnvRootPath,
                videoFromRepo.Path!
                );

            if (!path.Equals(string.Empty, StringComparison.Ordinal) && new FileInfo(path).Exists)
            {
                return new VideoStreamResult(
                    new FileStream(path, FileMode.Open, FileAccess.Read),
                    "video/mp4");
            }
        }

        return new NotFoundResult();
    }
}