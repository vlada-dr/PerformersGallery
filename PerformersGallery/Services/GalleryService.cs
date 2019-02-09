using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using PerformersGallery.Models;
using PerformersGallery.Models.FacePlusPlus;
using PerformersGallery.Models.Gallery;

namespace PerformersGallery.Services
{
    public class GalleryService
    {
        private readonly GalleryContext _context;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;

        public GalleryService(IMapper mapper, GalleryContext context, IMemoryCache memoryCache
        )
        {
            _mapper = mapper;
            _context = context;
            _memoryCache = memoryCache;
        }

        public async Task<IActionResult> AddItem(List<FaceFound> faces, string imageUrl)
        {
            var newfaces = FindEmotions(faces);
            var newPhoto = new GalleryPhoto
            {
                PhotoUrl = imageUrl,
                FacesFound = newfaces,
                TimeCreated = DateTime.Now
            };
            await _context.GalleryPhotos.AddAsync(newPhoto);
            await _context.SaveChangesAsync();
            _memoryCache.Remove("GalleryPhotos");
            return new OkResult();
        }

        public async Task<GalleryRoot> GetPhotos(GalleryViewRoot requestBody)
        {
            var photos = await GetPhotosFromCache();
            if (!string.IsNullOrEmpty(requestBody.Emotion))
                photos = photos.FindAll(el => el.FacesFound.Any(face =>
                    face != null && string.Equals(face.CastEmotion, requestBody.Emotion
                    )));
            var index = 0;
            if (requestBody.LastPhotoId != 0)
                index = photos.IndexOf(photos.Find(el => el.Id == requestBody.LastPhotoId)) + 1;
            var returnBody = _mapper.Map<GalleryRoot>(requestBody);
            returnBody.Photos = photos.Skip(index).Take(requestBody.Count).ToList();
            returnBody.LastPhotoId = returnBody.Photos.Count > 0 ? returnBody.Photos.Last().Id : 0;
            return returnBody;
        }

        private List<Attributes> FindEmotions(IEnumerable<FaceFound> faces)
        {
            var result = new List<Attributes>();
            if(faces != null)
            {
                foreach (var face in faces)
                {
                    double max = 0;
                    var emotion = "";
                    if (face.Attributes?.Emotion != null)
                    {
                        foreach (var propertyInfo in face.Attributes.Emotion.GetType().GetProperties())
                            if (propertyInfo.Name != "Id")
                            {
                                var value = (double)propertyInfo.GetValue(face.Attributes.Emotion);
                                if (value >= max)
                                {
                                    max = value;
                                    emotion = propertyInfo.Name;
                                }
                            }

                        face.Attributes.CastEmotion = emotion;
                    }
                    if(face.Attributes!=null) result.Add(face.Attributes);
                }
            }

            return result;
        }

        private async Task<List<GalleryPhoto>> GetPhotosFromCache()
        {
            return await _memoryCache.GetOrCreateAsync("GalleryPhotos", el => GetPhotosFromContext());
        }

        private async Task<List<GalleryPhoto>> GetPhotosFromContext()
        {
            return await _context.GalleryPhotos
                .Include("FacesFound.Emotion")
                .OrderByDescending(el => el.TimeCreated).ToListAsync();
        }
    }
}