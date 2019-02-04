﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PerformersGallery.Hubs;
using PerformersGallery.Services;

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
            services.AddCors();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddSignalR();
            services.AddSingleton(new SecretsService()
            {
                FacePlusPlusKey = Configuration["FacePlusPlus:Key"],
                FacePlusPlusSecret = Configuration["FacePlusPlus:Secret"],
                FlickrKey = Configuration["Flickr:Key"],
                FlickrSecret = Configuration["Flickr:Secret"],
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseCors(builder => builder.AllowAnyOrigin());
            // access to secrets Configuration["Flickr:Key"] TODO: pass to service

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