using Newtonsoft.Json;
using TestStack.BDDfy;
using TestStack.BDDfy.Reporters;
using TestStack.BDDfy.Reporters.Diagnostics;
using TestStack.BDDfy.Reporters.Serializers;

namespace TEST_ApiHost.Lib;

public class CustomDiagnosticsReport : IBatchProcessor
{
    public CustomDiagnosticsReport()
    { }

    public CustomDiagnosticsReport(string outputPath, string fileName = null)
    {
        OutputPath = AssemblyPathUtilities.GetResultsDirectory(outputPath);
        FileName = fileName;
    }

    public string FileName { get; set; }

    public void Process(IEnumerable<Story> stories)
    {
        var path = new DirectoryInfo(OutputPath);

        if (!path.Exists)
            path.Create();

        var storiesList = stories as IList<Story> ?? stories.ToList();
        string testNameSpace = null;

        if (storiesList.Count > 0)
        {
            var groupedByNamespace
                = (from story in storiesList
                   where story.Metadata != null
                   orderby story.Namespace
                   group story by story.Namespace
                    into g
                   select g).FirstOrDefault();
            if (groupedByNamespace != null)
            {
                testNameSpace = groupedByNamespace.Key ?? (
                    storiesList.Count > 0
                        ? storiesList.First().Namespace
                        : string.Format("TestResult_{0}", DateTime.UtcNow.ToFileTimeUtc()));
            }
            else
            {
                testNameSpace = storiesList.First().Namespace;
            }

            var firstOrDefault = storiesList.FirstOrDefault(story => story.Metadata != null);
            if (firstOrDefault != null)
            {
                var description = firstOrDefault.Metadata.Type.Assembly.GetAssemblyDescription();
                if (!string.IsNullOrWhiteSpace(description))
                    testNameSpace = description;
            }
        }
        else
        {
            testNameSpace = "NoStories" + DateTime.Now.ToFileTime();
        }

        var reporter = new DiagnosticsReportBuilder(
            new MySerializer());

        var model = new FileReportModel(stories.ToReportModel())
        {
            RunDate = DateTime.Now,
        };

        var diag = reporter.GetDiagnosticData(model);

        var myDiag = new MyDiagnostics
        {
            Title = testNameSpace,
            RunDate = model.RunDate,
            Summary = model.Summary,
            StoryDiagnostics = diag,
            Stories = storiesList
        };
        var jModel = JsonConvert.SerializeObject(myDiag,
            Formatting.Indented);

        using (var serializer = new StreamWriter(Path.Combine(path.FullName,
            string.Format("{0}.json", FileName ?? testNameSpace)), false))
        using (var jsonSrlzr = new JsonTextWriter(serializer))
        {
            jsonSrlzr.WriteRaw(jModel);
            jsonSrlzr.Flush();
        }
    }

    public string OutputPath { get; set; }
}

public class MyDiagnostics
{
    public string Title;
    public DateTime RunDate;
    public FileReportSummaryModel Summary;
    public IList<StoryDiagnostic> StoryDiagnostics;
    public IEnumerable<Story> Stories;
}

public class MySerializer : ISerializer
{
    public string Serialize(object obj)
    {
        return JsonConvert.SerializeObject(obj,
            Formatting.Indented);
    }
}