# Tharga.Blazor1

Blazor Web App with [Tharga Platform](https://github.com/Tharga/Platform): authentication, team management, API controllers, and more.

## Prerequisites

This application requires configuration before it can run.

### 1. Azure AD / Entra ID

Create an app registration in [Azure Portal](https://portal.azure.com) or [Entra admin center](https://entra.microsoft.com):

1. Go to **App registrations** > **New registration**
2. Set a redirect URI: `https://localhost:{port}/signin-oidc`
3. Note the **Application (client) ID** and **Directory (tenant) ID**
4. For CIAM tenants, the Authority is `https://{tenant}.ciamlogin.com/{tenant-id}`
5. For standard Entra ID, the Authority is `https://login.microsoftonline.com/{tenant-id}/v2.0`

### 2. MongoDB

You need a MongoDB instance. Options:

- **Local**: Install [MongoDB Community](https://www.mongodb.com/try/download/community) or run via Docker:
  ```bash
  docker run -d -p 27017:27017 --name mongodb mongo
  ```
- **Cloud**: Create a free cluster at [MongoDB Atlas](https://www.mongodb.com/atlas)

### 3. User Secrets

Store sensitive configuration using [User Secrets](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets) (never commit these to source control):

```bash
cd Tharga.Blazor1
dotnet user-secrets init
dotnet user-secrets set "AzureAd:Authority" "https://your-tenant.ciamlogin.com/your-tenant-id"
dotnet user-secrets set "AzureAd:ClientId" "your-client-id"
dotnet user-secrets set "AzureAd:TenantId" "your-tenant-id"
dotnet user-secrets set "ConnectionStrings:Default" "mongodb://localhost:27017/Tharga.Blazor1"
```

### 4. Run

```bash
dotnet run --project Tharga.Blazor1
```

Navigate to `/swagger` to see the API documentation.

## Configuration reference

All settings go in `appsettings.json` or User Secrets:

```json
{
  "AzureAd": {
    "Authority": "",
    "ClientId": "",
    "TenantId": "",
    "CallbackPath": "/signin-oidc"
  },
  "ConnectionStrings": {
    "Default": "mongodb://localhost:27017/Tharga.Blazor1"
  }
}
```

| Setting | Required | Description |
|---------|----------|-------------|
| `AzureAd:Authority` | Yes | Identity provider URL |
| `AzureAd:ClientId` | Yes | Azure app registration client ID |
| `AzureAd:TenantId` | Yes | Azure tenant ID |
| `AzureAd:CallbackPath` | No | OAuth callback path (default: `/signin-oidc`) |
| `ConnectionStrings:Default` | Yes | MongoDB connection string |

## Features included

| Feature | Description |
|---------|-------------|
| UI Foundation | Radzen layout, buttons, breadcrumbs, error boundary |
| Authentication | Azure AD / Entra ID login with Cookie + OIDC |
| API Controllers | MVC routing, Swagger UI at `/swagger` |
| Team Management | Multi-tenant teams with MongoDB persistence |
| API Key Auth | `X-API-KEY` header authentication |
| Scopes | Fine-grained permission checks |
| Tenant Roles | Named roles that bundle scopes |
| Audit Logging | Service call and auth event logging |

## Project structure

| Project | Description |
|---------|-------------|
| `Tharga.Blazor1` | Server project (Blazor SSR + API controllers) |
| `Tharga.Blazor1.Client` | Client project (Interactive WebAssembly) |
| `Tharga.Blazor1.Tests` | Unit tests |
| `Tharga.Blazor1.IntegrationTests` | Integration tests |

## Publish

```bash
dotnet publish Tharga.Blazor1 -c Release -o ./publish
```
