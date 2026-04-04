using Xunit;

namespace Tharga.Starter.Tests;

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
    public async Task Builds()
    {
        var projectPath = await TemplateTestHelper.CreateProjectAsync(_tempDir, "tharga-blazor-platform", "PlatformBuild");
        await TemplateTestHelper.AssertBuildsAsync(projectPath);
    }

    [Fact]
    public async Task ContainsServerProject()
    {
        var projectPath = await TemplateTestHelper.CreateProjectAsync(_tempDir, "tharga-blazor-platform", "PlatformServer");
        Assert.True(Directory.Exists(Path.Combine(projectPath, "PlatformServer")),
            "Server project folder should exist");
        Assert.True(File.Exists(Path.Combine(projectPath, "PlatformServer", "PlatformServer.csproj")),
            "Server project file should exist");
    }

    [Fact]
    public async Task ContainsClientProject()
    {
        var projectPath = await TemplateTestHelper.CreateProjectAsync(_tempDir, "tharga-blazor-platform", "PlatformClient");
        Assert.True(Directory.Exists(Path.Combine(projectPath, "PlatformClient.Client")),
            "Client project folder should exist");
    }

    [Fact]
    public async Task ContainsTestProjects()
    {
        var projectPath = await TemplateTestHelper.CreateProjectAsync(_tempDir, "tharga-blazor-platform", "PlatformTests");
        Assert.True(Directory.Exists(Path.Combine(projectPath, "PlatformTests.Tests")),
            "Unit test project folder should exist");
        Assert.True(Directory.Exists(Path.Combine(projectPath, "PlatformTests.IntegrationTests")),
            "Integration test project folder should exist");
    }

    [Fact]
    public async Task ContainsPlatformPackageReferences()
    {
        var projectPath = await TemplateTestHelper.CreateProjectAsync(_tempDir, "tharga-blazor-platform", "PlatformPkgs");
        var csproj = await File.ReadAllTextAsync(Path.Combine(projectPath, "PlatformPkgs", "PlatformPkgs.csproj"));
        Assert.Contains("Tharga.Team.Blazor", csproj);
        Assert.Contains("Tharga.Team.Service", csproj);
        Assert.Contains("Tharga.Team.MongoDB", csproj);
    }

    [Fact]
    public async Task ContainsTeamFeatureFiles()
    {
        var projectPath = await TemplateTestHelper.CreateProjectAsync(_tempDir, "tharga-blazor-platform", "PlatformTeam");
        Assert.True(File.Exists(Path.Combine(projectPath, "PlatformTeam", "Features", "Team", "AppTeamService.cs")),
            "AppTeamService should exist");
        Assert.True(File.Exists(Path.Combine(projectPath, "PlatformTeam", "Features", "Team", "AppUserService.cs")),
            "AppUserService should exist");
        Assert.True(File.Exists(Path.Combine(projectPath, "PlatformTeam", "Features", "Team", "TeamEntity.cs")),
            "TeamEntity should exist");
    }

    [Fact]
    public async Task ContainsPlatformPages()
    {
        var projectPath = await TemplateTestHelper.CreateProjectAsync(_tempDir, "tharga-blazor-platform", "PlatformPages");
        var pagesDir = Path.Combine(projectPath, "PlatformPages", "Components", "Pages");
        var devDir = Path.Combine(projectPath, "PlatformPages", "Components", "Developer");
        Assert.True(File.Exists(Path.Combine(pagesDir, "Profile.razor")), "Profile page should exist");
        Assert.True(File.Exists(Path.Combine(pagesDir, "Teams.razor")), "Teams page should exist");
        Assert.True(File.Exists(Path.Combine(pagesDir, "ApiKeys.razor")), "ApiKeys page should exist");
        Assert.True(File.Exists(Path.Combine(pagesDir, "Audit.razor")), "Audit page should exist");
        Assert.True(File.Exists(Path.Combine(devDir, "Users.razor")), "Developer Users page should exist");
        Assert.True(File.Exists(Path.Combine(devDir, "Database.razor")), "Developer Database page should exist");
        Assert.True(File.Exists(Path.Combine(devDir, "DeveloperAudit.razor")), "Developer Audit page should exist");
    }

    [Fact]
    public async Task ContainsAzureAdConfig()
    {
        var projectPath = await TemplateTestHelper.CreateProjectAsync(_tempDir, "tharga-blazor-platform", "PlatformConfig");
        var appsettings = await File.ReadAllTextAsync(Path.Combine(projectPath, "PlatformConfig", "appsettings.json"));
        Assert.Contains("AzureAd", appsettings);
        Assert.Contains("ConnectionStrings", appsettings);
    }

    [Fact]
    public async Task ProgramContainsPlatformRegistrations()
    {
        var projectPath = await TemplateTestHelper.CreateProjectAsync(_tempDir, "tharga-blazor-platform", "PlatformProg");
        var program = await File.ReadAllTextAsync(Path.Combine(projectPath, "PlatformProg", "Program.cs"));
        Assert.Contains("AddThargaAuth", program);
        Assert.Contains("AddThargaControllers", program);
        Assert.Contains("AddThargaTeamBlazor", program);
        Assert.Contains("AddThargaApiKeys", program);
        Assert.Contains("AddThargaScopes", program);
        Assert.Contains("AddThargaTenantRoles", program);
        Assert.Contains("AddThargaAuditLogging", program);
        Assert.DoesNotContain("#if", program);
        Assert.DoesNotContain("#endif", program);
    }

    [Fact]
    public async Task BuildsWithoutSamples()
    {
        var projectPath = await TemplateTestHelper.CreateProjectAsync(_tempDir, "tharga-blazor-platform", "PlatformNoSamp", "--IncludeSamples false");
        await TemplateTestHelper.AssertBuildsAsync(projectPath);
    }

    [Fact]
    public async Task BuildsWithHealth()
    {
        var projectPath = await TemplateTestHelper.CreateProjectAsync(_tempDir, "tharga-blazor-platform", "PlatformHealth", "--IncludeHealth true");
        await TemplateTestHelper.AssertBuildsAsync(projectPath);
    }

    [Fact]
    public async Task BuildsWithRateLimiting()
    {
        var projectPath = await TemplateTestHelper.CreateProjectAsync(_tempDir, "tharga-blazor-platform", "PlatformRate", "--IncludeRateLimiting true");
        await TemplateTestHelper.AssertBuildsAsync(projectPath);
    }

    [Fact]
    public async Task NameSubstitutionWorksInProgram()
    {
        var projectPath = await TemplateTestHelper.CreateProjectAsync(_tempDir, "tharga-blazor-platform", "MyApp");
        var program = await File.ReadAllTextAsync(Path.Combine(projectPath, "MyApp", "Program.cs"));
        Assert.Contains("MyApp", program);
        Assert.DoesNotContain("Tharga.Blazor1", program);
    }

    [Fact]
    public async Task NavMenuContainsLoginDisplay()
    {
        var projectPath = await TemplateTestHelper.CreateProjectAsync(_tempDir, "tharga-blazor-platform", "PlatformNav");
        var navMenu = await File.ReadAllTextAsync(Path.Combine(projectPath, "PlatformNav", "Components", "Layout", "NavMenu.razor"));
        Assert.Contains("LoginDisplay", navMenu);
        Assert.Contains("TeamSelector", navMenu);
        Assert.DoesNotContain("//-", navMenu);
        Assert.DoesNotContain("#if", navMenu);
    }
}
