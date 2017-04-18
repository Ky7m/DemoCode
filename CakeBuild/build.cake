//////////////////////////////////////////////////////////////////////
// TOOLS
//////////////////////////////////////////////////////////////////////
#tool "nuget:?package=OctopusTools"
#tool "nuget:?package=xunit.runner.console"

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////
var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var buildNumber = Argument("buildNumber", "1.0.0.0");
var octoServer = Argument("octoServer", "{Your Server}");
var octoApiKey = Argument("octoApiKey", "{API Key}");
var octoProject = Argument("octoProject", "{Project Name}");
var octoTargetEnvironment = Argument("octoTargetEnvironment", "Development");

///////////////////////////////////////////////////////////////////////////////
// GLOBAL VARIABLES
///////////////////////////////////////////////////////////////////////////////
var outputDirectory = Directory("./build");
var packageDirectory= Directory("./publish");
var buildArtifacts = Directory("./BuildArtifacts/TestResults");
var solutionFile = "./CakeBuild.sln";
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

Task("Transform")
    .WithCriteria(isContinuousIntegrationBuild)
    .Does(() =>
    {
        var file = File("./test/CakeBuildDemo.RegressionTests/app.config");
        XmlPoke(file, "/configuration/appSettings/add[@key = 'ApiEndpointBaseAddress']/@value", "developmenthost");
    });

Task("Build")
    .IsDependentOn("Restore")
    .IsDependentOn("Transform")
    .Does(() =>
    {
       DotNetBuild(solutionFile, settings => settings
            .SetConfiguration(configuration)
            .WithTarget("Rebuild")
            //.WithProperty("TreatWarningsAsErrors", "True")
            .SetVerbosity(Verbosity.Minimal));
    });

Task("UnitTests")
    .IsDependentOn("Build")
    .Does(() =>
    {
        XUnit2(string.Format("./test/**/bin/{0}/*.UnitTests.dll",configuration), 
        new XUnit2Settings 
        {
            XmlReport = true,
            OutputDirectory = buildArtifacts
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

Task("OctoPush")
    .WithCriteria(isContinuousIntegrationBuild)
    .IsDependentOn("Zip-Files")
    .Does(() => 
    {
        OctoPush(octoServer,
                    octoApiKey,
                    new FilePath(packageName),
                    new OctopusPushSettings {
                    ReplaceExisting = true
                    });
    });

Task("OctoRelease")
    .WithCriteria(isContinuousIntegrationBuild)
    .IsDependentOn("OctoPush")
    .Does(() => 
    {
        OctoCreateRelease(octoProject, new CreateReleaseSettings {
            Server = octoServer,
            ApiKey = octoApiKey,
            ReleaseNumber = buildNumber
            });
    });

Task("OctoDeploy")
    .WithCriteria(isContinuousIntegrationBuild)
    .IsDependentOn("OctoRelease")
    .Does(() => 
    {
        OctoDeployRelease(octoServer,
            octoApiKey,
            octoProject,
            octoTargetEnvironment,
            buildNumber,
            new OctopusDeployReleaseDeploymentSettings {
                ShowProgress = false,
                WaitForDeployment = true
            });
    });

Task("RegressionTests")
    .WithCriteria(isContinuousIntegrationBuild)
    .IsDependentOn("OctoDeploy")
    .Does(() =>
    {
        XUnit2(string.Format("./test/**/bin/{0}/*.RegressionTests.dll",configuration), 
        new XUnit2Settings 
        {
            XmlReport = true,
            OutputDirectory = buildArtifacts
        });
    });

Task("Default")
    .IsDependentOn("OctoDeploy")
    .IsDependentOn("RegressionTests");

RunTarget(target);