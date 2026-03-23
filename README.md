# Tharga.Starter

Project templates for building applications with Tharga packages.

## Install

```bash
dotnet new install Tharga.Starter
```

## Available templates

| Template | Short name | Description |
|----------|-----------|-------------|
| Tharga Blazor | `tharga-blazor` | Blazor web application with Tharga packages |
| Tharga Console | `tharga-console` | Console application with Tharga packages |

## Usage

```bash
dotnet new tharga-blazor -n MyApp
dotnet new tharga-console -n MyWorker
```

## Features

The templates build on the Tharga and Quilt4Net package ecosystem. Each feature refers to its own package documentation for detailed integration instructions.

**Legend:** `x` = included by default, `opt` = optional flag

### [Tharga](https://github.com/Tharga) packages

| # | Feature | Package | Repo | Depends on | `tharga-blazor` |
|---|---------|---------|------|------------|:---:|
| 1 | Radzen UI via Tharga.Blazor | `Tharga.Blazor` | [Platform](https://github.com/Tharga/Platform) | — | x |
| 2 | Breadcrumbs, About page, Error boundary | `Tharga.Blazor` | [Platform](https://github.com/Tharga/Platform) | — | x |
| 3 | Authentication (Entra ID) | `Tharga.Team.Blazor` | [Platform](https://github.com/Tharga/Platform) | 1–2 | |
| 7 | MongoDB | `Tharga.MongoDB.Blazor` | [MongoDB](https://github.com/Tharga/MongoDB) | 3 | |
| 8 | Team Management | `Tharga.Team.MongoDB` | [Platform](https://github.com/Tharga/Platform) | 3, 7 | |
| 9 | API Controllers | `Tharga.Blazor` (Tharga.Api) | [Platform](https://github.com/Tharga/Platform) | 3 | |
| 11 | Scopes | `Tharga.Blazor` (Tharga.Api) | [Platform](https://github.com/Tharga/Platform) | 3, 8 | |
| 12 | Team Roles | `Tharga.Blazor` (Tharga.Api) | [Platform](https://github.com/Tharga/Platform) | 11 | |
| 13 | Audit Logging | `Tharga.Blazor` (Tharga.Api) | [Platform](https://github.com/Tharga/Platform) | 7, 8 | |

### [Quilt4Net](https://github.com/Quilt4/Quilt4Net.Toolkit) packages

| # | Feature | Package | Depends on | `tharga-blazor` |
|---|---------|---------|------------|:---:|
| 4 | API Logging | `Quilt4Net.Toolkit.Api` | Tharga #3 | |
| 5 | Remote Configuration & Feature Toggle | `Quilt4Net.Toolkit.Blazor` | #4 | |
| 6 | Content & Localization | `Quilt4Net.Toolkit.Blazor` | #4 | |
| 14 | Health Endpoints | `Quilt4Net.Toolkit.Health` | — | opt |

### Built-in

| # | Feature | Package | Depends on | `tharga-blazor` |
|---|---------|---------|------------|:---:|
| 10 | Rate Limiting | `Microsoft.AspNetCore.RateLimiting` | — | [opt](docs/rate-limiting.md) |

Optional flags: `--IncludeHealth true`, `--IncludeRateLimiting true`

As more template variants are added (e.g. `tharga-blazor-platform`, `tharga-blazor-full`), additional columns will show which features each template includes.

### Dependency graph

```
1–2: Tharga.Blazor (Radzen, Error boundary, Breadcrumbs, About)
  │
  3: Authentication (Tharga.Team.Blazor)
  │
  ├── 4: Quilt4Net Logging
  │     ├── 5: Remote Configuration
  │     └── 6: Content & Localization
  │
  ├── 7: MongoDB
  │     └── 8: Team Management
  │           ├── 9:  API Controllers
  │           ├── 11: Scopes → 12: Team Roles
  │           └── 13: Audit Logging
  │
  ├── 10: Rate Limiting
  └── 14: Health Endpoints (independent)
```

## Development

Install templates locally for testing:

```bash
dotnet new install ./templates/blazor
dotnet new install ./templates/console
```

Uninstall:

```bash
dotnet new uninstall ./templates/blazor
dotnet new uninstall ./templates/console
```
