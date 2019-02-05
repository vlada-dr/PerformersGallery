using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PerformersGallery.Hubs;
using PerformersGallery.Models;
using PerformersGallery.Services;
using Swashbuckle.AspNetCore.Swagger;
using System.Net.Http;

namespace PerformersGallery
{
    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            
            Configuration = configuration;
            
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<GalleryContext>(opt => opt.UseInMemoryDatabase("GalleryDB"));
            services.AddCors();
            services.AddAutoMapper();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddSignalR();
            services.AddSingleton(new SecretsService()
            {
                FacePlusPlusKey = "-ZNoJiAUABmEq7u6BfV8FrzuGlQ9by7o", //Configuration["FacePlusPlus-Key"],
                FacePlusPlusSecret = "l4ExKKxgQkAZWExTevnz4ii53Yeqm9eK",
                //Configuration["FacePlusPlus-Secret"],
                FlickrKey = "6a652fcc8e692fc3c6422edd123eae5f",//Configuration["Flickr-Key"],
                FlickrSecret = "4534f6ab4f02fe3b"
                //Configuration["Flickr-Secret"],
            });
            services.AddTransient<FlickrService>();
            services.AddTransient<FaceService>();
            services.AddTransient<GalleryService>();
            services.AddSingleton<HttpClient>();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info {
                    Title = "PerformersGallery API for INT20H by Performers Team",
                    Version = "v1",
                    Description = "API developed by Shcherbakova Anastasiia, front end developed by Doroshenko Vlada",
                    Contact = new Contact
                    {
                        Name = "Shcherbakova Anastasiia, Doroshenko Vlada"
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
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Performers");
                c.RoutePrefix = string.Empty;
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            app.UseSignalR(routes =>
            {
                routes.MapHub<GalleryHub>("/galleryHub");
            });
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
