using System.Diagnostics;
using Xunit;

namespace Tharga.Starter.Tests;

internal static class TemplateTestHelper
{
    public static string TemplatesRoot { get; } =
        Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "..", "templates"));

    public static async Task InstallTemplateAsync(string templatePath)
    {
        var result = await RunAsync("dotnet", $"new install \"{templatePath}\" --force");
        Assert.True(result.ExitCode == 0, $"Template install failed:\n{result.Output}\n{result.Error}");
    }

    public static async Task UninstallTemplateAsync(string templatePath)
    {
        await RunAsync("dotnet", $"new uninstall \"{templatePath}\"");
    }

    public static string CreateTempDirectory()
    {
        var path = Path.Combine(Path.GetTempPath(), "tharga-template-tests", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(path);
        return path;
    }

    public static async Task<string> CreateProjectAsync(string tempDir, string shortName, string projectName, string extraArgs = "")
    {
        var args = $"new {shortName} -n {projectName} -o \"{Path.Combine(tempDir, projectName)}\" {extraArgs}".Trim();
        var result = await RunAsync("dotnet", args);
        Assert.True(result.ExitCode == 0, $"Project creation failed:\n{result.Output}\n{result.Error}");
        return Path.Combine(tempDir, projectName);
    }

    public static async Task AssertBuildsAsync(string projectPath)
    {
        var result = await RunAsync("dotnet", $"build \"{projectPath}\" -c Release");
        Assert.True(result.ExitCode == 0, $"Build failed:\n{result.Output}\n{result.Error}");
    }

    public static async Task AssertTestsPassAsync(string projectPath)
    {
        var result = await RunAsync("dotnet", $"test \"{projectPath}\" -c Release --no-build");
        Assert.True(result.ExitCode == 0, $"Tests failed:\n{result.Output}\n{result.Error}");
    }

    /// <summary>
    /// Runs the generated project's own integration tests, which start the real application. A template that
    /// compiles can still throw while the host is built (a scope registered twice, a missing registration), and a
    /// build alone does not see that. Placeholder configuration stands in for what a developer supplies.
    /// </summary>
    public static async Task AssertGeneratedAppStartsAsync(string projectPath, string projectName)
    {
        var integrationTests = Path.Combine(projectPath, $"{projectName}.IntegrationTests");
        var result = await RunAsync("dotnet", $"test \"{integrationTests}\" -c Release", PlaceholderConfiguration);
        Assert.True(result.ExitCode == 0, $"The generated app did not start:\n{result.Output}\n{result.Error}");
    }

    private static readonly IReadOnlyDictionary<string, string> BuildIsolation = new Dictionary<string, string>
    {
        ["MSBUILDDISABLENODEREUSE"] = "1",
        ["UseSharedCompilation"] = "false",
        ["DOTNET_CLI_USE_MSBUILD_SERVER"] = "0"
    };

    private static readonly IReadOnlyDictionary<string, string> PlaceholderConfiguration = new Dictionary<string, string>
    {
        ["ConnectionStrings__Default"] = "mongodb://localhost:27017/startupcheck",
        ["AzureAd__Authority"] = "https://example.ciamlogin.com/example.onmicrosoft.com",
        ["AzureAd__ClientId"] = "00000000-0000-0000-0000-000000000000",
        ["AzureAd__TenantId"] = "00000000-0000-0000-0000-000000000000"
    };

    public static void Cleanup(string tempDir)
    {
        try
        {
            if (Directory.Exists(tempDir))
                Directory.Delete(tempDir, true);
        }
        catch
        {
            // Best effort cleanup
        }
    }

    private static async Task<ProcessResult> RunAsync(string fileName, string arguments, IReadOnlyDictionary<string, string>? environment = null)
    {
        using var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = fileName,
                Arguments = arguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };

        // A build that leaves worker processes behind keeps the redirected output pipe open until they time out
        // (MSBuild idles its nodes for 15 minutes), so reading to the end of the output waited that long.
        foreach (var (key, value) in BuildIsolation) process.StartInfo.Environment[key] = value;

        if (environment != null)
        {
            foreach (var (key, value) in environment) process.StartInfo.Environment[key] = value;
        }

        process.Start();
        var output = await process.StandardOutput.ReadToEndAsync();
        var error = await process.StandardError.ReadToEndAsync();
        await process.WaitForExitAsync();

        return new ProcessResult(process.ExitCode, output, error);
    }

    private record ProcessResult(int ExitCode, string Output, string Error);
}
