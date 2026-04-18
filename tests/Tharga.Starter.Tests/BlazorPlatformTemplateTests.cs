using Xunit;

namespace Tharga.Starter.Tests;

[Collection("Template")]
public class BlazorPlatformTemplateTests : IAsyncLifetime
{
    private readonly string _templatePath = Path.Combine(TemplateTestHelper.TemplatesRoot, "blazor-platform");
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
    public async Task DefaultOptions_Builds()
    {
        var projectPath = await TemplateTestHelper.CreateProjectAsync(_tempDir, "tharga-blazor-platform", "PlatformBuild");
        await TemplateTestHelper.AssertBuildsAsync(projectPath);
    }

    [Fact]
    public async Task DefaultOptions_ContainsExpectedStructureAndContent()
    {
        var projectPath = await TemplateTestHelper.CreateProjectAsync(_tempDir, "tharga-blazor-platform", "PlatformCheck");

        // Project structure
        Assert.True(Directory.Exists(Path.Combine(projectPath, "PlatformCheck")), "Server project should exist");
        Assert.True(Directory.Exists(Path.Combine(projectPath, "PlatformCheck.Client")), "Client project should exist");
        Assert.True(Directory.Exists(Path.Combine(projectPath, "PlatformCheck.Tests")), "Test project should exist");
        Assert.True(Directory.Exists(Path.Combine(projectPath, "PlatformCheck.IntegrationTests")), "Integration test project should exist");

        // Package references
        var csproj = await File.ReadAllTextAsync(Path.Combine(projectPath, "PlatformCheck", "PlatformCheck.csproj"));
        Assert.Contains("Tharga.Team.Blazor", csproj);
        Assert.Contains("Tharga.Team.Service", csproj);
        Assert.Contains("Tharga.Team.MongoDB", csproj);

        // Platform pages
        var pagesDir = Path.Combine(projectPath, "PlatformCheck", "Components", "Pages");
        var devDir = Path.Combine(projectPath, "PlatformCheck", "Components", "Developer");
        Assert.True(File.Exists(Path.Combine(pagesDir, "Profile.razor")));
        Assert.True(File.Exists(Path.Combine(pagesDir, "Teams.razor")));
        Assert.True(File.Exists(Path.Combine(pagesDir, "ApiKeys.razor")));
        Assert.True(File.Exists(Path.Combine(pagesDir, "Audit.razor")));
        Assert.True(File.Exists(Path.Combine(devDir, "Users.razor")));
        Assert.True(File.Exists(Path.Combine(devDir, "Database.razor")));
        Assert.True(File.Exists(Path.Combine(devDir, "DeveloperAudit.razor")));

        // Team feature files
        var teamDir = Path.Combine(projectPath, "PlatformCheck", "Features", "Team");
        Assert.True(File.Exists(Path.Combine(teamDir, "AppTeamService.cs")));
        Assert.True(File.Exists(Path.Combine(teamDir, "AppUserService.cs")));
        Assert.True(File.Exists(Path.Combine(teamDir, "TeamEntity.cs")));

        // Config
        var appsettings = await File.ReadAllTextAsync(Path.Combine(projectPath, "PlatformCheck", "appsettings.json"));
        Assert.Contains("AzureAd", appsettings);
        Assert.Contains("ConnectionStrings", appsettings);

        // Program.cs — all platform registrations, no template directives
        var program = await File.ReadAllTextAsync(Path.Combine(projectPath, "PlatformCheck", "Program.cs"));
        Assert.Contains("AddThargaAuth", program);
        Assert.Contains("AddThargaControllers", program);
        Assert.Contains("AddThargaTeamBlazor", program);
        Assert.Contains("AddThargaApiKeys", program);
        Assert.Contains("AddThargaScopes", program);
        Assert.Contains("AddThargaTenantRoles", program);
        Assert.Contains("AddThargaAuditLogging", program);
        Assert.DoesNotContain("#if", program);
        Assert.DoesNotContain("#endif", program);

        // NavMenu
        var navMenu = await File.ReadAllTextAsync(Path.Combine(projectPath, "PlatformCheck", "Components", "Layout", "NavMenu.razor"));
        Assert.Contains("LoginDisplay", navMenu);
        Assert.Contains("TeamSelector", navMenu);
        Assert.Contains("LanguageSelector", navMenu);
        Assert.DoesNotContain("//-", navMenu);
        Assert.DoesNotContain("#if", navMenu);

        // Name substitution
        Assert.Contains("PlatformCheck", program);
        Assert.DoesNotContain("Tharga.Blazor1", program);
    }

    [Fact]
    [Trait("Category", "Slow")]
    public async Task NoSamples_Builds()
    {
        var projectPath = await TemplateTestHelper.CreateProjectAsync(_tempDir, "tharga-blazor-platform", "PlatformNoSamp", "--IncludeSamples false");
        await TemplateTestHelper.AssertBuildsAsync(projectPath);
    }

