using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using PerformersGallery.Helpers;
using PerformersGallery.Models;
using PerformersGallery.Models.Flickr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PerformersGallery.Services
{
    public class FlickrService
    {
        private readonly IMapper _mapper;
        private readonly SecretsService _secrets;
        private readonly GalleryContext _context;
        private readonly HttpClient _http;
        private readonly FaceService _faceService;
        private readonly IMemoryCache _memoryCache;
        private readonly string _flickrUrl = "https://api.flickr.com/services/rest/";
        public FlickrService(SecretsService secrets, IMapper mapper, 
            GalleryContext context, HttpClient http, FaceService faceService,
            IMemoryCache memoryCache)
        {
            _secrets = secrets;
            _mapper = mapper;
            _context = context;
            _http = http;
            _faceService = faceService;
            _memoryCache = memoryCache;
        }

        public async Task<IActionResult> RefreshPhotos()
        {
            await ByTag();
            return new OkResult();
        }

        private async Task<IActionResult> ByTag()
        {
            var responseBody = new FlickrRoot();
            do
            {
                var response = await _http.GetAsync(BuildByTagQuery(responseBody.Photos.Page));
                responseBody = await HandleRequestAsync(responseBody, response);
            } while (responseBody.Photos.Page < responseBody.Photos.Pages);
            
            return new OkResult();
        }

        private async Task<IActionResult> ByPhotoset()
        {
            var responseBody = new FlickrRoot();
            do
            {
                var response = await _http.GetAsync(BuildByPhotosetQuery(responseBody.Photos.Page));
                responseBody = await HandleRequestAsync(responseBody, response);

            } while (responseBody.Photos.Page < responseBody.Photos.Pages);
            return new OkResult();
        }

        private async Task<FlickrRoot> HandleRequestAsync(FlickrRoot responseBody, HttpResponseMessage response)
        {
            responseBody = JsonConvert.DeserializeObject<FlickrRoot>(await response.Content.ReadAsStringAsync());
            var newPhotos = new List<string>();
            foreach (var photo in responseBody.Photos.Photo)
            {
                if (await _context.FlickrPhotos.FindAsync(photo.Id) == null)
                {
                    newPhotos.Add(CreatePhotoUrl(photo));
                    await _context.AddAsync(photo);
                }
            }
            await _faceService.AnalyzePhotos(newPhotos);
            await _context.SaveChangesAsync();
            return responseBody;
        }

        private string BuildByTagQuery(int page)
        {
            var query = new Dictionary<string, string>();
            query.Add("method", "flickr.photos.search");
            query.Add("api_key", _secrets.FlickrKey);
            query.Add("text", "int20h");
            query.Add("page", (page + 1).ToString());
            query.Add("format", "json");
            query.Add("nojsoncallback", "1");
            return RequestsHelper.BuildQuery(_flickrUrl, query);
        }

        private string BuildByPhotosetQuery(int page)
        {
            var query = new Dictionary<string, string>();
            query.Add("method", "flickr.photosets.getPhotos");
            query.Add("api_key", _secrets.FlickrKey);
            query.Add("photoset_id", "72157674388093532");
            query.Add("user_id", "144522605@N06");
            query.Add("page", (page + 1).ToString());
            query.Add("format", "json");
            query.Add("nojsoncallback", "1");
            return RequestsHelper.BuildQuery(_flickrUrl, query);
        }

        private string CreatePhotoUrl(FlickrPhoto photo)
        {
            var photoUrl = $"https://farm{photo.Farm}.staticflickr.com/{photo.Server}/{photo.Id}_{photo.Secret}.jpg";
            return photoUrl;
        }
    }
}
