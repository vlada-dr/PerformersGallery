using AutoMapper;
using PerformersGallery.Models.Gallery;

namespace PerformersGallery.Helpers
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<GalleryViewRoot, GalleryRoot>();
            CreateMap<GalleryRoot, GalleryViewRoot>();
        }
    }
}