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

| # | Feature | Package | Depends on | Repo | `tharga-blazor` |
|---|---------|---------|------------|------|:---:|
| 1 | Radzen UI via Tharga.Blazor | `Tharga.Blazor` | — | [Tharga/Platform](https://github.com/Tharga/Platform) | x |
| 2 | Breadcrumbs, About page, Error boundary | `Tharga.Blazor` | — | [Tharga/Platform](https://github.com/Tharga/Platform) | x |
| 3 | Authentication (Entra ID) | `Tharga.Team.Blazor` | 1–2 | [Tharga/Platform](https://github.com/Tharga/Platform) | |
| 4 | API Logging | `Quilt4Net.Toolkit.Api` | 3 | [Quilt4/Quilt4Net.Toolkit](https://github.com/Quilt4/Quilt4Net.Toolkit) | |
| 5 | Remote Configuration & Feature Toggle | `Quilt4Net.Toolkit.Blazor` | 4 | [Quilt4/Quilt4Net.Toolkit](https://github.com/Quilt4/Quilt4Net.Toolkit) | |
| 6 | Content & Localization | `Quilt4Net.Toolkit.Blazor` | 4 | [Quilt4/Quilt4Net.Toolkit](https://github.com/Quilt4/Quilt4Net.Toolkit) | |
| 7 | MongoDB | `Tharga.MongoDB.Blazor` | 3 | [Tharga/MongoDB](https://github.com/Tharga/MongoDB) | |
| 8 | Team Management | `Tharga.Team.MongoDB` | 3, 7 | [Tharga/Platform](https://github.com/Tharga/Platform) | |
| 9 | API Controllers | `Tharga.Blazor` (Tharga.Api) | 3 | [Tharga/Platform](https://github.com/Tharga/Platform) | |
| 10 | Rate Limiting | Built-in | — | — | opt |
| 11 | Scopes | `Tharga.Blazor` (Tharga.Api) | 3, 8 | [Tharga/Platform](https://github.com/Tharga/Platform) | |
| 12 | Team Roles | `Tharga.Blazor` (Tharga.Api) | 11 | [Tharga/Platform](https://github.com/Tharga/Platform) | |
| 13 | Audit Logging | `Tharga.Blazor` (Tharga.Api) | 7, 8 | [Tharga/Platform](https://github.com/Tharga/Platform) | |
| 14 | Health Endpoints | `Quilt4Net.Toolkit.Health` | — | [Quilt4/Quilt4Net.Toolkit](https://github.com/Quilt4/Quilt4Net.Toolkit) | opt |

**Legend:** `x` = included by default, `opt` = optional flag

Optional flags: `--IncludeHealth true`, `--IncludeRateLimiting true`

For detailed package documentation, see:
- [Tharga Platform](https://github.com/Tharga/Platform) — Tharga.Blazor, authentication, team management, API controllers, scopes, audit
- [Quilt4Net.Toolkit](https://github.com/Quilt4/Quilt4Net.Toolkit) — health endpoints, logging, remote configuration, content
- [Tharga MongoDB](https://github.com/Tharga/MongoDB) — MongoDB integration

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
