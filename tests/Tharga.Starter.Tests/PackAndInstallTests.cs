using System.Diagnostics;
using System.IO.Compression;
using Xunit;

namespace Tharga.Starter.Tests;

[Collection("Template")]
public class PackAndInstallTests : IAsyncLifetime
{
    private static readonly string RepoRoot =
        Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", ".."));
    private static readonly string PackOutput = Path.Combine(RepoRoot, "nupkg-test");
    private string _nupkgPath = null!;

    public async Task InitializeAsync()
    {
        Directory.CreateDirectory(PackOutput);
        var csproj = Path.Combine(RepoRoot, "Tharga.Starter.csproj");
        var result = await RunAsync("dotnet",
            $"pack \"{csproj}\" -c Release -o \"{PackOutput}\" -p:PackageVersion=0.0.0-test");
        Assert.True(result.ExitCode == 0, $"Pack failed:\n{result.Output}\n{result.Error}");

        _nupkgPath = Path.Combine(PackOutput, "Tharga.Starter.0.0.0-test.nupkg");
        Assert.True(File.Exists(_nupkgPath), $"Nupkg not found at {_nupkgPath}");
    }

    public Task DisposeAsync()
    {
        try { Directory.Delete(PackOutput, true); } catch { }
        return Task.CompletedTask;
    }

    [Fact]
    public void NupkgContainsAllThreeTemplates()
    {
        using var archive = ZipFile.OpenRead(_nupkgPath);
        var names = archive.Entries.Select(e => e.FullName).ToList();
        Assert.Contains(names, n => n == "content/templates/console/.template.config/template.json");
        Assert.Contains(names, n => n == "content/templates/blazor/.template.config/template.json");
        Assert.Contains(names, n => n == "content/templates/blazor-platform/.template.config/template.json");
    }

    [Fact]
    public void NupkgContainsReadme()
    {
        using var archive = ZipFile.OpenRead(_nupkgPath);
        var names = archive.Entries.Select(e => e.FullName).ToList();
        Assert.Contains(names, n => n == "README.md");
    }

    [Fact]
    public void NupkgExcludesBinAndObj()
    {
        using var archive = ZipFile.OpenRead(_nupkgPath);
        var names = archive.Entries.Select(e => e.FullName).ToList();
        Assert.DoesNotContain(names, n => n.Contains("/bin/") || n.Contains("/obj/"));
    }

    [Fact]
    public async Task NupkgInstallsAndProducesWorkingTemplates()
    {
        var installResult = await RunAsync("dotnet", $"new install \"{_nupkgPath}\" --force");
        Assert.True(installResult.ExitCode == 0, $"Install failed:\n{installResult.Output}\n{installResult.Error}");

        try
        {
            var tempDir = TemplateTestHelper.CreateTempDirectory();
            try
            {
                await TemplateTestHelper.CreateProjectAsync(tempDir, "tharga-console", "PackConsole");
                await TemplateTestHelper.CreateProjectAsync(tempDir, "tharga-blazor", "PackBlazor");
                await TemplateTestHelper.CreateProjectAsync(tempDir, "tharga-blazor-platform", "PackPlatform");
            }
            finally
            {
                TemplateTestHelper.Cleanup(tempDir);
            }
        }
        finally
        {
            await RunAsync("dotnet", "new uninstall Tharga.Starter");
        }
    }

    private static async Task<(int ExitCode, string Output, string Error)> RunAsync(string fileName, string arguments)
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
        return (process.ExitCode, output, error);
    }
}
