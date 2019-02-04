using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PerformersGallery.Services
{
    public class FaceService
    {
        private readonly IMapper _mapper;
        private readonly SecretsService _secrets;
        public FaceService(SecretsService secrets, IMapper mapper)
        {
            _secrets = secrets;
            _mapper = mapper;
        }
    }
}
