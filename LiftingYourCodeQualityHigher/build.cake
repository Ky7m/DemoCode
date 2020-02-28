#tool nuget:?package=MSBuild.SonarQube.Runner.Tool
#addin nuget:?package=Cake.Sonar

// ARGUMENTS
var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");


// GLOBAL VARIABLES
var sonarKey = "a92b973fff11a24134d01a8895def0571d2903e4";
var solutionDirectory = "../../AutoMapper";

Task("Restore")
    .Does(() =>
    {
        DotNetCoreRestore(solutionDirectory);
    });

Task("Build")
    .IsDependentOn("Restore")
    .Does(() =>
    {
         var buildSettings = new DotNetCoreMSBuildSettings()
            .WithTarget("Rebuild")
            .SetMaxCpuCount(0); // parallel

        var settings = new DotNetCoreBuildSettings
        {
            Configuration = configuration,
            NoRestore = true,
            MSBuildSettings = buildSettings
        };

        DotNetCoreBuild(solutionDirectory, settings);
    });

/*
This task use Coverlet to collect the code coverage. 
Install it on your test project using the Coverlet NuGet package: 'Install-Package coverlet.msbuild'
*/
Task("Tests")
    .IsDependentOn("Build")
    .Does(() =>
    {
        var settings = new DotNetCoreTestSettings
        {
            NoRestore = true,
            ArgumentCustomization = args => 
                        args.Append("/p:CollectCoverage=true")
                            .Append("/p:CoverletOutputFormat=opencover")
        };
        DotNetCoreTest(solutionDirectory, settings);
    });

Task("Analyze")
  .IsDependentOn("BeginAnalysis")
  .IsDependentOn("Build")
  .IsDependentOn("Tests")
  .IsDependentOn("EndAnalysis");

Task("BeginAnalysis")
  .Does(() => {
     SonarBegin(new SonarBeginSettings{
        Exclusions = "/**/Samples/**/*.cs,**/*.Tests/*.cs",
        OpenCoverReportsPath = "/**/*.opencover.xml",
        Login = sonarKey,
        Url = "https://sonarqubedemo.azurewebsites.net",
        Key = "Automapper",
        // ArgumentCustomization = args => args
        // .Append("/o:ky7m-github")
     });
  });

Task("EndAnalysis")
  .Does(() => {
     SonarEnd(new SonarEndSettings{
        Login = sonarKey
     });
  });

Task("Default")
    .IsDependentOn("Analyze");

RunTarget(target);
