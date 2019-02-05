using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PerformersGallery.Models.Gallery
{
    public class GalleryViewRoot
    {
        public int Count { get; set; }
        public int LastPhotoId { get; set; }
        public string Emotion { get; set; }
    }
}
