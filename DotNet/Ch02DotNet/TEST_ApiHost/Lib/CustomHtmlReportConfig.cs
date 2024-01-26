using TestStack.BDDfy;
using TestStack.BDDfy.Reporters.Html;

namespace TEST_ApiHost.Lib;

internal class CustomHtmlReportConfig : DefaultHtmlReportConfiguration
{
    public override bool RunsOn(Story story)
    {
        return base.RunsOn(story);
    }

    public override string OutputFileName
    { get; set; }

    public override string OutputPath
    { get; set; }

    public override string ReportDescription
    { get; set; }

    public override string ReportHeader
    { get; set; }
}