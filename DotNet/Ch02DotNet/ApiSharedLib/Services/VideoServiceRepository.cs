using ApiHost.DbContexts;
using ApiHost.Entities;
using ApiSharedLib.VideoRequests;
using Microsoft.EntityFrameworkCore;
using Microsoft.FluentUI.AspNetCore.Components;

namespace ApiSharedLib.Services;

public class VideoServiceRepository(VideoContext videoContext) : IVideoServiceRepository
{
    private VideoContext _context { get; } = videoContext;

    public async Task<Video?> GetVideoAsync(Guid videoId)
    {
        if (videoId == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(videoId));
        }

        return await _context.Videos.FirstOrDefaultAsync(a => a.Id == videoId);
    }

    public async Task<GridItemsProviderResult<VideoDto>> GetVideosProviderResultAsync()
    {
        var result = _context.Videos.Select(v => new VideoDto(v.Id, v.Title, v.Path));

        return new GridItemsProviderResult<VideoDto>
        {
            Items = await result.ToListAsync(),
            TotalItemCount = await result.CountAsync()
        };            
    }

    public async Task<IEnumerable<VideoDto>> GetVideosAsync()
    {
        var result = _context.Videos.Select(v => new VideoDto(v.Id, v.Title, v.Path));

        return await result.ToListAsync();
    }
}