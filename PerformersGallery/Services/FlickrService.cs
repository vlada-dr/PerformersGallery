using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
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
        private readonly string _flickrUrl = AdditionalData.Info.Flickr.Url.Value;
        private readonly HttpClient _http;
        private readonly SecretsService _secrets;
        private readonly IMemoryCache _memoryCache;

        public FlickrService(SecretsService secrets,
            GalleryContext context, HttpClient http, FaceService faceService, IMemoryCache memoryCache
        )
        {
            _secrets = secrets;
            _context = context;
            _http = http;
            _faceService = faceService;
            _memoryCache = memoryCache;
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
            var dbPhotos = await GetPhotosFromCache();
            foreach (var photo in responseBody.Photos.Photo)
                if (dbPhotos.Find(el => el.Id == photo.Id) == null)
                {
                    newPhotos.Add(CreatePhotoUrl(photo));
                    await _context.AddAsync(photo);
                }

            await _faceService.AnalyzePhotos(newPhotos);
            await _context.SaveChangesAsync();
            _memoryCache.Remove("FlickrPhotos");
        }

        private async Task HandleRequestAsync(FlickrPhotosetsRoot responseBody, HttpResponseMessage response)
        {
            var newPhotos = new List<string>();
            var dbPhotos = await GetPhotosFromCache();
            foreach (var photo in responseBody.Photoset.Photo)
                if (dbPhotos.Find(el => el.Id == photo.Id) == null)
                {
                    newPhotos.Add(CreatePhotoUrl(photo));
                    await _context.AddAsync(photo);
                }

            await _faceService.AnalyzePhotos(newPhotos);
            await _context.SaveChangesAsync();
            _memoryCache.Remove("FlickrPhotos");
        }

        private string BuildByTagQuery(int page)
        {
            var query = new Dictionary<string, string>
            {
                {"method", AdditionalData.Info.Flickr.MethodSearch.Value},
                {"api_key", _secrets.FlickrKey},
                {"text", AdditionalData.Info.Flickr.Tag.Value},
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
                {"method", AdditionalData.Info.Flickr.MethodPhotosets.Value},
                {"api_key", _secrets.FlickrKey},
                {"photoset_id", AdditionalData.Info.Flickr.Photoset.Value},
                {"user_id", AdditionalData.Info.Flickr.User.Value},
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

        private async Task<List<FlickrPhoto>> GetPhotosFromCache()
        {
            return await _memoryCache.GetOrCreateAsync("FlickrPhotos", el => GetPhotosFromContext());
        }

        private async Task<List<FlickrPhoto>> GetPhotosFromContext()
        {
            return await _context.FlickrPhotos
                .ToListAsync();
        }
    }
}