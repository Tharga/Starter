# Tharga.Starter

Project templates and step-by-step guides for building applications with Tharga packages.

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
