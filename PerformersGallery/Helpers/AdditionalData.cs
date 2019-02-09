using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PerformersGallery.Helpers
{
    public static class AdditionalData
    {
        public static dynamic Info { get; set; }
        public static DateTime LastPhotoUpdate { get; set; } = DateTime.Now.AddMinutes(-10);
        public static void Initialize(string path)
        {
            string json = File.ReadAllText(path + @"\Helpers\Files\additionalData.json");
            Info = JsonConvert.DeserializeObject<object>(json);
        }

    }
}
