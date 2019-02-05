using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PerformersGallery.Models.Flickr
{
    public class FlickrRoot
    {
        public FlickrPhotoContainer Photos { get; set; }
        public string Stat { get; set; }
    }
}
