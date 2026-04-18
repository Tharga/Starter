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

    private static async Task<ProcessResult> RunAsync(string fileName, string arguments)
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

        process.Start();
        var output = await process.StandardOutput.ReadToEndAsync();
        var error = await process.StandardError.ReadToEndAsync();
        await process.WaitForExitAsync();

        return new ProcessResult(process.ExitCode, output, error);
    }

    private record ProcessResult(int ExitCode, string Output, string Error);
}
