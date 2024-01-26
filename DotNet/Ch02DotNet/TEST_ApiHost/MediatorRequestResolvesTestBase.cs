using Lamar;
using MediatR;
using ApiSharedLib.Services;
using TEST_ApiHost.Lib;
using FluentAssertions;
using ApiSharedLib.VideoRequests;
using TestStack.BDDfy.Configuration;
using TestStack.BDDfy.Reporters.Html;
using Humanizer.Inflections;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace TEST_ApiHost;

public class MediatorRequestResolvesTestBase
{
    private const string RelativePath = @"..\..\Results";
    protected readonly IMediator _mediator;


    static MediatorRequestResolvesTestBase()
    {
        //Configurator.BatchProcessors.HtmlReport.Disable();
        //Configurator.BatchProcessors.MarkDownReport.Enable();
        //Configurator.BatchProcessors.DiagnosticsReport.Enable();

        var outputPath = AssemblyPathUtilities.GetResultsDirectory(RelativePath);
        var reportOutputName = $"{outputPath}\\{nameof(TEST_ApiHost)}.html";

        Configurator.BatchProcessors.Add(new CustomDiagnosticsReport
        {
            OutputPath = AssemblyPathUtilities.GetResultsDirectory(outputPath)
        });
        Configurator.BatchProcessors.Add(new HtmlReporter(
            new CustomHtmlReportConfig
            {
                ReportDescription = "BDD Results Report",
                ReportHeader = "Report Header",
                OutputFileName = "Report.html",
                OutputPath = outputPath
            }, new CustomHtmlReportBuilder()));
    }

    public MediatorRequestResolvesTestBase()
    {
        var container = new Container(cfg =>
        {
            cfg.Scan(scanner =>
            {
                scanner.AssemblyContainingType(typeof(VideoRequest));
                scanner.IncludeNamespaceContainingType<VideoRequest>();
                scanner.WithDefaultConventions();
                scanner.AddAllTypesOf(typeof(IRequestHandler<,>));
                scanner.AddAllTypesOf(typeof(IRequestHandler<>));
            });
            cfg.For<IVideoServiceRepository>().Use<VideoSvcRepoMock>();
            cfg.For<IMediator>().Use<Mediator>();
        });

        _mediator = container.GetInstance<IMediator>();
        _mediator.Should().NotBeNull();
    }
}

public static class MediatorRequestResolvesTestBaseExtensions
{
    public static string LogicRepl(this string value)
    {
        var tokens = value.Split(' ').ToList();
        var sb = new StringBuilder();

        tokens.ForEach(t =>
        {
            sb.Append($"{t.LogicTokenRepl()}");
        });

        var res = sb.Replace("||", " ").Replace("|", "").Replace("~", "").ToString();
        return res;
    }

    public static string LogicTokenRepl(this string value)
    {
        return value switch
        {
            "lt" => "|<~",
            "gt" => "~>|",
            _ => $"|{value}|"
        };
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