using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanThietBiDiDongDATN.Application.Catalog.Commom
{
    public class StatusCodeRedirectMiddleware
    {
        private readonly RequestDelegate _next;

        public StatusCodeRedirectMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            //try
            //{
                await _next(context);
                if (context.Response.StatusCode == 400)
                {
                    context.Response.Redirect("/Error/400");
                }
                if (context.Response.StatusCode == 404)
                {
                    context.Response.Redirect("/Error/404");
                }
                else if (context.Response.StatusCode == 500)
                {
                    context.Response.Redirect("/Error/500");
                }
            //}catch (Exception ex)
            //{
            //    context.Response.Redirect("/Error/500");
            //}
        }
    }
}
