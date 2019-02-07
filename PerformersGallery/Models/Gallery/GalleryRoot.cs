using System.Collections.Generic;

namespace PerformersGallery.Models.Gallery
{
    public class GalleryRoot
    {
        public int Count { get; set; }
        public int LastPhotoId { get; set; }
        public string Emotion { get; set; }
        public List<GalleryPhoto> Photos { get; set; }
    }
}