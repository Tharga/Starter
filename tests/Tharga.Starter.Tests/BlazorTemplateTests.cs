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

    [Fact]
    public async Task ContainsIntegrationTestProject()
    {
        var projectPath = await TemplateTestHelper.CreateProjectAsync(_tempDir, "tharga-blazor", "BlazorIntTest");
        Assert.True(Directory.Exists(Path.Combine(projectPath, "BlazorIntTest.IntegrationTests")),
            "Integration test project folder should exist");
        Assert.True(File.Exists(Path.Combine(projectPath, "BlazorIntTest.IntegrationTests", "BlazorIntTest.IntegrationTests.csproj")),
            "Integration test project file should exist");
    }

    [Fact]
    public async Task DefaultIncludesSamplePages()
    {
        var projectPath = await TemplateTestHelper.CreateProjectAsync(_tempDir, "tharga-blazor", "BlazorSamples");
        Assert.True(File.Exists(Path.Combine(projectPath, "BlazorSamples.Client", "Pages", "Counter.razor")),
            "Counter page should exist by default");
        Assert.True(File.Exists(Path.Combine(projectPath, "BlazorSamples", "Components", "Pages", "Weather.razor")),
            "Weather page should exist by default");
    }

    [Fact]
    public async Task ExcludesSamplesWhenOptionIsFalse()
    {
        var projectPath = await TemplateTestHelper.CreateProjectAsync(_tempDir, "tharga-blazor", "BlazorNoSamples", "--IncludeSamples false");
        Assert.False(File.Exists(Path.Combine(projectPath, "BlazorNoSamples.Client", "Pages", "Counter.razor")),
            "Counter page should not exist when IncludeSamples is false");
        Assert.False(File.Exists(Path.Combine(projectPath, "BlazorNoSamples", "Components", "Pages", "Weather.razor")),
            "Weather page should not exist when IncludeSamples is false");
    }

    [Fact]
    public async Task BuildsWithoutSamples()
    {
        var projectPath = await TemplateTestHelper.CreateProjectAsync(_tempDir, "tharga-blazor", "BlazorNoSamplesBuild", "--IncludeSamples false");
        await TemplateTestHelper.AssertBuildsAsync(projectPath);
    }

    [Fact]
    public async Task NavMenuDoesNotContainTemplateDirectives()
    {
        var projectPath = await TemplateTestHelper.CreateProjectAsync(_tempDir, "tharga-blazor", "BlazorNavCheck");
        var navMenu = await File.ReadAllTextAsync(Path.Combine(projectPath, "BlazorNavCheck", "Components", "Layout", "NavMenu.razor"));
        Assert.DoesNotContain("//-", navMenu);
        Assert.DoesNotContain("#if", navMenu);
        Assert.DoesNotContain("#endif", navMenu);
    }

    [Fact]
    public async Task NavMenuExcludesSampleLinksWhenOptionIsFalse()
    {
        var projectPath = await TemplateTestHelper.CreateProjectAsync(_tempDir, "tharga-blazor", "BlazorNavNoSamp", "--IncludeSamples false");
        var navMenu = await File.ReadAllTextAsync(Path.Combine(projectPath, "BlazorNavNoSamp", "Components", "Layout", "NavMenu.razor"));
        Assert.DoesNotContain("Counter", navMenu);
        Assert.DoesNotContain("Weather", navMenu);
        Assert.DoesNotContain("//-", navMenu);
    }

    [Fact]
    public async Task ContainsAboutPage()
    {
        var projectPath = await TemplateTestHelper.CreateProjectAsync(_tempDir, "tharga-blazor", "BlazorAbout");
        Assert.True(File.Exists(Path.Combine(projectPath, "BlazorAbout", "Components", "Pages", "About.razor")),
            "About page should exist");
    }

    [Fact]
    public async Task ContainsThargaBlazorPackageReference()
    {
        var projectPath = await TemplateTestHelper.CreateProjectAsync(_tempDir, "tharga-blazor", "BlazorPkgRef");
        var csproj = await File.ReadAllTextAsync(Path.Combine(projectPath, "BlazorPkgRef", "BlazorPkgRef.csproj"));
        Assert.Contains("Tharga.Blazor", csproj);
    }

    [Fact]
    public async Task BuildsWithHealth()
    {
        var projectPath = await TemplateTestHelper.CreateProjectAsync(_tempDir, "tharga-blazor", "BlazorHealth", "--IncludeHealth true");
        await TemplateTestHelper.AssertBuildsAsync(projectPath);
    }

    [Fact]
    public async Task BuildsWithRateLimiting()
    {
        var projectPath = await TemplateTestHelper.CreateProjectAsync(_tempDir, "tharga-blazor", "BlazorRateLimit", "--IncludeRateLimiting true");
        await TemplateTestHelper.AssertBuildsAsync(projectPath);
    }

    [Fact]
    public async Task BuildsWithHealthAndRateLimiting()
    {
        var projectPath = await TemplateTestHelper.CreateProjectAsync(_tempDir, "tharga-blazor", "BlazorBoth", "--IncludeHealth true --IncludeRateLimiting true");
        await TemplateTestHelper.AssertBuildsAsync(projectPath);
    }

    [Fact]
    public async Task HealthIncludesPackageReference()
    {
        var projectPath = await TemplateTestHelper.CreateProjectAsync(_tempDir, "tharga-blazor", "BlazorHealthPkg", "--IncludeHealth true");
        var csproj = await File.ReadAllTextAsync(Path.Combine(projectPath, "BlazorHealthPkg", "BlazorHealthPkg.csproj"));
        Assert.Contains("Quilt4Net.Toolkit.Health", csproj);
    }

    [Fact]
    public async Task DefaultIncludesHealthPackageReference()
    {
        var projectPath = await TemplateTestHelper.CreateProjectAsync(_tempDir, "tharga-blazor", "BlazorDefHealth");
        var csproj = await File.ReadAllTextAsync(Path.Combine(projectPath, "BlazorDefHealth", "BlazorDefHealth.csproj"));
        Assert.Contains("Quilt4Net.Toolkit.Health", csproj);
    }

    [Fact]
    public async Task HealthIncludesRegistrationInProgram()
    {
        var projectPath = await TemplateTestHelper.CreateProjectAsync(_tempDir, "tharga-blazor", "BlazorHealthProg", "--IncludeHealth true");
        var program = await File.ReadAllTextAsync(Path.Combine(projectPath, "BlazorHealthProg", "Program.cs"));
        Assert.Contains("AddQuilt4NetHealth", program);
        Assert.Contains("UseQuilt4NetHealth", program);
        Assert.DoesNotContain("#if", program);
        Assert.DoesNotContain("#endif", program);
    }

    [Fact]
    public async Task RateLimitingIncludesRegistrationInProgram()
    {
        var projectPath = await TemplateTestHelper.CreateProjectAsync(_tempDir, "tharga-blazor", "BlazorRateProg", "--IncludeRateLimiting true");
        var program = await File.ReadAllTextAsync(Path.Combine(projectPath, "BlazorRateProg", "Program.cs"));
        Assert.Contains("AddRateLimiter", program);
        Assert.Contains("UseRateLimiter", program);
        Assert.DoesNotContain("#if", program);
        Assert.DoesNotContain("#endif", program);
    }

    [Fact]
    public async Task DefaultIncludesHealthAndRateLimitingInProgram()
    {
        var projectPath = await TemplateTestHelper.CreateProjectAsync(_tempDir, "tharga-blazor", "BlazorDefProg");
        var program = await File.ReadAllTextAsync(Path.Combine(projectPath, "BlazorDefProg", "Program.cs"));
        Assert.Contains("AddQuilt4NetHealth", program);
        Assert.Contains("UseQuilt4NetHealth", program);
        Assert.Contains("AddRateLimiter", program);
        Assert.Contains("UseRateLimiter", program);
        Assert.DoesNotContain("#if", program);
        Assert.DoesNotContain("#endif", program);
    }
}
