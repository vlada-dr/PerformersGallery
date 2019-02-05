using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PerformersGallery.Models;
using PerformersGallery.Models.FacePlusPlus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PerformersGallery.Services
{
    public class FaceService
    {
        private readonly IMapper _mapper;
        private readonly SecretsService _secrets;
        private readonly GalleryContext _context;
        private readonly HttpClient _http;
        private readonly GalleryService _galleryService;
        private readonly string _faceUrl = "https://api-us.faceplusplus.com/facepp/v3/detect";
        public FaceService(SecretsService secrets, IMapper mapper, GalleryContext context, HttpClient http, GalleryService galleryService)
        {
            _secrets = secrets;
            _mapper = mapper;
            _context = context;
            _http = http;
            _galleryService = galleryService;
        }

        public async Task<IActionResult> AnalyzePhotos(List<string> urls)
        {
            foreach(var url in urls)
            {
                await PhotoRequest(url);
            }
            return new OkResult();
        }

        private async Task<IActionResult> PhotoRequest(string url)
        {
            var response = await _http.PostAsync(_faceUrl, BuildRequest(url));
            var responseBody = JsonConvert.DeserializeObject<FacedRoot>(await response.Content.ReadAsStringAsync());
            await _galleryService.AddItem(responseBody.Faces, url);
            await _context.SaveChangesAsync();
            return new OkResult();
        }


        private FormUrlEncodedContent BuildRequest(string url)
        {
            var body = new Dictionary<string, string>
            {
                { "api_key", _secrets.FacePlusPlusKey},
                { "api_secret", _secrets.FacePlusPlusSecret},
                { "image_url", url},
                { "return_attributes", "age,emotion"},
            };
            return new FormUrlEncodedContent(body);
        }
    }
}
