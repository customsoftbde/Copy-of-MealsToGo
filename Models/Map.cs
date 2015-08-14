using System.Collections.Generic;

namespace MealsToGo.Models
{
    public class Map
    {
        public string Name { get; set; }
        public GLatLong LatLng { get; set; }
        public int Zoom { get; set; }

        private List<Location>  locations = new List<Location>();

        public List<Location> Locations
        {
            get { return locations; }
            set { locations = value; }
        }
    }
}