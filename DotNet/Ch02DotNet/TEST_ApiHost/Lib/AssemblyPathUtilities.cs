using System.Diagnostics;
using System.Reflection;

namespace TEST_ApiHost.Lib;

public static class AssemblyPathUtilities
{
    public static string GetResultsDirectory(string resultOffset = null)
    {
        return AppDomain.CurrentDomain.BaseDirectory.GetResultsPathFromAppBasePath(resultOffset);
    }

    public static string AssemblyRootName
    {
        get
        {
            var baseName = AppDomain.CurrentDomain.FriendlyName.Split('-', ' ').Last();
            var name = Path.GetFileNameWithoutExtension(baseName);
            return name;
        }
    }

    public static string AssemblyDescription
    {
        get
        {
            try
            {
                var baseAssyPath = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                    string.Concat(AssemblyRootName, ".dll")));
                if (baseAssyPath.Exists)
                {
                    var assys = AppDomain.CurrentDomain.GetAssemblies();
                    var assy = assys.FirstOrDefault(a =>
                    {
                        var result = !a.IsDynamic &&
                                     string.Compare(baseAssyPath.FullName, new Uri(a.CodeBase).LocalPath,
                                         StringComparison.OrdinalIgnoreCase)
                                     == 0;
                        return result;
                    });
                    if (assy != null)
                        return assy.GetAssemblyDescription();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return AssemblyRootName;
        }
    }

    public static string GetAssemblyDescription(this Assembly assy)
    {
        var desc = assy.GetCustomAttributes(typeof(AssemblyDescriptionAttribute)).FirstOrDefault();
        if (desc is AssemblyDescriptionAttribute)
        {
            var attdesc = desc as AssemblyDescriptionAttribute;
            return attdesc.Description;
        }

        return AssemblyRootName;
    }

    public static string GetAssemblyBasePath(this Assembly assy)
    {
        var appbase = assy.EscapedCodeBase;
        var uri = new Uri(appbase);
        return Uri.UnescapeDataString(uri.AbsolutePath);
    }

    public static string GetResultsPathFromAppBasePath(this string basePath,
        string resultRelativePath)
    {
        var resultsPath = Path.GetFullPath(
            Path.Combine(basePath, resultRelativePath)
            );
        return resultsPath;
    }
}