using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestStack.BDDfy.Xunit;
using TestStack.BDDfy;

namespace TEST_ApiHost;

public class TypeResolutionTests
{

    [BddfyFact]
    void ShouldResolveRequestHandlers()
    {
        string
            GVN = "Given a Service Provider",
            WHN = "When ",
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
}
