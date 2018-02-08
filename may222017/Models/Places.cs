using Foolproof;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace may222017.Models
{
    public class Places
    {
        public int Id { get; set; }
        
        public string status { get; set; }
        
        public string PlaceName { get; set; }

        public string Address { get; set; }
        public string Town { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }
        public string CellNumber { get; set; }
        public string Website { get; set; }
        public string EventType { get; set; }
        public bool SoundSystem { get; set; }
        public bool ColdDrink { get; set; }
        public bool BridalRoom { get; set; }
        public bool AirConditioning { get; set; }
        public bool PartyLight { get; set; }
        public bool Screen { get; set; }

        public int Accomodation { get; set; }

        public int MinPriceRange { get; set; }

        [GreaterThan("MinPriceRange",ErrorMessage ="Min. Price Range must be less than Max. Price Range")]
        public int MaxPriceRange { get; set; }

        public bool IsCateringAvailable { get; set; }
        public List<string> ImageLink { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual ICollection<ImageLinks> Links { get; set; }

        public Places()
        {
            status = "Pending";
        }
    }
}