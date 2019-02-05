using PerformersGallery.Models.FacePlusPlus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PerformersGallery.Models.Gallery
{
    public class GalleryPhoto
    {
        public int Id { get; set; }
        public string PhotoUrl { get; set; }
        public List<FaceFound> FacesFound { get; set; }
        public DateTime TimeCreated { get; set; }
    }
}
