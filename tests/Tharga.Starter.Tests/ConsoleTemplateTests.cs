using Xunit;

namespace Tharga.Starter.Tests;

[Collection("Template")]
public class ConsoleTemplateTests : IAsyncLifetime
{
    private readonly string _templatePath = Path.Combine(TemplateTestHelper.TemplatesRoot, "console");
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
    public async Task WithSample_Builds()
    {
        var projectPath = await TemplateTestHelper.CreateProjectAsync(_tempDir, "tharga-console", "WithSample");
        await TemplateTestHelper.AssertBuildsAsync(projectPath);
    }

    [Fact]
    public async Task WithSample_TestsPass()
    {
        var projectPath = await TemplateTestHelper.CreateProjectAsync(_tempDir, "tharga-console", "WithSampleTests");
        await TemplateTestHelper.AssertBuildsAsync(projectPath);
        await TemplateTestHelper.AssertTestsPassAsync(projectPath);
    }

    [Fact]
    public async Task WithoutSample_Builds()
    {
        var projectPath = await TemplateTestHelper.CreateProjectAsync(_tempDir, "tharga-console", "WithoutSample", "--IncludeSample false");
        await TemplateTestHelper.AssertBuildsAsync(projectPath);
    }

    [Fact]
    public async Task WithoutSample_TestsPass()
    {
        var projectPath = await TemplateTestHelper.CreateProjectAsync(_tempDir, "tharga-console", "WithoutSampleTests", "--IncludeSample false");
        await TemplateTestHelper.AssertBuildsAsync(projectPath);
        await TemplateTestHelper.AssertTestsPassAsync(projectPath);
    }

    [Fact]
    public async Task WithSample_ContainsCommandsFolder()
    {
        var projectPath = await TemplateTestHelper.CreateProjectAsync(_tempDir, "tharga-console", "HasCommands");
        Assert.True(Directory.Exists(Path.Combine(projectPath, "HasCommands", "Commands")),
            "Commands folder should exist when IncludeSample is true");
    }

    [Fact]
    public async Task WithoutSample_DoesNotContainCommandsFolder()
    {
        var projectPath = await TemplateTestHelper.CreateProjectAsync(_tempDir, "tharga-console", "NoCommands", "--IncludeSample false");
        Assert.False(Directory.Exists(Path.Combine(projectPath, "NoCommands", "Commands")),
            "Commands folder should not exist when IncludeSample is false");
    }
}
