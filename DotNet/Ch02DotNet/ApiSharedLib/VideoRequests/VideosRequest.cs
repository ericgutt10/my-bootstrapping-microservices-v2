using MediatR;
using Microsoft.FluentUI.AspNetCore.Components;

namespace ApiSharedLib.VideoRequests;

public record VideosRequest : IRequest<IEnumerable<VideoDto>>
{
}