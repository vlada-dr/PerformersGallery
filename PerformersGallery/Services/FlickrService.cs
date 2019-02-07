using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PerformersGallery.Helpers;
using PerformersGallery.Models;
using PerformersGallery.Models.Flickr;

namespace PerformersGallery.Services
{
    public class FlickrService
    {
        private readonly GalleryContext _context;
        private readonly FaceService _faceService;
        private readonly string _flickrUrl = "https://api.flickr.com/services/rest/";
        private readonly HttpClient _http;
        private readonly SecretsService _secrets;

        public FlickrService(SecretsService secrets,
            GalleryContext context, HttpClient http, FaceService faceService
        )
        {
            _secrets = secrets;
            _context = context;
            _http = http;
            _faceService = faceService;
        }

        public async Task<IActionResult> RefreshPhotos()
        {
            await ByTag();
            await ByPhotoset();
            return new OkResult();
        }

        private async Task<IActionResult> ByTag()
        {
            var responseBody = new FlickrRoot();
            do
            {
                var response = await _http.GetAsync(BuildByTagQuery(responseBody.Photos.Page));
                responseBody = JsonConvert.DeserializeObject<FlickrRoot>(await response.Content.ReadAsStringAsync());
                await HandleRequestAsync(responseBody, response);
            } while (responseBody.Photos.Page < responseBody.Photos.Pages);

            return new OkResult();
        }

        private async Task<IActionResult> ByPhotoset()
        {
            var responseBody = new FlickrPhotosetsRoot();
            do
            {
                var response = await _http.GetAsync(BuildByPhotosetQuery(responseBody.Photoset.Page));
                responseBody =
                    JsonConvert.DeserializeObject<FlickrPhotosetsRoot>(await response.Content.ReadAsStringAsync());
                await HandleRequestAsync(responseBody, response);
            } while (responseBody.Photoset.Page < responseBody.Photoset.Pages);

            return new OkResult();
        }

        private async Task HandleRequestAsync(FlickrRoot responseBody, HttpResponseMessage response)
        {
            var newPhotos = new List<string>();
            foreach (var photo in responseBody.Photos.Photo)
                if (await _context.FlickrPhotos.FindAsync(photo.Id) == null)
                {
                    newPhotos.Add(CreatePhotoUrl(photo));
                    await _context.AddAsync(photo);
                }

            await _faceService.AnalyzePhotos(newPhotos);
            await _context.SaveChangesAsync();
        }

        private async Task HandleRequestAsync(FlickrPhotosetsRoot responseBody, HttpResponseMessage response)
        {
            var newPhotos = new List<string>();
            foreach (var photo in responseBody.Photoset.Photo)
                if (await _context.FlickrPhotos.FindAsync(photo.Id) == null)
                {
                    newPhotos.Add(CreatePhotoUrl(photo));
                    await _context.AddAsync(photo);
                }

            await _faceService.AnalyzePhotos(newPhotos);
            await _context.SaveChangesAsync();
        }

        private string BuildByTagQuery(int page)
        {
            var query = new Dictionary<string, string>
            {
                {"method", "flickr.photos.search"},
                {"api_key", _secrets.FlickrKey},
                {"text", "int20h"},
                {"page", (page + 1).ToString()},
                {"format", "json"},
                {"nojsoncallback", "1"}
            };
            return RequestsHelper.BuildQuery(_flickrUrl, query);
        }

        private string BuildByPhotosetQuery(int page)
        {
            var query = new Dictionary<string, string>
            {
                {"method", "flickr.photosets.getPhotos"},
                {"api_key", _secrets.FlickrKey},
                {"photoset_id", "72157674388093532"},
                {"user_id", "144522605@N06"},
                {"page", (page + 1).ToString()},
                {"format", "json"},
                {"nojsoncallback", "1"}
            };
            return RequestsHelper.BuildQuery(_flickrUrl, query);
        }

        private string CreatePhotoUrl(FlickrPhoto photo)
        {
            var photoUrl = $"https://farm{photo.Farm}.staticflickr.com/{photo.Server}/{photo.Id}_{photo.Secret}.jpg";
            return photoUrl;
        }
    }
}