using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace may222017.Models
{
    public class ImageLinks
    {
        public int Id { get; set; }
        //public string Link { get; set; }
        public byte[] ImageData { get; set; }

        //public virtual Places Places { get; set; }
        public int PlaceId { get; set; }
    }
}