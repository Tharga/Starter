# Rate Limiting

The `tharga-blazor` template includes an optional rate limiting feature using the built-in ASP.NET Core `Microsoft.AspNetCore.RateLimiting` middleware.

Enable it when creating a project:

```bash
dotnet new tharga-blazor -n MyApp --IncludeRateLimiting true
```

## Default configuration

The template sets up a **FixedWindow** rate limiter with these defaults:

| Setting | Default | Description |
|---------|---------|-------------|
| PermitLimit | 100 | Max requests allowed per window |
| Window | 1 minute | Time window duration |
| Partitioning | Per IP address | Each client IP gets its own limit |
| RejectionStatusCode | 429 | HTTP status returned when limit is exceeded |

## Customizing

After creating your project, modify the rate limiter in `Program.cs`:

```csharp
builder.Services.AddRateLimiter(options =>
{
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 100,    // Change to your desired limit
                Window = TimeSpan.FromMinutes(1)  // Change to your desired window
            }));
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
});
```

## Alternative rate limiter types

ASP.NET Core provides several built-in rate limiter algorithms:

### SlidingWindow

Divides the window into segments for smoother rate limiting:

```csharp
RateLimitPartition.GetSlidingWindowLimiter(partitionKey, _ => new SlidingWindowRateLimiterOptions
{
    PermitLimit = 100,
    Window = TimeSpan.FromMinutes(1),
    SegmentsPerWindow = 4  // 4 segments of 15 seconds each
});
```

### TokenBucket

Allows bursts while maintaining an average rate:

```csharp
RateLimitPartition.GetTokenBucketLimiter(partitionKey, _ => new TokenBucketRateLimiterOptions
{
    TokenLimit = 100,
    ReplenishmentPeriod = TimeSpan.FromMinutes(1),
    TokensPerPeriod = 50
});
```

### Concurrency

Limits the number of concurrent requests rather than rate:

```csharp
RateLimitPartition.GetConcurrencyLimiter(partitionKey, _ => new ConcurrencyLimiterOptions
{
    PermitLimit = 10
});
```

## Per-endpoint rate limiting

Apply rate limiting to specific controllers or endpoints using the `[EnableRateLimiting]` attribute:

```csharp
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("api", limiterOptions =>
    {
        limiterOptions.PermitLimit = 50;
        limiterOptions.Window = TimeSpan.FromMinutes(1);
    });
});
```

```csharp
[EnableRateLimiting("api")]
public class MyController : ControllerBase { }
```

## Further reading

- [ASP.NET Core Rate Limiting documentation](https://learn.microsoft.com/en-us/aspnet/core/performance/rate-limit)
