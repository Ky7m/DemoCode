//////////////////////////////////////////////////////////////////////
// TOOLS
//////////////////////////////////////////////////////////////////////
#tool "nuget:?package=xunit.runner.console"
#tool "nuget:?package=JetBrains.ReSharper.CommandLineTools"

//////////////////////////////////////////////////////////////////////
// ADDINS
//////////////////////////////////////////////////////////////////////
#addin "Cake.Prca"
#addin "Cake.Prca.Issues.InspectCode"

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////
var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var buildNumber = Argument("buildNumber", "1.0.0.0");

///////////////////////////////////////////////////////////////////////////////
// GLOBAL VARIABLES
///////////////////////////////////////////////////////////////////////////////
var outputDirectory = Directory("./build");
var packageDirectory= Directory("./publish");
var buildArtifacts = Directory("./BuildArtifacts");
var testResults = buildArtifacts + Directory("_TestResults");
var analysisResults = buildArtifacts + Directory("_AnalysisResults");
var solutionFile = "./ASP.NET-CakeBuild-Sample.sln";
var packageName = string.Format("./publish/CakeBuildDemo.{0}.zip",buildNumber);

var isContinuousIntegrationBuild = !BuildSystem.IsLocalBuild;

Task("Clean")
    .Does(() =>
    {
        CleanDirectory(outputDirectory);
        CleanDirectory(packageDirectory);
        CleanDirectory(buildArtifacts);
    });

Task("Restore")
    .IsDependentOn("Clean")
    .Does(() =>
    {
        NuGetRestore(solutionFile);
    });

Task("Build")
    .IsDependentOn("Restore")
    .IsDependentOn("Transform")
    .Does(() =>
	{
       MSBuild(solutionFile, settings => settings
            .SetConfiguration(configuration)
            .WithTarget("Rebuild")
            .SetVerbosity(Verbosity.Minimal)
            .UseToolVersion(MSBuildToolVersion.Default)
            .SetMSBuildPlatform(MSBuildPlatform.Automatic)
            .SetPlatformTarget(PlatformTarget.MSIL) // Any CPU
            .SetNodeReuse(true)
			//.WithProperty("TreatWarningsAsErrors", "True")
            );
    });

Task("Inspect")
    .IsDependentOn("Restore")
    .WithCriteria(IsRunningOnWindows())
    .Does(() =>
    {
        DupFinder(solutionFile, new DupFinderSettings {
            ShowStats = true,
            ShowText = true,
            OutputFile = analysisResults + File("dupFinder-output.xml"),
            ThrowExceptionOnFindingDuplicates = true
        });
        
        var msBuildProperties = new Dictionary<string, string> {
            {"Configuration", configuration}
        };

        var inspectReportFilePath = analysisResults + File("InspectCode-output.xml");
        InspectCode(solutionFile, new InspectCodeSettings {
            SolutionWideAnalysis = true,
            ThrowExceptionOnFindingViolations = true,
            Extensions = new string[] {
                "EtherealCode.ReSpeller",
                "Sizikov.AsyncSuffix"
            },
            ArgumentCustomization = args => args.Append("--toolset=15.0"),
            OutputFile = inspectReportFilePath
        });

        if (FileExists(inspectReportFilePath)) {
            var settings = new ReadIssuesSettings(MakeAbsolute(Directory("./")));
            var issues = ReadIssues(InspectCodeIssuesFromFilePath(inspectReportFilePath), settings);
            Information("{0} issues are found.", issues.Count());
        }
    });

Task("Transform")
    .WithCriteria(isContinuousIntegrationBuild)
    .Does(() =>
    {
        var file = File("./test/CakeBuildDemo.RegressionTests/app.config");
        XmlPoke(file, "/configuration/appSettings/add[@key = 'ApiEndpointBaseAddress']/@value", "developmenthost");
    });

Task("UnitTests")
    .IsDependentOn("Build")
    .Does(() =>
    {
        XUnit2(string.Format("./test/**/bin/{0}/*.UnitTests.dll",configuration), 
        new XUnit2Settings 
        {
            XmlReport = true,
            OutputDirectory = testResults
        });
    });

Task("PatchVersion")
    .WithCriteria(isContinuousIntegrationBuild)
    .Does(() =>
    {
        CreateAssemblyInfo("./src/CakeBuildDemo.ApiApp/Properties/AssemblyInfo.cs", new AssemblyInfoSettings 
            {
                Product = "CakeBuildDemo.ApiApp",
                Version = buildNumber,
                FileVersion = buildNumber,
                InformationalVersion = buildNumber,
                Copyright = string.Format("Copyright © {0}", DateTime.Now.Year)
            });
    });

Task("Package")
    .IsDependentOn("UnitTests")
    .IsDependentOn("PatchVersion")
    .Does(() =>
    {
        DotNetBuild("./src/CakeBuildDemo.ApiApp/CakeBuildDemo.ApiApp.csproj", settings => settings
            .SetConfiguration(configuration)
            .WithProperty("DeployOnBuild", "true")
            .WithProperty("WebPublishMethod", "FileSystem")
            .WithProperty("DeployTarget", "WebPublish")
            .WithProperty("publishUrl", MakeAbsolute(outputDirectory).FullPath)
            .SetVerbosity(Verbosity.Minimal));
    });

Task("Zip-Files")
    .IsDependentOn("Package")
    .Does(() =>
    {
        Zip(outputDirectory, packageName);
    });

Task("Default")
    .IsDependentOn("Inspect")
    .IsDependentOn("Zip-Files");

RunTarget(target);