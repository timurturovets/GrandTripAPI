namespace GrandTripAPI.Models
{
    public class Line
    {
        public int LineId { get; set; }
        public int[] LatLngs { get; set; }
        public int RouteId { get; set; }
    }
}