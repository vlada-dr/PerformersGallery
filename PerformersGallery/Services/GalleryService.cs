using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PerformersGallery.Services
{
    public class GalleryService
    {
        private readonly IMapper _mapper;
        public GalleryService(IMapper mapper)
        {
            _mapper = mapper;
        }
    }
}
