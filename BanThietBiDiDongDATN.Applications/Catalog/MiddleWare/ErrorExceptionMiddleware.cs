using BanThietBiDiDongDATN.Application.Catalog.Commom;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System.Net;
using System.Text.Json;

namespace BanThietBiDiDongDoAn.Applications.MiddleWare
{
    public class ErrorExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        //private readonly ILogger<ErrorExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;
        public ErrorExceptionMiddleware(RequestDelegate next, /*ILogger<ErrorExceptionMiddleware> logger,*/
            IHostEnvironment env)
        {
            _env = env;
            //_logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, ex.Message);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                var response = new ApiResponse(context.Response.StatusCode);

                var json = JsonConvert.SerializeObject(response);
                await context.Response.WriteAsync(json);
            }
        }
    }
}
