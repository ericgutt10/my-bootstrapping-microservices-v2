using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ApiSharedLib.VideoRequests;

public record VideoRequest(Guid VideoId, string EnvRootPath)
    : IRequest<ActionResult>
{
    public VideoRequest()
        : this(Guid.Empty, string.Empty)
    {
    }
}
