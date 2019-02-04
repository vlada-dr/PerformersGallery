using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PerformersGallery.Models.FacePlusPlus
{
    public class FacedPhoto
    {
        public Dictionary<string, Landmark> Landmark { get; set; }
        public Attributes Attributes { get; set; }
        public FaceRectangle FaceRectangle { get; set; }
        public string FaceToken { get; set; }
    }

}
