namespace GrandTripAPI.Models.JSON
{
    public class DotJson
    {
        public int Id { get; set; }
        public string Name {get;set;}
        public string Description {get;set;}
        public double PositionX {get;set;}
        public double PositionY { get; set; }

        public Dot ToDomain()
        {
            return new Dot
            {
                DotId = Id,
                DotName = Name,
                DotDescription = Description,
                PositionX = PositionX,
                PositionY = PositionY
            };
        }
    }
}