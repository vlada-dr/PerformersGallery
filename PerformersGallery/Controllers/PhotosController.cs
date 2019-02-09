using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PerformersGallery.Helpers;
using PerformersGallery.Models.Gallery;
using PerformersGallery.Services;

namespace PerformersGallery.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhotosController : ControllerBase
    {
        private static DateTime LastUpdate { get; set; }
        private readonly FlickrService _flickrService;
        private readonly GalleryService _galleryService;

        public PhotosController(GalleryService galleryService, FlickrService flickrService)
        {
            _galleryService = galleryService;
            _flickrService = flickrService;
        }

        [HttpGet]
        public async Task<GalleryRoot> GetGallery([FromQuery] GalleryViewRoot galleryRoot)
        {
            if (AdditionalData.LastPhotoUpdate < DateTime.Now.AddMinutes(-5) 
                && galleryRoot.LastPhotoId == 0) {
                AdditionalData.LastPhotoUpdate = DateTime.Now;
                await _flickrService.RefreshPhotos();
            }
            return await _galleryService.GetPhotos(galleryRoot);
        }

    }
}