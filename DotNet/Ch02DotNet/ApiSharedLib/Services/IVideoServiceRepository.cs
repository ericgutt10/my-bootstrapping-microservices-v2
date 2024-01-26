using ApiHost.Entities;
using ApiSharedLib.VideoRequests;
using Microsoft.FluentUI.AspNetCore.Components;

namespace ApiSharedLib.Services
{
    public interface IVideoServiceRepository
    {
        Task<Video?> GetVideoAsync(Guid videoId);

        Task<GridItemsProviderResult<VideoDto>> GetVideosProviderResultAsync();

        Task<IEnumerable<VideoDto>> GetVideosAsync();
    }
}