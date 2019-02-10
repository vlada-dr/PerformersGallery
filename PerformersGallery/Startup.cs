using System.Net.Http;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PerformersGallery.Helpers;
using PerformersGallery.Models;
using PerformersGallery.Services;
using Swashbuckle.AspNetCore.Swagger;

namespace PerformersGallery
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            Environment = env;
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment Environment { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // comment this 2 lines for using in-memory db
            string connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<GalleryContext>(options =>
                options.UseSqlServer(connection));
            // uncomment this line for using in-memory db
            // services.AddDbContext<GalleryContext>(opt => opt.UseInMemoryDatabase("GalleryDB"));
            services.AddMvc();
            services.AddCors();
            services.AddAutoMapper();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddSingleton(new SecretsService
            {
                FacePlusPlusKey = Configuration.GetSection("Profile:FacePlusPlusKey").Value,
                FacePlusPlusSecret = Configuration.GetSection("Profile:FacePlusPlusSecret").Value,
                FlickrKey = Configuration.GetSection("Profile:FlickrKey").Value,
                FlickrSecret = Configuration.GetSection("Profile:FlickrSecret").Value
            });
            services.AddTransient<FlickrService>();
            services.AddTransient<FaceService>();
            services.AddTransient<GalleryService>();
            services.AddSingleton<HttpClient>();
            AdditionalData.Initialize(Environment.ContentRootPath);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Title = AdditionalData.Info.Swagger.Title.Value,
                    Version = AdditionalData.Info.Swagger.Version.Value,
                    Description = AdditionalData.Info.Swagger.Description.Value,
                    Contact = new Contact
                    {
                        Name = AdditionalData.Info.Swagger.Name.Value
                    }
                });
            });
            services.AddMemoryCache();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseCors(builder => builder.AllowAnyOrigin());
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint(
                    AdditionalData.Info.Swagger.EndpointUrl.Value as string,
                     AdditionalData.Info.Swagger.EndpointName.Value as string);
                c.RoutePrefix = string.Empty;
            });

            if (env.IsDevelopment())
            {
                // app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}