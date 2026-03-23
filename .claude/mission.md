# Mission: Tharga.Starter

Dotnet project templates and step-by-step guides for building applications with Tharga packages.

## Purpose

Provide `dotnet new` templates that let users scaffold new projects with Tharga packages pre-configured. Templates are layered — from a minimal starting point to a fully configured application — so users can either start from scratch and follow the guide, or jump in at any snapshot.

## Template types

Templates are organized by application type, all within this single repository:

- **Blazor** — interactive web applications
- **Console** — background workers, CLI tools
- **API** — standalone web APIs

Each type may have multiple snapshots representing progressive stages of configuration (basic, +health, +auth, +full).

## Structure

```
templates/
  blazor/
    .template.config/template.json
    ...
  console/
    .template.config/template.json
    ...
Tharga.Starter.csproj
```

## Key packages

The templates integrate packages from the Tharga ecosystem:

- **Quilt4Net.Toolkit.Health** — health endpoints (live, ready, health, metrics, version)
- **Quilt4Net.Toolkit.Api** — API infrastructure, correlation IDs, logging middleware
- **Tharga.Toolkit** — common utilities

## Replaces the monolithic Implementation Guide

This project replaces the previous monolithic "Tharga Packages — Implementation Guide". Instead of one document describing all packages, each Tharga package project now owns its own integration instructions. The templates in this repo tie everything together — a user starts from a template and follows per-package guides to add features incrementally.

## Documentation Requests
- **Rate limiting configuration guide** — Add documentation for configuring rate limiting using the built-in ASP.NET Core `Microsoft.AspNetCore.RateLimiting` middleware. Should cover: default values, how to customize PermitLimit/Window, available rate limiter types (FixedWindow, SlidingWindow, TokenBucket), and how to apply `[EnableRateLimiting("api")]` to controllers. This was previously tracked in the Platform project but rate limiting is standard .NET, not a Tharga package feature. Requested by: Tharga.Platform.

## Guiding principles

- Each Tharga package project owns its own integration docs; templates reference them rather than duplicating
- Templates should always use the latest stable versions of Tharga packages
- Keep templates minimal — only include what the template stage promises
- One NuGet template pack (`Tharga.Starter`) publishes all templates
