using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PerformersGallery.Models.Gallery;
using PerformersGallery.Services;

namespace PerformersGallery.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhotosController : ControllerBase
    {
        private readonly GalleryService _galleryService;
        private readonly FlickrService _flickrService;
        public PhotosController(GalleryService galleryService, FlickrService flickrService)
        {
            _galleryService = galleryService;
            _flickrService = flickrService;
        }

        [HttpGet]
        public async Task<GalleryRoot> GetGallery([FromQuery] GalleryViewRoot galleryRoot)
        {
            return await _galleryService.GetPhotos(galleryRoot);
        }

        [HttpGet("RefreshData")]
        public async Task<IActionResult> RefreshData()
        {
            return await _flickrService.RefreshPhotos();
        }
    }
}