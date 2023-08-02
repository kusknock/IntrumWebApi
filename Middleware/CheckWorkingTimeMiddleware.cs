using ItrumWebApi.Models;
using System.Net;

namespace IntrumWebApi.Middleware
{
    public class CheckWorkingTimeMiddleware
    {

        private readonly RequestDelegate _next;

        public CheckWorkingTimeMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IConfiguration configuration)
        {
            DateTime dateTimeStart = DateTime.Parse(configuration["WorkingTime:Start"]);
            DateTime dateTimeEnd = DateTime.Parse(configuration["WorkingTime:End"]);

            DateTime now = DateTime.Now;

            if (now > dateTimeEnd || now < dateTimeStart)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                await context.Response.WriteAsync("Магазин не работает");
            }
            else
            {
                await _next.Invoke(context);
            }
        }
    }
}
