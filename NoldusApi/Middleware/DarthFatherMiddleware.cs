using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using NoldusApi.Dtos;
using NoldusApi.Dtos.AuthorDtos;
using NoldusApi.Middleware;

namespace NoldusApi.Middleware
{
    public class DarthFatherMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IMapper _mapper;

        public DarthFatherMiddleware(RequestDelegate next, IMapper mapper)
        {
            _next = next;
            _mapper = mapper;
        }
        
        private async Task<string> ReadSteamToString(HttpContext context)
        {
            if (!context.Request.Body.CanSeek)
            {
                // We only do this if the stream isn't *already* seekable,
                // as EnableBuffering will create a new stream instance
                // each time it's called
                context.Request.EnableBuffering();
            }

            context.Request.Body.Position = 0;

            var reader = new StreamReader(context.Request.Body, Encoding.UTF8);

            var body = await reader.ReadToEndAsync().ConfigureAwait(false);

            context.Request.Body.Position = 0;

            return body;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Method != "POST")
            {
                await _next(context);
                return;
            }
            
            if (!context.Request.Path.HasValue)
            {
                // bad request
                await _next(context);
                return;
            }
            
            string path = context.Request.Path.Value!;
            if (path.ToLower() == "/api/author")
            {
                var body = await ReadSteamToString(context);
                var author = JsonConvert.DeserializeObject<AuthorWriteDto>(body);
                if (author.Pseudonym != null)
                {               
                    if (author.Pseudonym.ToLower() == "dark father")
                    {
                        context.Response.StatusCode = 403;
                        return;
                    }
                }

            } else if (path.ToLower() == "api/authors")
            {
                var body = await ReadSteamToString(context);
                var authors = JsonConvert.DeserializeObject<IEnumerable<AuthorWriteDto>>(body);
            
                if (!authors.Any(x => x.Pseudonym == null))
                {
                    if (authors.Any(x => x.Pseudonym.ToLower() == "dark father"))
                    {
                        context.Response.StatusCode = 403;
                        return;
                    }
                }
            }

            await _next(context);
        }
    }
}

// Extension method used to add the middleware to the HTTP request pipeline.
public static class MyMiddlewareExtensions
{
    public static IApplicationBuilder UseMyMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<DarthFatherMiddleware>();
    }
} 