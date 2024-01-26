using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Net;
using System.Text;
using TestStack.BDDfy;
using TestStack.BDDfy.Configuration;
using TestStack.BDDfy.Reporters;
using TestStack.BDDfy.Reporters.Html;

namespace TEST_ApiHost.Lib;

public class LocalMetroReportBuilder : IReportBuilder
{
    private HtmlReportModel _model;

    private readonly StringBuilder _html;

    private const int TabIndentation = 2;

    private int _tabCount;

    public LocalMetroReportBuilder()
    {
        _html = new StringBuilder();
    }

    string IReportBuilder.CreateReport(FileReportModel model)
    {
        return CreateReport(model as HtmlReportModel);
    }

    public string CreateReport(HtmlReportModel model)
    {
        _model = model;
        AddLine("<!DOCTYPE html>");
        using (OpenTag(HtmlTag.html))
        {
            HtmlHead();
            HtmlBody();
        }

        return _html.ToString();
    }

    private void HtmlHead()
    {
        using (OpenTag(HtmlTag.head))
        {
            AddLine("<meta charset='utf-8'/>");
            EmbedCssFile(Properties.Resources.metro_min_css);
            AddLine("<link href='http://fonts.googleapis.com/css?family=Roboto:400,300' rel='stylesheet' type='text/css'>");
            AddLine($"<title>BDDfy Test Result {_model.RunDate.AsShortDateTimeString()}</title>");
        }
    }

    private void HtmlBody()
    {
        using (OpenTag(HtmlTag.body))
        {
            using (OpenTag("<div id='main'>", HtmlTag.div))
            {
                Header();
                ResultSummary();
                ResultDetails();
            }

            Footer();
        }
    }

    private void Header()
    {
        using (OpenTag("<section id='titles'>", HtmlTag.section))
        {
            AddLine($"<h1 class='blue'>{_model.Configuration.ReportHeader}</h1>");
            AddLine($"<h3>{_model.Configuration.ReportDescription}</h3>");
        }
    }

    private void ResultSummary()
    {
        using (OpenTag("<section id='summaryTotals' class='group'>", HtmlTag.section))
        {
            using (OpenTag("<div class='tiles'>", HtmlTag.div))
            {
                using (OpenTag("<div class='tilerow'>", HtmlTag.div))
                {
                    using (OpenTag("<div id='storycount' class='tile tileNoHover two-h purplebg'>", HtmlTag.div))
                    {
                        AddLine("<h3>stories</h3>");
                        AddLine($"<h1>{_model.Summary.Stories}</h1>");
                    }

                    using (OpenTag("<div id='scenariocount' class='tile tileNoHover two-h tealbg'>", HtmlTag.div))
                    {
                        AddLine("<h3>scenarios</h3>");
                        AddLine($"<h1>{_model.Summary.Scenarios}</h1>");
                    }
                }

                using (OpenTag("<div class='tilerow' id='filterOptions'>", HtmlTag.div))
                {
                    Tuple<string, string, int>[] array = new Tuple<string, string, int>[4]
                    {
                        Tuple.Create("Passed", "limebg", _model.Summary.Passed),
                        Tuple.Create("Failed", "redbg", _model.Summary.Failed),
                        Tuple.Create("Inconclusive", "orangebg", _model.Summary.Inconclusive),
                        Tuple.Create("NotImplemented", "bluebg", _model.Summary.NotImplemented)
                    };
                    Tuple<string, string, int>[] array2 = array;
                    foreach (Tuple<string, string, int> tuple in array2)
                    {
                        using (OpenTag("<a href='#'>", HtmlTag.a))
                        {
                            using (OpenTag($"<div class='tile one {tuple.Item2}' data-target-class='{tuple.Item1}'>", HtmlTag.div))
                            {
                                AddLine($"<h4>{tuple.Item1.ToUpperInvariant()}</h4>");
                                AddLine($"<h1>{tuple.Item3}</h1>");
                            }
                        }
                    }
                }
            }
        }
    }

    private void ExpandCollapse()
    {
        using (OpenTag("<div id='expandCollapse' class='group'>", HtmlTag.div))
        {
            AddLine("<h2 style='float: left'>results</h2>");
            AddLine("<a href='#' class='expandAll'>show steps</a>");
            AddLine("<a href='#' class='collapseAll'>hide steps</a>");
        }
    }

    private void ResultDetails()
    {
        using (OpenTag("<section id='testResults'>", HtmlTag.section))
        {
            ExpandCollapse();
            using (OpenTag("<ul class='testResult'>", HtmlTag.ul))
            {
                foreach (ReportModel.Story story in _model.Stories)
                {
                    AddStory(story);
                }
            }
        }
    }

