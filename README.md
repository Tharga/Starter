# Tharga.Starter

Project templates for building applications with Tharga packages.

## Requirements

- [.NET 10 SDK](https://dotnet.microsoft.com/download). All three templates target .NET 10.
- `tharga-blazor-platform` also needs an Entra ID app registration and a MongoDB database to run. See the
  README inside the generated project for the setup steps.

## Install

```bash
dotnet new install Tharga.Starter
```

## Available templates

| Template | Short name | Description |
|----------|-----------|-------------|
| Tharga Blazor | `tharga-blazor` | Blazor web application with Tharga UI foundation |
| Tharga Blazor Platform | `tharga-blazor-platform` | Blazor web application with full Tharga Platform (auth, teams, API, audit) |
| Tharga Console | `tharga-console` | Console application with Tharga packages |

## Usage

```bash
dotnet new tharga-blazor -n MyApp
dotnet new tharga-blazor-platform -n MyPlatformApp
dotnet new tharga-console -n MyWorker
```

Turn features on or off with the options listed under [Template options](#template-options), for example:

```bash
dotnet new tharga-blazor-platform -n MyPlatformApp --IncludeMcp false --IncludeCache true
```

## Template options

Every option is a `true` or `false` value, passed as `--OptionName value`.

| Option | `tharga-blazor` | `tharga-blazor-platform` | `tharga-console` | What it controls |
|--------|:---:|:---:|:---:|------------------|
| `IncludeSamples` | true | true | | Counter and Weather sample pages |
| `IncludeSample` | | | true | Sample command code (note: singular) |
| `IncludeHealth` | true | true | | Quilt4Net health endpoints |
| `IncludeRateLimiting` | true | true | | Rate limiting middleware |
| `IncludeQuilt4Net` | | true | | Quilt4Net logging, remote configuration and content, plus the developer log pages |
| `IncludeCache` | | false | | Tharga.Cache with a developer page for cache monitoring |
| `IncludeMcp` | | true | | An MCP endpoint (`Tharga.Team.Mcp`) so agents can use the app with an API key |
| `IncludeEntra` | | true | | The Entra user directory (`Tharga.Team.Entra`); it only activates when `AzureAd:ClientSecret` is set |
| `IncludeDynamicRoles` | | false | | Team administrators can define their own roles at runtime, on a Roles page |

The values shown are the defaults.

## Features

The templates build on the Tharga and Quilt4Net package ecosystem. Each feature refers to its own package documentation for detailed integration instructions.

**Legend:** `x` = included by default, `opt` = included only when you turn its option on

### [Tharga](https://github.com/Tharga) packages

| # | Feature | Package | Repo | Depends on | `tharga-blazor` | `tharga-blazor-platform` |
|---|---------|---------|------|------------|:---:|:---:|
| 1 | Radzen UI via Tharga.Blazor | `Tharga.Blazor` | [Blazor](https://github.com/Tharga/Blazor) | — | x | x |
| 2 | Breadcrumbs, About page, Error boundary | `Tharga.Blazor` | [Blazor](https://github.com/Tharga/Blazor) | — | x | x |
| 3 | Authentication (Entra ID) | `Tharga.Team.Blazor` | [Team](https://github.com/Tharga/Team) | 1–2 | | x |
| 4 | MongoDB | `Tharga.MongoDB.Blazor` | [MongoDB](https://github.com/Tharga/MongoDB) | 3 | | x |
| 5 | Team management | `Tharga.Team.MongoDB` | [Team](https://github.com/Tharga/Team) | 3, 4 | | x |
| 6 | API controllers | `Tharga.Team.Service` | [Team](https://github.com/Tharga/Team) | 3 | | x |
| 7 | API key authentication | `Tharga.Team.Service` | [Team](https://github.com/Tharga/Team) | 5, 6 | | x |
| 8 | Scopes | `Tharga.Team.Service` | [Team](https://github.com/Tharga/Team) | 5 | | x |
| 9 | Tenant roles | `Tharga.Team` | [Team](https://github.com/Tharga/Team) | 8 | | x |
| 10 | Audit logging (logged and stored in MongoDB) | `Tharga.Team.Service` | [Team](https://github.com/Tharga/Team) | 5 | | x |
| 11 | Developer role, granted from `Developers:Emails`, with system scopes | `Tharga.Team.Blazor` | [Team](https://github.com/Tharga/Team) | 8 | | x |
| 12 | Access simulation and team consent | `Tharga.Team.Blazor` | [Team](https://github.com/Tharga/Team) | 8 | | x |
| 13 | Entra user directory | `Tharga.Team.Entra` | [Team](https://github.com/Tharga/Team) | 3 | | x |
| 14 | MCP endpoint | `Tharga.Team.Mcp` | [Team](https://github.com/Tharga/Team) | 7 | | x |
| 15 | Runtime-defined roles | `Tharga.Team.Blazor` | [Team](https://github.com/Tharga/Team) | 9 | | opt |
| 16 | Caching | `Tharga.Cache.Blazor` | [Cache](https://github.com/Tharga/Cache) | — | | opt |

Options: 13 `IncludeEntra`, 14 `IncludeMcp`, 15 `IncludeDynamicRoles`, 16 `IncludeCache`. Items 13 and 14 are on by
default and can be turned off.

### [Quilt4Net](https://github.com/Quilt4/Quilt4Net.Toolkit) packages

| # | Feature | Package | Depends on | `tharga-blazor` | `tharga-blazor-platform` |
|---|---------|---------|------------|:---:|:---:|
| 17 | Health endpoints | `Quilt4Net.Toolkit.Health` | — | x | x |
| 18 | Logging, remote configuration and content, with developer log pages | `Quilt4Net.Toolkit.Api`, `Quilt4Net.Toolkit.Blazor` | — | | x |

Options: 17 `IncludeHealth`, 18 `IncludeQuilt4Net`. Both are on by default and can be turned off.

### Built-in

| # | Feature | Package | Depends on | `tharga-blazor` | `tharga-blazor-platform` |
|---|---------|---------|------------|:---:|:---:|
| 19 | Rate limiting | `Microsoft.AspNetCore.RateLimiting` | — | x | x |

Option: 19 `IncludeRateLimiting`. It is on by default and can be turned off.

### Dependency graph

```
1–2: Tharga.Blazor (Radzen, Error boundary, Breadcrumbs, About)
  │
  3: Authentication (Tharga.Team.Blazor)
  │
  ├── 4: MongoDB
  │     └── 5: Team management
  │           ├── 6:  API controllers
  │           │     └── 7: API key auth ── 14: MCP endpoint
  │           ├── 8: Scopes ── 9: Tenant roles ── 15: Runtime roles (opt)
  │           │     ├── 11: Developer role
  │           │     └── 12: Simulation and consent
  │           └── 10: Audit logging
  │
  ├── 13: Entra user directory
  ├── 16: Caching (independent, opt)
  ├── 17: Health endpoints (independent)
  ├── 18: Quilt4Net logging and content (independent)
  └── 19: Rate limiting (independent)
```

## Development

Install templates locally for testing:

```bash
dotnet new install ./templates/blazor
dotnet new install ./templates/blazor-platform
dotnet new install ./templates/console
```

Uninstall:

```bash
dotnet new uninstall ./templates/blazor
dotnet new uninstall ./templates/blazor-platform
dotnet new uninstall ./templates/console
```

Run the tests one class at a time. The classes share the global `dotnet new` template store, so running them in
parallel is unreliable:

```bash
dotnet test tests/Tharga.Starter.Tests -c Release --filter "FullyQualifiedName~.BlazorPlatformTemplateTests"
```
