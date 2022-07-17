namespace LowestCostService.Domain.Models
{
    public class Package
    {
        public string SourceAddress { get; set; }
        public string DestinationAddress { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public float Length { get; set; }
    }
}
