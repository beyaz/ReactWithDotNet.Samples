using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using ReactWithDotNet.UIDesigner;

namespace CounterSample;

public static class ReactWithDotNetIntegration
{
    public static void ConfigureReactWithDotNet(this WebApplication app)
    {
        app.UseMiddleware<ReactWithDotNetJavaScriptFiles>();

        var routeMap = typeof(PageList)
            .GetFields(BindingFlags.Static | BindingFlags.Public)
            .Where(f => f.FieldType == typeof(PageRouteInfo))
            .Select(f => (PageRouteInfo)f.GetValue(null))
            .Where(x => x is not null)
            .ToDictionary(x => x.Url, x => x, StringComparer.OrdinalIgnoreCase);

        RequestHandlerPath = "/" + nameof(HandleReactWithDotNetRequest);

        app.Use(async (httpContext, next) =>
        {
            var path = httpContext.Request.Path.Value ?? string.Empty;

            if (path == RequestHandlerPath)
            {
                await HandleReactWithDotNetRequest(httpContext);
                return;
            }

            if (routeMap.TryGetValue(path, out var routeInfo))
            {
                await WriteHtmlResponse(httpContext, typeof(MainLayout), routeInfo.page);
                return;
            }

            #if DEBUG
            if (path == ReactWithDotNetDesigner.UrlPath)
            {
                await WriteHtmlResponse(httpContext, typeof(MainLayout), typeof(ReactWithDotNetDesigner));
                return;
            }
            #endif

            await next();
        });
    }

    static Task HandleReactWithDotNetRequest(HttpContext httpContext)
    {
        httpContext.Response.ContentType = "application/json; charset=utf-8";

        return ProcessReactWithDotNetComponentRequest(new()
        {
            HttpContext           = httpContext,
            OnReactContextCreated = OnReactContextCreated,
            OnReactContextDisposed = OnReactContextDisposed
        });
    }


    static Task OnReactContextDisposed(ReactContext reactcontext, Exception exception)
    {
        // you can dispose db connections or user specific informations
        
        return Task.CompletedTask;
    }

    static Task OnReactContextCreated(ReactContext context)
    {
        // you can open db connections or user specific informations to context
        
        return Task.CompletedTask;
    }

    static Task WriteHtmlResponse(HttpContext httpContext, Type layoutType, Type mainContentType)
    {
        httpContext.Response.ContentType = "text/html; charset=UTF-8";

        httpContext.Response.Headers[HeaderNames.CacheControl] = "no-cache, no-store, must-revalidate";
        httpContext.Response.Headers[HeaderNames.Expires]      = "0";
        httpContext.Response.Headers[HeaderNames.Pragma]       = "no-cache";

        return ProcessReactWithDotNetPageRequest(new()
        {
            LayoutType            = layoutType,
            MainContentType       = mainContentType,
            HttpContext           = httpContext,
            OnReactContextCreated = OnReactContextCreated
        });
    }
}