    private void Footer()
    {
        using (OpenTag(HtmlTag.section))
        {
            AddLine($"<p>Tested at: {_model.RunDate}</p>");
            AddLine("<p>Powered by <a href='https://github.com/TestStack/TestStack.BDDfy'>BDDfy</a></p>");
        }

        if (_model.Configuration.ResolveJqueryFromCdn)
        {
            AddLine("<script type='text/javascript' src='http://code.jquery.com/jquery-2.1.0.min.js'></script>");
        }
        else
        {
            EmbedJavascriptFile(Properties.Resources.jquery_2_1_0_min_js);
        }

        EmbedJavascriptFile(Properties.Resources.metro_min_js);
    }

    private void AddStory(ReportModel.Story story)
    {
        List<ReportModel.Scenario> source = story.Scenarios.ToList();
        IEnumerable<IGrouping<string, ReportModel.Scenario>> enumerable = from s in story.Scenarios
                                                                          group s by s.Id;
        Result result = (Result)source.Max((s) => (int)s.Result);
        using (OpenTag(HtmlTag.li))
        {
            using (OpenTag($"<div class='story {result}'>", HtmlTag.div))
            {
                AddStoryMetadataAndNarrative(story);
                using (OpenTag("<div class='scenarios'>", HtmlTag.div))
                {
                    foreach (IGrouping<string, ReportModel.Scenario> item in enumerable)
                    {
                        AddScenario(item.ToArray());
                    }
                }
            }
        }
    }

    private void AddScenario(ReportModel.Scenario[] scenarioGroup)
    {
        using (OpenTag($"<div class='scenario'>", HtmlTag.div))
        {
            if (scenarioGroup.Count() == 1)
            {
                AddScenario(scenarioGroup.Single());
            }
            else
            {
                AddScenarioWithExamples(scenarioGroup);
            }
        }
    }

    private void AddScenarioWithExamples(ReportModel.Scenario[] scenarioGroup)
    {
        ReportModel.Scenario scenario = scenarioGroup.First();
        Result result = (Result)scenarioGroup.Max((s) => (int)s.Result);
        AddLine($"<div class='{result} canToggle scenarioTitle' data-toggle-target='{scenario.Id}'>{WebUtility.HtmlEncode(scenario.Title)}{FormatTags(scenario.Tags)}</div>");
        using (OpenTag($"<ul class='steps' id='{scenario.Id}'>", HtmlTag.ul))
        {
            foreach (ReportModel.Step item in scenario.Steps.Where((s) => s.ShouldReport))
            {
                using (OpenTag($"<li class='step {item.ExecutionOrder}'>", HtmlTag.li))
                {
                    string[] array = WebUtility.HtmlEncode(item.Title).Split(new string[1] { Environment.NewLine }, StringSplitOptions.None);
                    string arg = array[0];
                    AddLine($"<span>{arg}</span>");
                    for (int i = 1; i < array.Length; i++)
                    {
                        AddLine($"<div class='step-title-extra-lines'>{array[i]}</div>");
                    }
                }
            }

            AddExamples(scenarioGroup);
        }
    }

    private string FormatTags(List<string> tags)
    {
        return string.Join(string.Empty, tags.Select((t) => $"<div class='tag'>{t}</div>"));
    }

    private void AddExamples(ReportModel.Scenario[] scenarioGroup)
    {
        ReportModel.Scenario scenario = scenarioGroup.First();
        Result result = (Result)scenarioGroup.Max((s) => (int)s.Result);
        using (OpenTag("<li class='step'>", HtmlTag.li))
        {
            AddLine("<span class='example-header'>Examples:</span>");
            using (OpenTag($"<table class='examples' style='border-collapse: collapse;margin-left:10px''>", HtmlTag.table))
            {
                using (OpenTag("<tr>", HtmlTag.tr))
                {
                    AddLine($"<th></th>");
                    string[] headers = scenario.Example.Headers;
                    foreach (string arg in headers)
                    {
                        AddLine($"<th>{arg}</th>");
                    }

                    if (result == Result.Failed)
                    {
                        AddLine($"<th>Error</th>");
                    }
                }

                foreach (ReportModel.Scenario scenario2 in scenarioGroup)
                {
                    AddExampleRow(scenario2, result);
                }
            }
        }
    }

    private void AddExampleRow(ReportModel.Scenario scenario, Result scenarioResult)
    {
        using (OpenTag("<tr>", HtmlTag.tr))
        {
            AddLine($"<td><Span class='{scenario.Result}' style='margin-right:4px;' /></td>");
            foreach (ExampleValue value in scenario.Example.Values)
            {
                AddLine($"<td>{WebUtility.HtmlEncode(value.GetValueAsString())}</td>");
            }

            if (scenarioResult != Result.Failed)
            {
                return;
            }

            using (OpenTag("<td>", HtmlTag.td))
            {
                ReportModel.Step step = scenario.Steps.FirstOrDefault((s) => s.Result == Result.Failed);
                if (step == null)
                {
                    return;
                }

                string stepId = Configurator.IdGenerator.GetStepId();
                string arg = WebUtility.HtmlEncode(step.Exception.Message);
                AddLine($"<span class='canToggle' data-toggle-target='{stepId}'>{arg}</span>");
                using (OpenTag($"<div class='step FailedException' id='{stepId}'>", HtmlTag.div))
                {
                    AddLine($"<code>{step.Exception.StackTrace}</code>");
                }
            }
        }
    }

