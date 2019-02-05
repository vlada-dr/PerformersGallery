using Microsoft.EntityFrameworkCore;
using PerformersGallery.Models.FacePlusPlus;
using PerformersGallery.Models.Flickr;
using PerformersGallery.Models.Gallery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PerformersGallery.Models
{
    public class GalleryContext : DbContext
    {
        public GalleryContext(DbContextOptions<GalleryContext> options)
            : base(options)
        {
        }

        public DbSet<FlickrPhoto> FlickrPhotos { get; set; }
        public DbSet<GalleryPhoto> GalleryPhotos { get; set; }
    }
}
