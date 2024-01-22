using ApiHost.DbContexts;
using ApiHost.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApiHost.Services
{
    public class VideoServiceRepository(VideoContext videoContext) : IVideoServiceRepository
    {
        private VideoContext _context { get; } = videoContext;

        public async Task<Video> GetVideoAsync(Guid videoId)
        {
            if (videoId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(videoId));
            }

#pragma warning disable CS8603 // Possible null reference return.
            return await _context.Videos.FirstOrDefaultAsync(a => a.Id == videoId);
#pragma warning restore CS8603 // Possible null reference return.
        }
    }
}
