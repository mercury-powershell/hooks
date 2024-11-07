using System;
using System.IO;
using System.Linq;
using LibGit2Sharp;
using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.IO;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.MSBuild;
using Nuke.Common.Tools.Pwsh;
using Serilog;

[ShutdownDotNetAfterServerBuild]
class Build : NukeBuild {
  // Parameters
  [Parameter("Configuration to build - Default is 'Release'")]
  readonly Configuration Configuration = Configuration.Release;

  [Parameter("NuGet API key for publishing to PSGallery")]
  [Secret]
  readonly string NuGetApiKey;

  // Git
  readonly Repository Repository = new(RootDirectory);
  string CurrentBranch;

  // Paths
  AbsolutePath PublishDirectoryPath
    => RootDirectory / "Mercury.PowerShell.Hooks";

  // Targets
  Target Clean => _ => _
    .Executes(() => {
      Log.Information("Cleaning directories...");

      PublishDirectoryPath.CreateOrCleanDirectory();
    });

  Target CheckForUncommittedChanges => _ => _
    .DependsOn(Clean)
    .Executes(() => {
      var hasUncommittedChanges = Repository.RetrieveStatus().IsDirty;

      if (hasUncommittedChanges) {
        Log.Fatal("There are uncommitted changes. Please commit or stash them before proceeding.");
        Environment.Exit(1);
      }
    });

  Target SwitchToMainBranch => _ => _
    .DependsOn(CheckForUncommittedChanges)
    .Executes(() => {
      CurrentBranch = Repository.Head.FriendlyName;

      if (CurrentBranch == "main") {
        return;
      }

      Log.Information("Switching to main branch...");
      Commands.Checkout(Repository, Repository.Branches["main"]);
    });

  Target GenerateNewRelease => _ => _
    .DependsOn(SwitchToMainBranch)
    .Executes(() => {
      var manifestPath = RootDirectory / "source" / "Mercury.PowerShell.Hooks" / "Mercury.PowerShell.Hooks.psd1";

      var versionLine = File
        .ReadAllLines(manifestPath)
        .FirstOrDefault(line => line.Trim().StartsWith("ModuleVersion"));

      if (string.IsNullOrEmpty(versionLine)) {
        throw new Exception("ModuleVersion not found in .psd1 file.");
      }

      var version = versionLine.Split('=')[1].Trim().Trim('\'', '"');
      Log.Information("Current Module Version: v{0}", version);

      if (Repository.Tags.Any(tag => tag.FriendlyName == $"v{version}")) {
        Log.Information("Release already exists.");
        return;
      }

      Log.Information("Creating new release");
      Repository.Tags.Add($"v{version}", Repository.Head.Tip);
    });

  Target PublishDotNetProject => _ => _
    .DependsOn(GenerateNewRelease)
    .Executes(() => {
      Log.Information("Publishing .NET project...");

      var projectPath = RootDirectory / "source" / "Mercury.PowerShell.Hooks" / "Mercury.PowerShell.Hooks.csproj";

      var version = Repository.Describe(Repository.Head.Tip, new DescribeOptions {
        Strategy = DescribeStrategy.Tags,
        MinimumCommitIdAbbreviatedSize = 60,
        AlwaysRenderLongFormat = true
      });

      MSBuildTasks
        .MSBuild(_ => _
          .SetProjectFile(projectPath)
          .SetConfiguration(Configuration)
          .SetVerbosity(MSBuildVerbosity.Minimal)
          .SetTargets("Clean", "Build", "Publish")
          .SetProperty("PublishDir", PublishDirectoryPath)
          .SetProperty("MercuryVersion", version));

      Log.Information("Publishing .NET project completed.");
    });

  Target GenerateDocumentation => _ => _
    .DependsOn(PublishDotNetProject)
    .Executes(() => {
      Log.Information("Generating documentation...");

      var newXmlDocumentationPs1Path = RootDirectory / "documentation" / "New-XmlDocumentation.ps1";

      PwshTasks
        .Pwsh(_ => _
          .SetNonInteractive(true)
          .SetNoLogo(true)
          .SetNoProfile(true)
          .SetExecutionPolicy("Unrestricted")
          .SetFile(newXmlDocumentationPs1Path)
          .SetFileArguments("-Language", "en-US", "-OutputDirectory", PublishDirectoryPath));

      Log.Information("Generating documentation completed.");
    });

  Target PublishToPSGallery => _ => _
    .DependsOn(GenerateDocumentation)
    .Requires(() => NuGetApiKey)
    .Executes(() => {
      Log.Information("Publishing to PSGallery...");

      PwshTasks
        .Pwsh(_ => _
          .SetNonInteractive(true)
          .SetNoLogo(true)
          .SetNoProfile(true)
          .SetCommand($"Set-Location {PublishDirectoryPath} && Publish-Module -Path . -Repository PSGallery -NuGetApiKey $env:PSGalleryApiKey")
          .SetProcessEnvironmentVariable("PSGalleryApiKey", NuGetApiKey));

      Log.Information("Publishing to PSGallery completed.");
    });

  Target CleanUp => _ => _
    .DependsOn(PublishToPSGallery)
    .Executes(() => {
      Log.Information("Cleaning up...");

      PublishDirectoryPath
        .DeleteDirectory();

      Commands.Checkout(Repository, Repository.Branches[CurrentBranch]);
    });

  Target Default => _ => _
    .DependsOn(CleanUp);

  public static int Main()
    => Execute<Build>(build => build.Default);
}
