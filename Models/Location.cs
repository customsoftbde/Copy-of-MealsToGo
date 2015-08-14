

namespace MealsToGo.Models
{
    public class Location
    {
        public string Name { get; set; }
        public GLatLong LatLng { get; set; }
        public string Image { get; set; }
    }

    public class LocationToSearch
    {
        public int UserId { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
    }
}