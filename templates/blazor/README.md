# Tharga.Blazor1

Blazor Web App with Radzen UI components via [Tharga.Blazor](https://github.com/Tharga/Platform).

## Getting started

The application is ready to run out of the box:

```bash
dotnet run --project Tharga.Blazor1
```

## Optional configuration

### Health endpoints

If created with `--IncludeHealth true`, configure Quilt4Net health checks in `Program.cs`.
See [Quilt4Net.Toolkit documentation](https://github.com/Quilt4/Quilt4Net.Toolkit).

### Rate limiting

If created with `--IncludeRateLimiting true`, adjust `PermitLimit` and `Window` in `Program.cs`
to match your traffic requirements.
See [ASP.NET Core Rate Limiting](https://learn.microsoft.com/en-us/aspnet/core/performance/rate-limit).

## Project structure

| Project | Description |
|---------|-------------|
| `Tharga.Blazor1` | Server project (Blazor SSR + Interactive Server) |
| `Tharga.Blazor1.Client` | Client project (Interactive WebAssembly) |
| `Tharga.Blazor1.Tests` | Unit tests |
| `Tharga.Blazor1.IntegrationTests` | Integration tests |

## Publish

```bash
dotnet publish Tharga.Blazor1 -c Release -o ./publish
```
