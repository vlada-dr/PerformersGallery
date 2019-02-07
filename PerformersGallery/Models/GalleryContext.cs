using Microsoft.EntityFrameworkCore;
using PerformersGallery.Models.Flickr;
using PerformersGallery.Models.Gallery;

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