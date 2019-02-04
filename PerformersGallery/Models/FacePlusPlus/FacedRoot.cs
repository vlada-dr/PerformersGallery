using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PerformersGallery.Models.FacePlusPlus
{
    public class FacedRoot
    {
        public string ImageId { get; set; }
        public string RequestId { get; set; }
        public long TimeUsed { get; set; }
        public List<FacedPhoto> Faces { get; set; }
    }
}
