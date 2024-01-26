using ApiHost.Entities;
using ApiSharedLib.Services;
using ApiSharedLib.VideoRequests;
using Microsoft.FluentUI.AspNetCore.Components;
using System.Collections;

namespace TEST_ApiHost.Lib
{
    public class VideoSvcRepoMock : IVideoServiceRepository
    {
        public async Task<Video?> GetVideoAsync(Guid videoId)
        {
            return await Task.FromResult(new Video(string.Empty)
            {
                Id = videoId,
                Path = string.Empty
            });
        }

        public async Task<IEnumerable<VideoDto>> GetVideosAsync()
        {
            return await Task.FromResult(new List<VideoDto>());
        }

        public async Task<GridItemsProviderResult<VideoDto>> GetVideosProviderResultAsync()
        {
            return await Task.FromResult(new GridItemsProviderResult<VideoDto>());
        }
    }
}

/*
[BddfyFact]
void VideoServiceReqeustResolves()
{
    string
        GVN = "",
        WHN = "",
        THN = "Then Request Resolves";

    void gvn() { }
    void whn() { }
    void thn() { }

    this
        .Given(gvn, GVN)
        .When(whn, WHN)
        .Then(thn, THN)
        .BDDfy();
}
*/