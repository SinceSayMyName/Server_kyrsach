using System.Diagnostics;

namespace Tracker.TelegramBot.Middleware
{
    public class OptionsMiddleware 
    {
        private readonly RequestDelegate _next;

        public OptionsMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var watch = new Stopwatch();
            watch.Start();

            //To add Headers AFTER everything you need to do this
            context.Response.OnStarting(state => {

                var httpContext = (HttpContext)state;
                httpContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
                httpContext.Response.Headers.Add("Access-Control-Allow-Headers", new[] { "Content-Type, Authorization" });
                httpContext.Response.Headers.Add("Access-Control-Allow-Methods", new[] { "HEAD, GET, POST, PUT, DELETE, OPTIONS" });
                httpContext.Response.Headers.Add("Access-Control-Allow-Credentials", new[] { "true" });

                if (httpContext.Request.Method == "OPTIONS")
                {
                    httpContext.Response.StatusCode = 200;
                    return httpContext.Response.WriteAsync("OK");
                }
                return Task.CompletedTask;
            }, context);

            await _next(context);
        }
    }
}
