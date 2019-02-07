using System.Collections.Generic;

namespace PerformersGallery.Models.Flickr
{
    public class FlickrPhotoContainer
    {
        public int Page { get; set; }
        public int Pages { get; set; }
        public int Perpage { get; set; }
        public string Total { get; set; }
        public List<FlickrPhoto> Photo { get; set; }
    }
}