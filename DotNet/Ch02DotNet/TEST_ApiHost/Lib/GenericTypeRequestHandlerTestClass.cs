using ApiSharedLib;
using MediatR;
using System.Reflection;

namespace TEST_ApiHost.Lib
{
    public abstract class GenericTypeRequestHandlerTestClass<TRequest> : IGenericTypeRequestHandlerTestClass<TRequest>
        where TRequest : IBaseRequest
    {
        public bool IsIRequest { get; }


        public bool IsIRequestT { get; }

        public bool IsIBaseRequest { get; }

        public GenericTypeRequestHandlerTestClass()
        {
            IsIRequest = typeof(IRequest).IsAssignableFrom(typeof(TRequest));
            IsIRequestT = typeof(TRequest).GetInterfaces()
                .Any(x => x.GetTypeInfo().IsGenericType &&
                          x.GetGenericTypeDefinition() == typeof(IRequest<>));

            IsIBaseRequest = typeof(IBaseRequest).IsAssignableFrom(typeof(TRequest));
        }

        public Type[] Handle(TRequest request)
        {
            return typeof(TRequest).GetInterfaces();
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