using ApiSharedLib.Services;
using MediatR;

namespace ApiSharedLib.VideoRequests
{
    public class VideosRequestHandler(IVideoServiceRepository videoServiceRepository) : IRequestHandler<VideosRequest, IEnumerable<VideoDto>>
    {
        public IVideoServiceRepository VideoServiceRepository { get; } = videoServiceRepository;

        public async Task<IEnumerable<VideoDto>> Handle(VideosRequest request, CancellationToken cancellationToken)
        {
            return await VideoServiceRepository.GetVideosAsync();
        }
    }
}