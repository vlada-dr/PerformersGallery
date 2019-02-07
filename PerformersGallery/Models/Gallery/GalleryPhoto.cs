using System;
using System.Collections.Generic;
using PerformersGallery.Models.FacePlusPlus;

namespace PerformersGallery.Models.Gallery
{
    public class GalleryPhoto
    {
        public int Id { get; set; }
        public string PhotoUrl { get; set; }
        public List<Attributes> FacesFound { get; set; }
        public DateTime TimeCreated { get; set; }
    }
}