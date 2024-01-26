using MediatR;

namespace TEST_ApiHost.Lib
{
    public interface IGenericTypeRequestHandlerTestClass<TRequest> where TRequest : IBaseRequest
    {
        Type[] Handle(TRequest request);
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