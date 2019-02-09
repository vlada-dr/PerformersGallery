using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PerformersGallery.Models.FacePlusPlus
{
    public class FacedRoot
    {
        [Key]
        public string ImageId { get; set; }

        public string RequestId { get; set; }
        public long TimeUsed { get; set; }
        public List<FaceFound> Faces { get; set; }
    }
}