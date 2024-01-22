using ApiHost.Entities;

namespace ApiHost.Services
{
    public interface IVideoServiceRepository
    {
        Task<Video> GetVideoAsync(Guid videoId);
    }
}