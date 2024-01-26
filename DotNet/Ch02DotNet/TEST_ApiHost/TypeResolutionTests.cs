using TestStack.BDDfy;
using TestStack.BDDfy.Xunit;

namespace TEST_ApiHost;

public class TypeResolutionTests
{
    [BddfyFact]
    private void ShouldResolveRequestHandlers()
    {
        string
            GVN = "Given",
            WHN = "When ",
            THN = "Then";

        void whn() { }
        void thn() { }

        this
            .Given(GVN)
            .When(whn, WHN)
            .Then(thn, THN)
            .BDDfy();
    }
}