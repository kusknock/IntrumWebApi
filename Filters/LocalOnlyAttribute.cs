using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace IntrumWebApi.Filters
{
    /// <summary>
    /// Атрибут, который позволяет локально или с определенного IP вызывать методы или контроллер
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class LocalOnlyAttribute : Attribute, IAuthorizationFilter
    {
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="context">Контекст запроса</param>
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // получаем ip, если он есть в белом списке
            var host = context.HttpContext.Connection?.RemoteIpAddress?.ToString();

            // проверяем "проверки" [условия]
            if (host != "localhost" || host != "::1")
                return; // даем вызывающему методу работать дальше

            context.Result = new UnauthorizedResult(); // не даем
        }
    }
}
