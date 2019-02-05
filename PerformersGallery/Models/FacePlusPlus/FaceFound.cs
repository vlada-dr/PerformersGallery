using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PerformersGallery.Models.FacePlusPlus
{
    public class FaceFound
    {
        public int Id { get; set; }
        public Attributes Attributes { get; set; }
        public string FaceToken { get; set; }
    }

}
