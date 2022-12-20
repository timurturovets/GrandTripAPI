using Newtonsoft.Json;

namespace GrandTripAPI.Models.JSON
{
    public class LineJson
    {
        public int Id { get; set; }
        public double[][] LatLngs { get; set; }
        public Line ToDomain()
        {
            return new Line
            {
                LineId = Id,
                LatLngs = JsonConvert.SerializeObject(LatLngs)
            };
        }
    }
}