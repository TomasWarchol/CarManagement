using Microsoft.AspNetCore.Mvc;


public class HeaderMiddleware
{
    private readonly RequestDelegate _next;

    public HeaderMiddleware(RequestDelegate next) => _next = next;

    public async Task Invoke(HttpContext context)
    {
        var userAgent = context.Request.Headers["User-Agent"].ToString();
        Console.WriteLine($"User-Agent: {userAgent}");
        await _next(context);
    }
}
