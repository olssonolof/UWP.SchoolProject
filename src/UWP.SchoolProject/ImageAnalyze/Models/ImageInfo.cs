using System.Collections.Generic;

namespace ImageAnalyze.Models
{
    public class ImageInfo
    {
        public List<Categories> Categories { get; set; }
        public MetaData MetaData { get; set; }
        public Description Description { get; set; }
    }
}
