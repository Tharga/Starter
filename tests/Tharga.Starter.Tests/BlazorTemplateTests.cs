using Xunit;

namespace Tharga.Starter.Tests;

[Collection("Template")]
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
    [Trait("Category", "Slow")]
    public async Task DefaultOptions_BuildsAndTestsPass()
    {
        var projectPath = await TemplateTestHelper.CreateProjectAsync(_tempDir, "tharga-blazor", "BlazorDefault");
        await TemplateTestHelper.AssertBuildsAsync(projectPath);
        await TemplateTestHelper.AssertTestsPassAsync(projectPath);
    }

    [Fact]
    public async Task DefaultOptions_ContainsExpectedStructureAndContent()
    {
        var projectPath = await TemplateTestHelper.CreateProjectAsync(_tempDir, "tharga-blazor", "BlazorCheck");

        // Project structure
        Assert.True(Directory.Exists(Path.Combine(projectPath, "BlazorCheck")), "Server project should exist");
        Assert.True(Directory.Exists(Path.Combine(projectPath, "BlazorCheck.Client")), "Client project should exist");
        Assert.True(Directory.Exists(Path.Combine(projectPath, "BlazorCheck.Tests")), "Test project should exist");
        Assert.True(Directory.Exists(Path.Combine(projectPath, "BlazorCheck.IntegrationTests")), "Integration test project should exist");

        // Package references
        var csproj = await File.ReadAllTextAsync(Path.Combine(projectPath, "BlazorCheck", "BlazorCheck.csproj"));
        Assert.Contains("Tharga.Blazor", csproj);
        Assert.Contains("Quilt4Net.Toolkit.Health", csproj);

        // Default includes samples
        Assert.True(File.Exists(Path.Combine(projectPath, "BlazorCheck.Client", "Pages", "Counter.razor")));
        Assert.True(File.Exists(Path.Combine(projectPath, "BlazorCheck", "Components", "Pages", "Weather.razor")));
        Assert.True(File.Exists(Path.Combine(projectPath, "BlazorCheck", "Components", "Pages", "About.razor")));

        // Program.cs includes health and rate limiting by default, no template directives
        var program = await File.ReadAllTextAsync(Path.Combine(projectPath, "BlazorCheck", "Program.cs"));
        Assert.Contains("AddQuilt4NetHealth", program);
        Assert.Contains("UseQuilt4NetHealth", program);
        Assert.Contains("AddRateLimiter", program);
        Assert.Contains("UseRateLimiter", program);
        Assert.DoesNotContain("#if", program);
        Assert.DoesNotContain("#endif", program);

        // NavMenu has no template directives
        var navMenu = await File.ReadAllTextAsync(Path.Combine(projectPath, "BlazorCheck", "Components", "Layout", "NavMenu.razor"));
        Assert.DoesNotContain("//-", navMenu);
        Assert.DoesNotContain("#if", navMenu);
    }

    [Fact]
    [Trait("Category", "Slow")]
    public async Task NoSamples_BuildsAndExcludesSampleContent()
    {
        var projectPath = await TemplateTestHelper.CreateProjectAsync(_tempDir, "tharga-blazor", "BlazorNoSamp", "--IncludeSamples false");
        await TemplateTestHelper.AssertBuildsAsync(projectPath);

        Assert.False(File.Exists(Path.Combine(projectPath, "BlazorNoSamp.Client", "Pages", "Counter.razor")));
        Assert.False(File.Exists(Path.Combine(projectPath, "BlazorNoSamp", "Components", "Pages", "Weather.razor")));

        var navMenu = await File.ReadAllTextAsync(Path.Combine(projectPath, "BlazorNoSamp", "Components", "Layout", "NavMenu.razor"));
        Assert.DoesNotContain("Counter", navMenu);
        Assert.DoesNotContain("Weather", navMenu);
        Assert.DoesNotContain("//-", navMenu);
    }

    [Fact]
    [Trait("Category", "Slow")]
    public async Task AllOptionsDisabled_Builds()
    {
        var projectPath = await TemplateTestHelper.CreateProjectAsync(_tempDir, "tharga-blazor", "BlazorMinimal",
            "--IncludeSamples false --IncludeHealth false --IncludeRateLimiting false");
        await TemplateTestHelper.AssertBuildsAsync(projectPath);

        var csproj = await File.ReadAllTextAsync(Path.Combine(projectPath, "BlazorMinimal", "BlazorMinimal.csproj"));
        Assert.DoesNotContain("Quilt4Net.Toolkit.Health", csproj);

        var program = await File.ReadAllTextAsync(Path.Combine(projectPath, "BlazorMinimal", "Program.cs"));
        Assert.DoesNotContain("AddQuilt4NetHealth", program);
        Assert.DoesNotContain("AddRateLimiter", program);
    }
}
