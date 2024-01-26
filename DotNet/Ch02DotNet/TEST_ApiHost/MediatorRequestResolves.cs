using ApiSharedLib.VideoRequests;
using FluentAssertions;
using Humanizer;
using Humanizer.Inflections;
using Lamar.IoC.Resolvers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FluentUI.AspNetCore.Components;
using TEST_ApiHost.Lib;
using TestStack.BDDfy;
using TestStack.BDDfy.Xunit;
using Xunit;

namespace TEST_ApiHost;

[Story(
    TitlePrefix = "",
    Title = "Mediator Request Resolution",
    AsA = "As a mediator container",
    IWant = "I want to handle requests",
    SoThat = "So that the correct implementation occurs")]
public class MediatorRequestResolvesTest : MediatorRequestResolvesTestBase
{
    private class VideoRequestGenericRequestHandler : GenericTypeRequestHandlerTestClass<VideoRequest>
    {
        public const string Title = "VideoRequest resolves to IRequest<ActionResult>";
    }

    [BddfyFact(DisplayName = VideoRequestGenericRequestHandler.Title)]
    private void VideoRequest_resolves_to_IRequest_lt_ActionResult_gt()
    {
        string
            GVN = "Given a generic VideoRequest request handler",
            WHN = "When the generic request is handled and IRequest<ActionResult> type is resolved",
            THN = $"Then mediator calls {nameof(VideoRequestHandler)} correctly";

        VideoRequestGenericRequestHandler? videoReqGenericHandler = new();
        VideoRequest request = new();

        void whn()
        {
            videoReqGenericHandler?.Handle(request)
                .Should().Contain(typeof(IRequest<ActionResult>));
        }
        async void thn()
        {
            (await _mediator.Send(request))
                .Should().BeOfType<NotFoundResult>();
        }

        this
            .Given(GVN)
            .When(whn, WHN)
            .Then(thn, THN)
            .BDDfy(VideoRequestGenericRequestHandler.Title);
    }

    private class VideosRequestGenericRequestHandler : GenericTypeRequestHandlerTestClass<VideosRequest>
    {
        public const string Title = "VideosRequest resolves to IEnumerable<VideoDto>";
    }
    [BddfyFact(DisplayName = VideosRequestGenericRequestHandler.Title)]
    private void VideosRequest_Resolves_to_IEnumerable_lt_VideoDto_gt()
    {
        string
            GVN = "Given a generic VideosRequest request handler",
            WHN = "When the generic request is handled and IEnumerable<VideoDto> type is resolved",
            THN = $"Then mediator calls {nameof(VideosRequestHandler)} correctly";

        VideosRequestGenericRequestHandler? videosReqGenericHandler = new();
        VideosRequest request = new();

        void whn()
        {
            videosReqGenericHandler?.Handle(request)
                .Should().Contain(typeof(IRequest<IEnumerable<VideoDto>>));
        }
        async void thn()
        {
            (await _mediator.Send(request))
                .Should().BeOfType<List<VideoDto>>();
        }

        this
            .Given(GVN)
            .When(whn, WHN)
            .Then(thn, THN)
            .BDDfy(VideosRequestGenericRequestHandler.Title);
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