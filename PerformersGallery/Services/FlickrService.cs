using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PerformersGallery.Services
{
    public class FlickrService
    {
        private readonly IMapper _mapper;
        private readonly SecretsService _secrets;
        public FlickrService(SecretsService secrets, IMapper mapper)
        {
            _secrets = secrets;
            _mapper = mapper;
        }
    }
}
