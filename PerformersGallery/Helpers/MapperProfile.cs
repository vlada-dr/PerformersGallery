using AutoMapper;
using PerformersGallery.Models.FacePlusPlus;
using PerformersGallery.Models.Flickr;
using PerformersGallery.Models.Gallery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PerformersGallery.Helpers
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<FacedPhoto, GalleryPhoto>();
        }
    }
}
