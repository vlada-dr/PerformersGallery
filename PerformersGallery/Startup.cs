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
                FacePlusPlusKey = Configuration["FacePlusPlus:Key"],
                FacePlusPlusSecret = Configuration["FacePlusPlus:Secret"],
                FlickrKey = Configuration["Flickr:Key"],
                FlickrSecret = Configuration["Flickr:Secret"],
            });
            services.AddTransient<FlickrService>();
            services.AddTransient<FaceService>();
            services.AddTransient<GalleryService>();
            services.AddSingleton<HttpClient>();
            services.AddMemoryCache();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseCors(builder => builder.AllowAnyOrigin());

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
