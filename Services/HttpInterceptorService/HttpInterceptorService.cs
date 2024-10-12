using Microsoft.Extensions.Logging;
using Toolbelt.Blazor;

namespace Services;

public class HttpInterceptorService(HttpClientInterceptor interceptor, RefreshTokenService refreshTokenService, ILogger<HttpInterceptorService> logger)
{
    public void RegisterEvent() => interceptor.BeforeSendAsync += InterceptBeforeHttpAsync;

    public async Task InterceptBeforeHttpAsync(object sender, HttpClientInterceptorEventArgs e)
    {
        logger.LogInformation("InterceptBeforeHttpAsync called.");
        var absPath = e.Request.RequestUri.AbsolutePath;

        if (!absPath.Contains("token") && !absPath.Contains("accounts"))
        {
            var token = await refreshTokenService.TryRefreshToken();

            if (!string.IsNullOrEmpty(token))
            {
                e.Request.Headers.Authorization = new AuthenticationHeaderValue("bearer", token);
            }
        }
    }

    public void DisposeEvent() => interceptor.BeforeSendAsync -= InterceptBeforeHttpAsync;
}