    private void AddScenario(ReportModel.Scenario scenario)
    {
        AddLine($"<div class='{scenario.Result} canToggle scenarioTitle' data-toggle-target='{scenario.Id}'>{WebUtility.HtmlEncode(scenario.Title)}{FormatTags(scenario.Tags)}</div>");
        using (OpenTag($"<ul class='steps' id='{scenario.Id}'>", HtmlTag.ul))
        {
            foreach (ReportModel.Step item in scenario.Steps.Where((s) => s.ShouldReport))
            {
                string text = string.Empty;
                bool flag = item.Exception != null && item.Result == Result.Failed;
                string text2 = flag ? "canToggle" : string.Empty;
                using (OpenTag($"<li class='step {item.Result} {text} {item.ExecutionOrder} {text2}' data-toggle-target='{item.Id}' >", HtmlTag.li))
                {
                    string[] array = item.Title.Split(new string[1] { Environment.NewLine }, StringSplitOptions.None);
                    string text3 = array[0];
                    if (flag)
                    {
                        text = string.Concat(item.Result, "Exception");
                        if (!string.IsNullOrEmpty(item.Exception.Message))
                        {
                            text3 = text3 + " [Exception Message: '" + item.Exception.Message + "']";
                        }
                    }

                    AddLine($"<span>{text3}</span>");
                    for (int i = 1; i < array.Length; i++)
                    {
                        AddLine($"<div class='step-title-extra-lines'>{array[i]}</div>");
                    }

                    if (flag)
                    {
                        using (OpenTag($"<div class='step {text}' id='{item.Id}'>", HtmlTag.div))
                        {
                            AddLine($"<code>{item.Exception.StackTrace}</code>");
                        }
                    }
                }
            }
        }
    }

    private void AddStoryMetadataAndNarrative(ReportModel.Story story)
    {
        using (OpenTag("<div class='storyMetadata'>", HtmlTag.div))
        {
            AddLine(story.Metadata == null ? $"<h3 class='namespaceName'>{story.Namespace}</h3>" : $"<h3 class='storyTitle'>{(string.IsNullOrWhiteSpace(story.Metadata.StoryUri) ? story.Metadata.TitlePrefix + story.Metadata.Title : $"<a href='{story.Metadata.StoryUri}'>{story.Metadata.TitlePrefix}{story.Metadata.Title}</a>")}</h3>");
            if (story.Metadata != null && !string.IsNullOrWhiteSpace(story.Metadata.ImageUri))
            {
                AddLine($"<img class='storyImg' src='{story.Metadata.ImageUri}' alt='Image for {story.Metadata.TitlePrefix}{story.Metadata.Title}'/>");
            }

            if (story.Metadata == null || string.IsNullOrEmpty(story.Metadata.Narrative1))
            {
                return;
            }

            using (OpenTag("<ul class='storyNarrative'>", HtmlTag.ul))
            {
                AddLine($"<li>{story.Metadata.Narrative1}</li>");
                if (!string.IsNullOrEmpty(story.Metadata.Narrative2))
                {
                    AddLine($"<li>{story.Metadata.Narrative2}</li>");
                }

                if (!string.IsNullOrEmpty(story.Metadata.Narrative3))
                {
                    AddLine($"<li>{story.Metadata.Narrative3}</li>");
                }
            }
        }
    }

    private HtmlReportTag OpenTag(string openingTag, HtmlTag tag)
    {
        AddLine(openingTag);
        _tabCount++;
        return new HtmlReportTag(tag, CloseTag);
    }

    private HtmlReportTag OpenTag(HtmlTag tag)
    {
        AddLine(string.Concat("<", tag, ">"));
        _tabCount++;
        return new HtmlReportTag(tag, CloseTag);
    }

    private void CloseTag(HtmlTag tag)
    {
        _tabCount--;
        string line = string.Concat("</", tag, ">");
        AddLine(line);
    }

    private void AddLine(string line)
    {
        int totalWidth = _tabCount * 2;
        _html.AppendLine(string.Empty.PadLeft(totalWidth) + line);
    }

    private void EmbedCssFile(string cssContent, string htmlComment = null)
    {
        using (OpenTag("<style type='text/css'>", HtmlTag.style))
        {
            AddHtmlComment(htmlComment);
            _html.AppendLine(cssContent);
        }
    }

    private void EmbedJavascriptFile(string javascriptContent, string htmlComment = null)
    {
        using (OpenTag(HtmlTag.script))
        {
            AddHtmlComment(htmlComment);
            _html.AppendLine(javascriptContent);
        }
    }

    private void AddHtmlComment(string htmlComment)
    {
        if (!string.IsNullOrWhiteSpace(htmlComment))
        {
            _html.AppendFormat("/*{0}*/", htmlComment);
            _html.AppendLine();
        }
    }

}