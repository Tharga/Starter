using Xunit;

namespace Tharga.Starter.Tests;

public class BlazorTemplateTests : IAsyncLifetime
{
    private readonly string _templatePath = Path.Combine(TemplateTestHelper.TemplatesRoot, "blazor");
    private string _tempDir = null!;

    public async Task InitializeAsync()
    {
        _tempDir = TemplateTestHelper.CreateTempDirectory();
        await TemplateTestHelper.InstallTemplateAsync(_templatePath);
    }

    public async Task DisposeAsync()
    {
        TemplateTestHelper.Cleanup(_tempDir);
        await TemplateTestHelper.UninstallTemplateAsync(_templatePath);
    }

    [Fact]
    public async Task Builds()
    {
        var projectPath = await TemplateTestHelper.CreateProjectAsync(_tempDir, "tharga-blazor", "BlazorBuild");
        await TemplateTestHelper.AssertBuildsAsync(projectPath);
    }

    [Fact]
    public async Task TestsPass()
    {
        var projectPath = await TemplateTestHelper.CreateProjectAsync(_tempDir, "tharga-blazor", "BlazorTests");
        await TemplateTestHelper.AssertBuildsAsync(projectPath);
        await TemplateTestHelper.AssertTestsPassAsync(projectPath);
    }

    [Fact]
    public async Task ContainsServerProject()
    {
        var projectPath = await TemplateTestHelper.CreateProjectAsync(_tempDir, "tharga-blazor", "BlazorServer");
        Assert.True(Directory.Exists(Path.Combine(projectPath, "BlazorServer")),
            "Server project folder should exist");
        Assert.True(File.Exists(Path.Combine(projectPath, "BlazorServer", "BlazorServer.csproj")),
            "Server project file should exist");
    }

    [Fact]
    public async Task ContainsClientProject()
    {
        var projectPath = await TemplateTestHelper.CreateProjectAsync(_tempDir, "tharga-blazor", "BlazorClient");
        Assert.True(Directory.Exists(Path.Combine(projectPath, "BlazorClient.Client")),
            "Client project folder should exist");
        Assert.True(File.Exists(Path.Combine(projectPath, "BlazorClient.Client", "BlazorClient.Client.csproj")),
            "Client project file should exist");
    }

    [Fact]
    public async Task ContainsTestProject()
    {
        var projectPath = await TemplateTestHelper.CreateProjectAsync(_tempDir, "tharga-blazor", "BlazorTestProj");
        Assert.True(Directory.Exists(Path.Combine(projectPath, "BlazorTestProj.Tests")),
            "Test project folder should exist");
        Assert.True(File.Exists(Path.Combine(projectPath, "BlazorTestProj.Tests", "BlazorTestProj.Tests.csproj")),
            "Test project file should exist");
    }
}
