using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PerformersGallery.Models;
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
        private readonly string _faceUrl = "https://api-us.faceplusplus.com/facepp/v3/detect";
        public FaceService(SecretsService secrets, IMapper mapper, GalleryContext context, HttpClient http)
        {
            _secrets = secrets;
            _mapper = mapper;
            _context = context;
            _http = http;
        }

        public async Task<IActionResult> AnalyzePhotos(List<string> urls)
        {
            return new OkResult();
        }

        private async Task<IActionResult> photoRequest(string url)
        {

        }

        private FormUrlEncodedContent buildRequest(string url)
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
