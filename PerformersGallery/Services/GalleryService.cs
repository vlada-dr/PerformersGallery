using AutoMapper;
using PerformersGallery.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PerformersGallery.Services
{
    public class GalleryService
    {
        private readonly IMapper _mapper;
        private readonly GalleryContext _context;
        public GalleryService(IMapper mapper, GalleryContext context)
        {
            _mapper = mapper;
            _context = context;
        }
    }
}
