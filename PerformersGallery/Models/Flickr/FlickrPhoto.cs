using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PerformersGallery.Models.Flickr
{
    public class FlickrPhoto
    {
        public string Id { get; set; }
        public string Owner { get; set; }
        public string Secret { get; set; }
        public string Server { get; set; }
        public int Farm { get; set; }
        public string Title { get; set; }
        public int IsPublic { get; set; }
        public int IsFriend { get; set; }
        public int IsFamily { get; set; }
    }
}