    [Fact]
    [Trait("Category", "Slow")]
    public async Task AllOptionsDisabled_Builds()
    {
        var projectPath = await TemplateTestHelper.CreateProjectAsync(_tempDir, "tharga-blazor-platform", "PlatformMin",
            "--IncludeSamples false --IncludeHealth false --IncludeRateLimiting false --IncludeQuilt4Net false");
        await TemplateTestHelper.AssertBuildsAsync(projectPath);

        // Quilt4Net pages excluded
        Assert.False(File.Exists(Path.Combine(projectPath, "PlatformMin", "Components", "Developer", "Log.razor")));
        Assert.False(File.Exists(Path.Combine(projectPath, "PlatformMin", "Components", "Developer", "LogDetail.razor")));
        Assert.False(File.Exists(Path.Combine(projectPath, "PlatformMin", "Components", "Developer", "LogSummary.razor")));
        Assert.False(File.Exists(Path.Combine(projectPath, "PlatformMin", "Components", "Developer", "Content.razor")));
        Assert.False(File.Exists(Path.Combine(projectPath, "PlatformMin", "Components", "Developer", "Configuration.razor")));

        // NavMenu excludes LanguageSelector
        var navMenu = await File.ReadAllTextAsync(Path.Combine(projectPath, "PlatformMin", "Components", "Layout", "NavMenu.razor"));
        Assert.DoesNotContain("LanguageSelector", navMenu);
    }

    [Fact]
    public async Task DatabasePageHasMonitorToolbar()
    {
        var projectPath = await TemplateTestHelper.CreateProjectAsync(_tempDir, "tharga-blazor-platform", "PlatformDb");
        var db = await File.ReadAllTextAsync(Path.Combine(projectPath, "PlatformDb", "Components", "Developer", "Database.razor"));
        Assert.Contains("MonitorToolbar", db);
        Assert.Contains("OnCallsReset", db);
        Assert.Contains("OnCacheReset", db);
    }

    [Fact]
    public async Task LogPageSupportsDeepLinking()
    {
        var projectPath = await TemplateTestHelper.CreateProjectAsync(_tempDir, "tharga-blazor-platform", "PlatformLog");
        var log = await File.ReadAllTextAsync(Path.Combine(projectPath, "PlatformLog", "Components", "Developer", "Log.razor"));
        Assert.Contains("DetailPath=\"/developer/log/detail\"", log);
        Assert.Contains("SummaryPath=\"/developer/log/summary\"", log);
        Assert.Contains("Tab=\"@Tab\"", log);

        Assert.True(File.Exists(Path.Combine(projectPath, "PlatformLog", "Components", "Developer", "LogDetail.razor")));
        Assert.True(File.Exists(Path.Combine(projectPath, "PlatformLog", "Components", "Developer", "LogSummary.razor")));
    }

    [Fact]
    [Trait("Category", "Slow")]
    public async Task IncludeCache_BuildsAndContainsCacheArtifacts()
    {
        var projectPath = await TemplateTestHelper.CreateProjectAsync(_tempDir, "tharga-blazor-platform", "PlatformCache",
            "--IncludeCache true");
        await TemplateTestHelper.AssertBuildsAsync(projectPath);

        var csproj = await File.ReadAllTextAsync(Path.Combine(projectPath, "PlatformCache", "PlatformCache.csproj"));
        Assert.Contains("Tharga.Cache.Blazor", csproj);

        var program = await File.ReadAllTextAsync(Path.Combine(projectPath, "PlatformCache", "Program.cs"));
        Assert.Contains("AddCache()", program);

        Assert.True(File.Exists(Path.Combine(projectPath, "PlatformCache", "Components", "Developer", "Cache.razor")));

        var navMenu = await File.ReadAllTextAsync(Path.Combine(projectPath, "PlatformCache", "Components", "Layout", "NavMenu.razor"));
        Assert.Contains("/developer/cache", navMenu);
    }

    [Fact]
    public async Task ExcludeCache_ExcludesCacheArtifacts()
    {
        var projectPath = await TemplateTestHelper.CreateProjectAsync(_tempDir, "tharga-blazor-platform", "PlatformNoCache");

        var csproj = await File.ReadAllTextAsync(Path.Combine(projectPath, "PlatformNoCache", "PlatformNoCache.csproj"));
        Assert.DoesNotContain("Tharga.Cache", csproj);

        var program = await File.ReadAllTextAsync(Path.Combine(projectPath, "PlatformNoCache", "Program.cs"));
        Assert.DoesNotContain("AddCache()", program);

        Assert.False(File.Exists(Path.Combine(projectPath, "PlatformNoCache", "Components", "Developer", "Cache.razor")));

        var navMenu = await File.ReadAllTextAsync(Path.Combine(projectPath, "PlatformNoCache", "Components", "Layout", "NavMenu.razor"));
        Assert.DoesNotContain("/developer/cache", navMenu);
    }
}
