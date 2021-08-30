using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using NoldusApi.DataAccess;


namespace NoldusApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            if (IsTesting())
            {
                services.AddDbContext<ApiContext>(options =>
                {
                    options.UseInMemoryDatabase("MemoryDb");
                });
            }
            else
            {
                var connString = Configuration.GetValue<string>("PostgreSqlConnection");
                services.AddDbContext<ApiContext>((_, context) => context.UseNpgsql(connString));
            }
            

            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });
            
            services.AddScoped<IAuthorRepo, AuthorRepo>();
            services.AddScoped<IBookRepo, BookRepo>();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddControllers();
            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo {Title = "NoldusApi", Version = "v1"}); });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "NoldusApi v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseFileServer(new FileServerOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(),"AppData")),
                RequestPath = "/AppData"
            });
            
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }

        private bool IsTesting()
        {
            var isTesting = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            if (isTesting == null)
            {
                return false;
            }

            if (isTesting == "testing")
            {
                return true;
            }

            return false;
        }
    }
}