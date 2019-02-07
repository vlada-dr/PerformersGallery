using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PerformersGallery.Models.Gallery;
using PerformersGallery.Services;

namespace PerformersGallery.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhotosController : ControllerBase
    {
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
            await _flickrService.RefreshPhotos();
            return await _galleryService.GetPhotos(galleryRoot);
        }

        [HttpGet("RefreshData")]
        public async Task<IActionResult> RefreshData()
        {
            return await _flickrService.RefreshPhotos();
        }
    }
}