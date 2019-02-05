using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PerformersGallery.Models.FacePlusPlus
{
    public class Attributes
    {
        public int Id { get; set; }
        public Emotion Emotion { get; set; }
        public Age Age { get; set; }     
        public string CastEmotion { get; set; }
    }
}
