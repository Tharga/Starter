# Tharga.Starter

Project templates for building applications with Tharga packages.

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

## Features

The templates build on the Tharga and Quilt4Net package ecosystem. Each feature refers to its own package documentation for detailed integration instructions.

**Legend:** `x` = included by default, `opt` = optional flag

### [Tharga](https://github.com/Tharga) packages

| # | Feature | Package | Repo | Depends on | `tharga-blazor` | `tharga-blazor-platform` |
|---|---------|---------|------|------------|:---:|:---:|
| 1 | Radzen UI via Tharga.Blazor | `Tharga.Blazor` | [Platform](https://github.com/Tharga/Platform) | — | x | x |
| 2 | Breadcrumbs, About page, Error boundary | `Tharga.Blazor` | [Platform](https://github.com/Tharga/Platform) | — | x | x |
| 3 | Authentication (Entra ID) | `Tharga.Team.Blazor` | [Platform](https://github.com/Tharga/Platform) | 1–2 | | x |
| 7 | MongoDB | `Tharga.MongoDB.Blazor` | [MongoDB](https://github.com/Tharga/MongoDB) | 3 | | x |
| 8 | Team Management | `Tharga.Team.MongoDB` | [Platform](https://github.com/Tharga/Platform) | 3, 7 | | x |
| 9 | API Controllers | `Tharga.Team.Service` | [Platform](https://github.com/Tharga/Platform) | 3 | | x |
| 10 | API Key Authentication | `Tharga.Team.Service` | [Platform](https://github.com/Tharga/Platform) | 8, 9 | | x |
| 11 | Scopes | `Tharga.Team.Service` | [Platform](https://github.com/Tharga/Platform) | 8 | | x |
| 12 | Tenant Roles | `Tharga.Team` | [Platform](https://github.com/Tharga/Platform) | 11 | | x |
| 13 | Audit Logging | `Tharga.Team.Service` | [Platform](https://github.com/Tharga/Platform) | 8 | | x |

### [Quilt4Net](https://github.com/Quilt4/Quilt4Net.Toolkit) packages

| # | Feature | Package | Depends on | `tharga-blazor` | `tharga-blazor-platform` |
|---|---------|---------|------------|:---:|:---:|
| 14 | Health Endpoints | `Quilt4Net.Toolkit.Health` | — | opt | opt |

### Built-in

| # | Feature | Package | Depends on | `tharga-blazor` | `tharga-blazor-platform` |
|---|---------|---------|------------|:---:|:---:|
| 15 | Rate Limiting | `Microsoft.AspNetCore.RateLimiting` | — | opt | opt |

Optional flags: `--IncludeHealth true`, `--IncludeRateLimiting true`

### Dependency graph

```
1–2: Tharga.Blazor (Radzen, Error boundary, Breadcrumbs, About)
  │
  3: Authentication (Tharga.Team.Blazor)
  │
  ├── 7: MongoDB
  │     └── 8: Team Management
  │           ├── 9:  API Controllers
  │           ├── 10: API Key Auth
  │           ├── 11: Scopes → 12: Tenant Roles
  │           └── 13: Audit Logging
  │
  ├── 14: Health Endpoints (independent)
  └── 15: Rate Limiting (independent)
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
